using LogicAPI.Server.Components;

namespace CheeseUtilMod.Components
{
    public abstract class Bin2BCDBase : LogicComponent
    {
        public abstract int bits { get; }
        private int digits;
        public static int digitsFromBits(int bits)
        {
            ulong maxNum = (1ul << bits) - 1ul;
            int numDigits = 1;
            while (maxNum > 10ul)
            {
                numDigits += 1;
                maxNum /= 10ul;
            }
            return numDigits;
        }

        protected override void Initialize()
        {
            digits = digitsFromBits(bits);
        }
        private ulong getPegShifted(int peg, int shift)
        {
            ulong bas = base.Inputs[peg].On ? 1ul : 0ul;
            return bas << shift;
        }

        private void setDigitPeg(int digit, byte value)
        {
            int basePeg = digit * 4;
            for (int i = 0; i < 4; i++)
            {
                Outputs[basePeg + i].On = (value & 1) == 1;
                value >>= 1;
            }
        }

        protected override void DoLogicUpdate()
        {

            byte[] digibytes = new byte[digits];
            ulong val = 0;
            for (int i = 0; i < bits; i++)
            {
                val |= getPegShifted(i, i);
            }

            int digiIndex = 0;
            while (val > 0)
            {
                digibytes[digiIndex++] = (byte)(val % 10);
                val /= 10;
            }
            for (int i = 0; i < digits; i++)
            {
                setDigitPeg(i, digibytes[i]);
            }
        }
    }
}
