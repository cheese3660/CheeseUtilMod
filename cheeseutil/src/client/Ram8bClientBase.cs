using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public class Ram8bClientBase : ComponentClientCode<IRamData>, FileLoadable
    {
        public int addressLines;
        public byte[] memory;
        private static int PEG_L = 2;

        protected override void Initialize()
        {
            addressLines = CodeInfoInts[0];
            memory = new byte[1 << addressLines];
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
                var max_index = 1 << addressLines;
                if (filedata.Length < max_index)
                {
                    max_index = filedata.Length;
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
            Data.initialize();
        }
    }
}
