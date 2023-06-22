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
            if (force || GetInputState(PEG_L) == true)
            {
                Data.ClientIncomingData = Compress(filedata);
                Data.state = 1;
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
        protected override void SetDataDefaultValues()
        {
            Data.addressWidth = 1;
            Data.bitWidth = 1;
            Data.state = 0;
            Data.ClientIncomingData = new byte[0];
            Data.Data = new byte[0];
        }
    }
}
