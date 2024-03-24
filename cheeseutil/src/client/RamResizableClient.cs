using System;
using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public class RamResizableClient : ComponentClientCode<IRamResizableData>, FileLoadable, FileSavable
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
            Data.initialize();
        }

        private Action<byte[]> _lastOnSave = null;
        public void Save(bool force, Action<byte[]> onSave)
        {

            if (force || GetInputState(PEG_L))
            {
                _lastOnSave = onSave;
                Data.State = 2;
            }
        }
        private static int widthToBytes(int width)
        {
            int bas = width / 8;
            int mod = width % 8 > 0 ? 1 : 0;
            return bas+mod;
        }
        
        private byte[] Decompress()
        {
            MemoryStream stream = new MemoryStream(Data.Data); // We load from here
            stream.Position = 0;
            stream.Position = 0;
            byte[] mem1 = new byte[(1 << Data.AddressWidth) * widthToBytes(Data.BitWidth)];
            try
            {
                DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                int bytesRead;
                int nextStartIndex = 0;
                while((bytesRead = decompressor.Read(mem1, nextStartIndex, mem1.Length - nextStartIndex)) > 0){
                    nextStartIndex += bytesRead;
                }
            }
            catch(Exception ex)
            {
                Logger.Error("[CheeseUtilMod] Loading data from client failed with exception: " + ex);
            }
            return mem1;
        }

        protected override void DataUpdate()
        {
            if (Data.State == 3)
            {
                Logger.Info("Received data from client!");
                // Now we must decompress all the data as well
                // We reset the state data
                if (_lastOnSave != null) _lastOnSave(Decompress());
                Data.State = 0;
            }
        }
    }
}
