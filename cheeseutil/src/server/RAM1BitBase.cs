using LogicAPI.Server.Components;

namespace CheeseUtilMod.Components
{
    public abstract class RAM1BitBase : LogicComponent
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
        private static int PEG_D = 2;
        private byte[] memory;
        protected override void Initialize()
        {
            memory = new byte[(1 << addressLines)/8];
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
                address |= getPegShifted(i + 2 + 1, i);
            }
            int byteAddress = address / 8;
            int bitIndex = address % 8;
            byte mask = (byte)(1 << bitIndex);
            if (base.Inputs[PEG_W].On)
            {
                if (base.Inputs[PEG_D].On)
                {
                    memory[byteAddress] |= mask;
                }
                else
                {
                    memory[byteAddress] &= (byte)~mask;
                }
            }
            if (base.Inputs[PEG_CS].On)
            {
                base.Outputs[0].On = (memory[byteAddress] & mask) != 0;
            } else
            {
                base.Outputs[0].On = false;
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
