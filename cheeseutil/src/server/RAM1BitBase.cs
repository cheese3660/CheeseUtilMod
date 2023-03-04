using LICC;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Text;
using CheeseUtilMod.Server;
using CheeseUtilMod.Shared.CustomData;

using System.Timers;
using System.IO;
using System.IO.Compression;

namespace CheeseUtilMod.Components
{
    public abstract class RAM1BitBase : LogicComponent<IRamData>
    {
        public override bool HasPersistentValues
        {
            get
            {
                return true;
            }
        }

        public abstract int addressLines { get; }
        private static int PEG_CS = 0;
        private static int PEG_W = 1;
        private static int PEG_L = 2;
        private static int PEG_D = 3;
        private bool loadfromsave;
        private byte[] memory;
        protected override void Initialize()
        {
            memory = new byte[(1 << addressLines)/8];
            loadfromsave = true;
        }
        public override void Dispose()
        {
        }
        private int getPegShifted(int peg, int shift)
        {
            int bas = base.Inputs[peg].On ? 1 : 0;
            return bas << shift;
        }
        protected override void DoLogicUpdate()
        {
            int address = 0;
            for (int i = 0; i < addressLines; i++)
            {
                address |= getPegShifted(i + 3 + 1, i);
            }
            int byteAddress = address / 8;
            int bitIndex = address % 8;
            byte mask = (byte)(1 << bitIndex);
            if (base.Inputs[PEG_W].On)
            {
                if (base.Inputs[PEG_D].On)
                {
                    memory[byteAddress] |= mask;
                }
                else
                {
                    memory[byteAddress] &= (byte)~mask;
                }
            }
            if (base.Inputs[PEG_CS].On)
            {
                base.Outputs[0].On = (memory[byteAddress] & mask) != 0;
            } else
            {
                base.Outputs[0].On = false;
            }
        }
        //protected override byte[] SerializeCustomData()
        //{
        //    return memory;
        //}

        //protected override void DeserializeData(byte[] data)
        //{
        //    if (data != null)
        //        memory = data;
        //}
        protected override void OnCustomDataUpdated()
        {

            if ((loadfromsave && Data.Data != null || Data.state == 1 && Data.ClientIncomingData != null))
            {
                var to_load_from = Data.Data;
                if (Data.state == 1)
                {
                    Logger.Info("Loading data from client");
                    to_load_from = Data.ClientIncomingData;
                }
                MemoryStream stream = new MemoryStream(to_load_from);
                stream.Position = 0;
				byte[] mem1 = new byte[memory.Length];
                try
                {
                    DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                    int bytesRead;
					int nextStartIndex = 0;
					while((bytesRead = decompressor.Read(mem1, nextStartIndex, mem1.Length-nextStartIndex)) > 0){
						nextStartIndex += bytesRead;
					}
                    Buffer.BlockCopy(mem1, 0, memory, 0, mem1.Length);
                }
				catch(Exception ex)
                {
					Logger.Error("[CheeseUtilmod] Loading data from client failed with exception: "+ex.ToString());
                }
                loadfromsave = false;
                if (Data.state == 1)
                {
                    Data.state = 0;
                    Data.ClientIncomingData = new byte[0];
                }
                QueueLogicUpdate();
            }
        }
        protected override void SetDataDefaultValues()
        {
            Data.Data = new byte[0];
            Data.state = 0;
            Data.ClientIncomingData = new byte[0];
        }
        protected override void SavePersistentValuesToCustomData()
        {

            MemoryStream memstream = new MemoryStream();
            memstream.Position = 0;
            DeflateStream compressor = new DeflateStream(memstream, CompressionLevel.Optimal, true);
            compressor.Write(memory, 0, memory.Length);
            compressor.Flush();
            int length = (int)memstream.Position;
            memstream.Position = 0;
            byte[] bytes = new byte[length];
            memstream.Read(bytes, 0, length);
            Data.Data = bytes;
        }
    }

}
