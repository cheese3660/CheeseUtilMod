using LogicWorld.Rendering.Components;
using JimmysUnityUtilities;

namespace CheeseUtilMod.Client
{
    class SevenSegmentDisplay : ComponentClientCode
    {
        protected override void DataUpdate()
        {
            base.QueueFrameUpdate();
        }
        protected override void FrameUpdate()
        {
            for (int i = 0; i < 7; i++)
            {
                bool set = GetInputState(i);
                if (set)
                {
                    SetBlockColor(Color24.Amber, i);
                } else
                {
                    SetBlockColor(Color24.Black, i);
                }
            }
            base.FrameUpdate();
        }
    }
}
