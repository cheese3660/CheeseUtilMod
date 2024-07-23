using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;

namespace CheeseUtilMod.Client
{
    public class SevenSegPlacingRulesGenerator : DynamicPlacingRulesGenerator<int>
    {
        public int scale; // This is 1, 2, or 4

        public override void Setup(ComponentInfo info)
        {
            scale = info.CodeInfoInts[0];
        }

        protected override int GetIdentifierFor(ComponentData componentData)
            => 0;

        protected override PlacingRules GeneratePlacingRulesFor(int _)
        {
            return new PlacingRules
            {
                AllowFineRotation = scale == 1,
                CanBeFlipped = false,
            };
        }
    }
}
