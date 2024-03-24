using LogicWorld.Rendering.Components;
using System;
using System.IO;
using System.IO.Compression;

using CheeseUtilMod.Shared.CustomData;
using LICC;

namespace CheeseUtilMod.Client
{
    public abstract class Ram16bClientBase : ComponentClientCode<IRamData>, FileLoadable, FileSavable
    {
        public abstract int addressLines { get; }
        public ushort[] memory;
        private static int PEG_L = 2;

        protected override void Initialize()
        {
            memory = new ushort[1 << addressLines];
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
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        protected void SendDataToServer()
        {
            Logger.Info("Sending data to client");
            byte[] mem1 = new byte[memory.Length * 2];
            Buffer.BlockCopy(memory, 0, mem1, 0, mem1.Length);
            Data.ClientIncomingData = Compress(mem1);
            Data.State = 1;
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
        
        private byte[] Decompress()
        {
            MemoryStream stream = new MemoryStream(Data.Data); // We load from here
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
