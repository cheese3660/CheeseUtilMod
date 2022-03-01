﻿using LogicAPI.Server.Components;
using System;
using System.Text;
using CheeseUtilMod.Server;

namespace CheeseUtilMod.Components
{
    public abstract class RAM8BitBase : LogicComponent, FileLoadable
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

        private byte[] memory;
        protected override void Initialize()
        {
            memory = new byte[(1 << addressLines)];
            CheeseUtilServer.fileLoadables.Add(this);
        }
        public override void Dispose()
        {
            CheeseUtilServer.fileLoadables.Remove(this);
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
                address |= getPegShifted(i + 3 + 8, i);
            }
            if (base.Inputs[PEG_W].On)
            {
                int data = 0;
                for (int i = 0; i < 8; i++)
                {
                    data |= getPegShifted(i + 3, i);
                }
                memory[address] = (byte)data;
            }
            if (base.Inputs[PEG_CS].On)
            {
                int data = memory[address];
                for (int i = 0; i < 8; i++)
                {
                    base.Outputs[i].On = (data & 1) == 1;
                    data >>= 1;
                }
            } else
            {
                for (int i = 0; i < 8; i++)
                {
                    base.Outputs[i].On = false;
                }
            }
        }
        protected override byte[] SerializeCustomData()
        {
            return memory;
        }

        protected override void DeserializeData(byte[] data)
        {
            if (data != null)
                memory = data;
        }
        public void Load(byte[] filedata, LICC.LineWriter writer)
        {
            if (base.Inputs[PEG_L].On)
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
            QueueLogicUpdate();
        }
    }
}
