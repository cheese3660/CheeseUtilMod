using LogicWorld.Rendering.Components;
using JimmysUnityUtilities;
using LogicWorld.ClientCode;
using LogicAPI.Data;

namespace CheeseUtilMod.Client
{
    class HexDisplay : ComponentClientCode<HexDisplay.IData>, IColorableClientCode
    {
        public Color24 Color { get => Data.color; set => Data.color = value; }

        public string ColorsFileKey => "SevenSegments";

        public float MinColorValue => 0.0f;

        private static bool[][] numbers = new bool[][]
        {
            new bool[] {true, true, true, true, true, true, false}, //0
            new bool[] {false, true, true, false, false, false, false }, //1
            new bool[] {true, true, false, true, true, false, true }, //2
            new bool[] {true, true, true, true, false, false, true }, //3
            new bool[] {false, true, true, false, false, true, true }, //4
            new bool[] {true, false, true, true, false, true, true }, //5
            new bool[] {true, false, true, true, true, true, true }, //6
            new bool[] {true, true, true, false, false, false, false }, //7
            new bool[] {true, true, true, true, true, true, true }, //8
            new bool[] {true, true, true, true, false, true, true }, //9
            new bool[] {true, true, true, false, true, true, true }, //A
            new bool[] {false, false, true, true, true, true, true }, //B
            new bool[] {true, false, false, true, true, true, false }, //C
            new bool[] {false, true, true, true, true, false, true }, //D
            new bool[] {true, false, false, true, true, true, true }, //E
            new bool[] {true, false, false, false, true, true, true }, //F
        };

        protected override void DataUpdate()
        {
            QueueFrameUpdate();
        }

        protected override void FrameUpdate()
        {
            GpuColor col = GpuColorConversionExtensions.ToGpuColor(Data.color);
            GpuColor black = GpuColorConversionExtensions.ToGpuColor(Color24.Black);
            int index = GetInputState(0) ? 1 : 0;
            index |= GetInputState(1) ? 2 : 0;
            index |= GetInputState(2) ? 4 : 0;
            index |= GetInputState(3) ? 8 : 0;

            for (int i = 0; i < 7; i++)
            {
                SetBlockColor(numbers[index][i] ? col : black, i);
            }
        }

        protected override void SetDataDefaultValues()
        {
            Data.color = Color24.Amber;
        }

        public interface IData
        {
            Color24 color { get; set; }
        }
    }
}
