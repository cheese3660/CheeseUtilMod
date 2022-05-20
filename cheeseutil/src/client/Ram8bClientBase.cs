using JimmysUnityUtilities;
using LogicWorld.ClientCode.Resizing;
using LogicWorld.Interfaces;
using LogicWorld.Rendering.Chunks;
using LogicWorld.Rendering.Components;
using LogicWorld.ClientCode;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

using CheeseUtilMod.Shared.CustomData;
using LICC;
using System.Timers;

namespace CheeseUtilMod.Client
{
    public abstract class Ram8bClientBase : ComponentClientCode<IRamData>, FileLoadable
    {
        public abstract int addressLines { get; }
        public byte[] memory;
        private static int PEG_L = 2;
        protected override void Initialize()
        {
            memory = new byte[(1 << addressLines)];
            CheeseUtilClient.fileLoadables.Add(this);
        }
        protected override void OnComponentDestroyed()
        {
            CheeseUtilClient.fileLoadables.Remove(this);
        }
        public void Load(byte[] filedata, LineWriter writer)
        {
            if (GetInputState(PEG_L))
            {
                var max_index = (1 << addressLines);
                if (filedata.Length < max_index)
                {
                    max_index = filedata.Length;
                }
                for (int i = 0; i < max_index; i++)
                {
                    memory[i] = filedata[i];
                }
            }
            SendDataToServer();
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
            Data.ClientIncomingData = Compress(memory);
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
