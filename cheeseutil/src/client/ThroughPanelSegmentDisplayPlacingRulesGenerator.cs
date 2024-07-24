using LogicAPI.Data;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

namespace CheeseUtilMod.Client
{
    public class ThroughPanelSegmentDisplayPlacingRulesGenerator : DynamicPlacingRulesGenerator<int, IThroughPanelSegmentDisplayData>
    {
        protected override int GetIdentifierFor(ComponentData componentData)
            => componentData.InputCount;

        protected override int GetIdentifierFor(ComponentData componentData, IThroughPanelSegmentDisplayData data)
            => data.size;

        protected override int GetDefaultIdentifier()
            => ThroughPanelSegmentDisplayDataInit.DefaultSize;

        protected override PlacingRules GeneratePlacingRulesFor(int size)
            => PlacingRules.FlippablePanelOfSize(size, size * 2);
    }
}
