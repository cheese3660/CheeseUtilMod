using LogicWorld.Server.Circuitry;


//Currently this is onyl one byte of ram per
namespace CheeseUtilMod.Components
{
    public class VariantRam : LogicComponent
    {

        private static int PEG_CS = 0;
        private static int PEG_W = 1;
        private static int PEG_D0 = 2;
        private static int PEG_D1 = 3;
        private static int PEG_D2 = 4;
        private static int PEG_D3 = 5;
        private static int PEG_D4 = 6;
        private static int PEG_D5 = 7;
        private static int PEG_D6 = 8;
        private static int PEG_D7 = 9;

        public override bool HasPersistentValues
        {
            get
            {
                return true;
            }
        }

        byte[] memory; //This is a list of the memory of the component, should default to empty
        int num_bytes;
        int addr_size;

        protected override void Initialize() {
            int num_inputs = base.ComponentData.InputCount - 10; //-8 for the data -1 for chip select, -1 for write
                                                                //Chip select and write are on the top
            addr_size = num_inputs;
            num_bytes = 2 << num_inputs;
            memory = new byte[num_bytes];
        }

        private int getPegShifted(int peg, int shift) {
            int bas = base.Inputs[peg].On ? 1 : 0;
            return bas << shift;
        }

        protected override void DoLogicUpdate() {
            //Get the address
            int address = 0;
            for (int i = 0; i < addr_size; i++) {
                address |= getPegShifted(i+10,i);
            }
            //First check inputs
            if (base.Inputs[PEG_W].On) {
                int data = getPegShifted(PEG_D0, 0);
                data |= getPegShifted(PEG_D1, 1);
                data |= getPegShifted(PEG_D2, 2);
                data |= getPegShifted(PEG_D3, 3);
                data |= getPegShifted(PEG_D4, 4);
                data |= getPegShifted(PEG_D5, 5);
                data |= getPegShifted(PEG_D6, 6);
                data |= getPegShifted(PEG_D7, 7);
                memory[address] = (byte)data;
            }

            //And then do output
            if (base.Inputs[PEG_CS].On) {
                byte d = memory[address];
                for (int i = 0; i < 8; i++) {
                    base.Outputs[i].On = (d & 1) == 1;
                    d >>= 1;
                }
            } else {
                for (int i = 0; i < 8; i++) base.Outputs[i].On = false;
            }
        }

        protected override byte[] SerializeCustomData() {
            return memory;
        }

        protected override void DeserializeData(byte[] data) {
            memory = data;
        }
    }
}