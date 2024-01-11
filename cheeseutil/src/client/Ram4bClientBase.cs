using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public abstract class Ram4bClientBase : ComponentClientCode<IRamData>, FileLoadable
    {
        public abstract int addressLines { get; }
        public byte[] memory;
        private static int PEG_L = 2;

        protected override void Initialize()
        {
            memory = new byte[1 << addressLines / 2];
            CheeseUtilClient.fileLoadables.Add(this);
        }

        protected override void OnComponentDestroyed()
        {
            CheeseUtilClient.fileLoadables.Remove(this);
        }

        public void Load(byte[] filedata, LineWriter writer, bool force)
        {
            if (force | GetInputState(PEG_L))
            {
                var max_index = (1 << addressLines / 2);
                if (filedata.Length * 2 < max_index)
                {
                    max_index = filedata.Length * 2;
                }
                for (int i = 0; i < max_index; i++)
                {
                    memory[i] = filedata[i];
                }
                SendDataToServer();
            }
        }

        static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        protected void SendDataToServer()
        {
            Data.ClientIncomingData = Compress(memory);
            Data.State = 1;
        }

        protected override void SetDataDefaultValues()
        {
            Data.Data = new byte[0];
            Data.State = 0;
            Data.ClientIncomingData = new byte[0];
        }
    }
}
