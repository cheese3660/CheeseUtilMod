using LogicWorld.Rendering.Components;
using System;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public abstract class Ram16bClientBase : ComponentClientCode<IRamData>, FileLoadable
    {
        public abstract int addressLines { get; }
        public ushort[] memory;
        private static int PEG_L = 2;
        protected override void Initialize()
        {
            memory = new ushort[(1 << addressLines)];
            CheeseUtilClient.fileLoadables.Add(this);
        }
        protected override void OnComponentDestroyed()
        {
            CheeseUtilClient.fileLoadables.Remove(this);
        }
        public void Load(byte[] filedata, LineWriter writer, bool force)
        {
            if (force || GetInputState(PEG_L))
            {
                var max_index = (1 << addressLines);
                if (filedata.Length / 2 < max_index)
                {
                    max_index = filedata.Length / 2;
                }
                for (int i = 0; i < max_index; i++)
                {
                    ushort lo = filedata[i * 2];
                    ushort hi = filedata[i * 2 + 1];
                    ushort val = (ushort)(lo | (hi << 8));
                    memory[i] = val;
                }
                SendDataToServer();
            }
        }
        static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        protected void SendDataToServer()
        {
            Logger.Info("Sending data to server");
            byte[] mem1 = new byte[memory.Length * 2];
            Buffer.BlockCopy(memory, 0, mem1, 0, mem1.Length);
            Data.ClientIncomingData = Compress(mem1);
            Data.state = 1;
        }

        protected override void SetDataDefaultValues()
        {
            Data.Data = new byte[0];
            Data.state = 0;
            Data.ClientIncomingData = new byte[0];
        }
    }
}
