using LogicWorld.Rendering.Components;
using JimmysUnityUtilities;
using LogicWorld.ClientCode;

namespace CheeseUtilMod.Client
{
    class SevenSegmentDisplay : ComponentClientCode<SevenSegmentDisplay.IData>, IColorableClientCode
    {
        public Color24 Color { get => Data.color; set => Data.color = value; }

        public string ColorsFileKey => "SevenSegments";


        public float MinColorValue => 0.0f;

        protected override void DataUpdate()
        {
            QueueFrameUpdate();
        }
        protected override void FrameUpdate()
        {
            for (int i = 0; i < 7; i++)
            {
                bool set = GetInputState(i);
                if (set)
                {
                    SetBlockColor(Data.color, i);
                } else
                {
                    SetBlockColor(Color24.Black, i);
                }
            }
            base.FrameUpdate();
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
