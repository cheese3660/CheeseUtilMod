using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using UnityEngine;
using LogicAPI.Data;

namespace CheeseUtilMod.Client
{
    public class RamPlacingRulesGenerator : DynamicPlacingRulesGenerator<int>
    {
        public int addressSize;
        public int dataSize;

        public override void Setup(ComponentInfo info)
        {
            addressSize = info.CodeInfoInts[0];
            dataSize = info.CodeInfoInts[1];
        }

        protected override int GetIdentifierFor(ComponentData componentData)
            => 0;

        protected override PlacingRules GeneratePlacingRulesFor(int identifier)
        {
            float current_width = dataSize;
            if (addressSize > dataSize)
            {
                current_width = addressSize;
            }
            return new PlacingRules
            {
                AllowFineRotation = false,
                GridPlacingDimensions = new Vector2Int((int)current_width, 2),
            };
        }
    }
}
