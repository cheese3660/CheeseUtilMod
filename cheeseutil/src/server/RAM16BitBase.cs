﻿using LogicAPI.Server.Components;
using System;

namespace CheeseUtilMod.Components
{
    public abstract class RAM16BitBase : LogicComponent
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

        private ushort[] memory;
        protected override void Initialize()
        {
            memory = new ushort[(1 << addressLines)];
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
                address |= getPegShifted(i + 2 + 16, i);
            }
            if (base.Inputs[PEG_W].On)
            {
                int data = 0;
                for (int i = 0; i < 16; i++)
                {
                    data |= getPegShifted(i + 2, i);
                }
                memory[address] = (ushort)data;
            }
            if (base.Inputs[PEG_CS].On)
            {
                int data = memory[address];
                for (int i = 0; i < 16; i++)
                {
                    base.Outputs[i].On = (data & 1) == 1;
                    data >>= 1;
                }
            } else
            {

                for (int i = 0; i < 16; i++)
                {
                    base.Outputs[i].On = false;
                }
            }
        }
        protected override byte[] SerializeCustomData()
        {
            if (memory != null)
            {
                byte[] newData = new byte[memory.Length * 2];
                Buffer.BlockCopy(memory, 0, newData, 0, memory.Length * 2);
                return newData;
            }
            else
            {
                return null;
            }
        }

        protected override void DeserializeData(byte[] data)
        {
            if (data != null)
            {
                memory = new ushort[data.Length / 2];
                Buffer.BlockCopy(data, 0, memory, 0, data.Length);
            }
        }
    }
}