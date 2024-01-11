using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public class RamResizableClient : ComponentClientCode<IRamResizableData>, FileLoadable
    {
        private static int PEG_L = 2;

        public void Load(byte[] filedata, LineWriter writer, bool force)
        {
            if (force || GetInputState(PEG_L))
            {
                Data.ClientIncomingData = Compress(filedata);
                Data.State = 1;
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

        protected override void SetDataDefaultValues()
        {
            Data.AddressWidth = 1;
            Data.BitWidth = 1;
            Data.State = 0;
            Data.ClientIncomingData = new byte[0];
            Data.Data = new byte[0];
        }
    }
}
