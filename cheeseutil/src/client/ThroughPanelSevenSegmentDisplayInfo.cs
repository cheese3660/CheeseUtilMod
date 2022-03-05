using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseUtilMod.Client
{
    public class ThroughPanelSevenSegmentDisplayInfo : ThroughPanelSegmentDisplayVariantInfo
    {
        public override bool Hex => false;

        public override string ComponentTextID => "CheeseUtilMod.SevenSegResizable";
    }
}
