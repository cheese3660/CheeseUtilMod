using JimmysUnityUtilities;

namespace CheeseUtilMod.Client
{
    public interface IThroughPanelSegmentDisplayData
    {
        Color24 color { get; set; }
        int size { get; set; }
    }

    public static class ThroughPanelSegmentDisplayDataInit
    {
        public const int DefaultSize = 1;

        public static void Initialize(this IThroughPanelSegmentDisplayData data)
        {
            data.color = Color24.Amber;
            data.size = DefaultSize;
        }
    }
}
