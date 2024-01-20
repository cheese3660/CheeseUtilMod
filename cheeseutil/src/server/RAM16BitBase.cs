using LogicWorld.Server.Circuitry;
using System;
using CheeseUtilMod.Shared.CustomData;

using System.IO;
using System.IO.Compression;

namespace CheeseUtilMod.Components
{
    public abstract class RAM16BitBase : LogicComponent<IRamData>
    {
        public override bool HasPersistentValues => true;

        public abstract int addressLines { get; }
        private static int PEG_CS = 0;
        private static int PEG_W = 1;
        private static int PEG_L = 2;

        private ushort[] memory;
        private bool loadfromsave;

        protected override void Initialize()
        {
            loadfromsave = true;
            memory = new ushort[1 << addressLines];
        }

        public override void Dispose()
        {
        }

        private int getPegShifted(int peg, int shift)
        {
            int bas = Inputs[peg].On ? 1 : 0;
            return bas << shift;
        }

        protected override void DoLogicUpdate()
        {
            int address = 0;
            for (int i = 0; i < addressLines; i++)
            {
                address |= getPegShifted(i + 3 + 16, i);
            }
            if (Inputs[PEG_W].On)
            {
                int data = 0;
                for (int i = 0; i < 16; i++)
                {
                    data |= getPegShifted(i + 3, i);
                }
                memory[address] = (ushort)data;
            }
            if (Inputs[PEG_CS].On)
            {
                int data = memory[address];
                for (int i = 0; i < 16; i++)
                {
                    Outputs[i].On = (data & 1) == 1;
                    data >>= 1;
                }
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    Outputs[i].On = false;
                }
            }
        }

        protected override void OnCustomDataUpdated()
        {
            if (loadfromsave && Data.Data != null || Data.State == 1 && Data.ClientIncomingData != null)
            {
                var to_load_from = Data.Data;
                if (Data.State == 1)
                {
                    Logger.Info("Loading data from client");
                    to_load_from = Data.ClientIncomingData;
                }
                MemoryStream stream = new MemoryStream(to_load_from);
                stream.Position = 0;
                byte[] mem1 = new byte[memory.Length * 2];
                try
                {
                    DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                    int bytesRead;
                    int nextStartIndex = 0;
                    while((bytesRead = decompressor.Read(mem1, nextStartIndex, mem1.Length - nextStartIndex)) > 0){
                        nextStartIndex += bytesRead;
                    }
                    Buffer.BlockCopy(mem1, 0, memory, 0, mem1.Length);
                }
                catch(Exception ex)
                {
                    Logger.Error("[CheeseUtilMod] Loading data from client failed with exception: " + ex);
                }
                loadfromsave = false;
                if (Data.State == 1)
                {
                    Data.State = 0;
                    Data.ClientIncomingData = new byte[0];
                }
                QueueLogicUpdate();
            }
        }

        protected override void SetDataDefaultValues()
        {
            Data.Data = new byte[0];
            Data.State = 0;
            Data.ClientIncomingData = new byte[0];
        }

        protected override void SavePersistentValuesToCustomData()
        {
            MemoryStream memstream = new MemoryStream();
            memstream.Position = 0;
            DeflateStream compressor = new DeflateStream(memstream, CompressionLevel.Optimal, true);
            byte[] mem1 = new byte[memory.Length * 2];
            Buffer.BlockCopy(memory, 0, mem1, 0, mem1.Length);
            compressor.Write(mem1,0,mem1.Length);
            compressor.Flush();
            int length = (int)memstream.Position;
            memstream.Position = 0;
            byte[] bytes = new byte[length];
            memstream.Read(bytes, 0, length);
            Data.Data = bytes;
        }
    }
}
