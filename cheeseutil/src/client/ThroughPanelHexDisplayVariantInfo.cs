using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseUtilMod.Client
{
    public class ThroughPanelHexDisplayVariantInfo : ThroughPanelSegmentDisplayVariantInfo
    {
        public override bool Hex => true;

        public override string ComponentTextID => "CheeseUtilMod.HexResizable";
    }
}
