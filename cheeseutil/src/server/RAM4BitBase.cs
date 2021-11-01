using LogicAPI.Server.Components;

namespace CheeseUtilMod.Components
{
    public abstract class RAM4BitBase : LogicComponent
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
        private static int PEG_D0 = 2;
        private static int PEG_D1 = 3;
        private static int PEG_D2 = 4;
        private static int PEG_D3 = 5;


        private byte[] memory;
        protected override void Initialize()
        {
            memory = new byte[(1 << addressLines) / 2];
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
                address |= getPegShifted(i + 2 + 4, i);
            }
            int byteAddress = address / 2;
            int nibbleIndex = address % 2;
            int mask = 15 << (nibbleIndex << 2);
            if (base.Inputs[PEG_W].On)
            {
                int data = getPegShifted(PEG_D0, 0);
                data |= getPegShifted(PEG_D1, 1);
                data |= getPegShifted(PEG_D2, 2);
                data |= getPegShifted(PEG_D3, 3);
                memory[byteAddress] &= (byte)~mask;
                memory[byteAddress] |= (byte)(data << (nibbleIndex << 2));
            }
            if (base.Inputs[PEG_CS].On)
            {
                int data = memory[byteAddress] & mask;
                data >>= (nibbleIndex << 2);
                for (int i = 0; i < 4; i++)
                {
                    base.Outputs[i].On = (data & 1) == 1;
                    data >>= 1;
                }
            } else
            {
                for (int i = 0; i < 4; i++)
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
    }
}
