using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using UnityEngine;
using LogicAPI.Data;

namespace CheeseUtilMod.Client
{
    public class RamResizablePlacingRulesGenerator : DynamicPlacingRulesGenerator<(int InputCount, int OutputCount)>
    {
        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
            => (componentData.InputCount, componentData.OutputCount);

        protected override PlacingRules GeneratePlacingRulesFor((int InputCount, int OutputCount) identifier)
        {
            var dataSize = identifier.OutputCount;
            var addressSize = identifier.InputCount - 3 - dataSize;
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
