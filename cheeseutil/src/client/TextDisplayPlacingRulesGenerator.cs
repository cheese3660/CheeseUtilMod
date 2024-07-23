using CheeseUtilMod.Shared.CustomData;
using LogicAPI.Data;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

namespace CheeseUtilMod.Client
{
    public sealed class TextDisplayPlacingRulesGenerator : DynamicPlacingRulesGenerator<(int sizeX, int sizeZ), ITextConsoleData>
    {
        protected override (int sizeX, int sizeZ) GetIdentifierFor(ComponentData componentData, ITextConsoleData data)
            => (data.SizeX, data.SizeZ);

        protected override (int sizeX, int sizeZ) GetDefaultIdentifier()
            => (TextConsoleDataInit.DefaultSizeX, TextConsoleDataInit.DefaultSizeZ);

        protected override PlacingRules GeneratePlacingRulesFor((int sizeX, int sizeZ) identifier)
            => PlacingRules.FlippablePanelOfSize(identifier.sizeX, identifier.sizeZ);
    }
}
