using LogicWorld.Rendering.Components;
using JimmysUnityUtilities;
using LogicWorld.ClientCode;
using LogicWorld.ClientCode.Resizing;
using LogicWorld.SharedCode.Components;
using UnityEngine;
using System;

namespace CheeseUtilMod.Client
{
    class SevenSegmentDisplayResizable : ComponentClientCode<SevenSegmentDisplayResizable.IData>, IColorableClientCode, IResizableX
    {
        public Color24 Color { get => Data.color; set => Data.color = value; }

        public string ColorsFileKey => "SevenSegments";


        public float MinColorValue => 0.0f;

        public int SizeX { get => Data.size; set => Data.size = value; }

        public int MinX => 1;

        public int MaxX => 32;
        private int prevSize;

        public float GridIntervalX => 1.0f;
        private static float segmentWidth = 0.15f; //0.17 at a normal 1x2 scale
        private static float segmentHeight = 0.6f; //0.7 at a normal 1x2 scale
        private static float segmentMiddle = 0.5f;
        private static Vector2[][] sevenSegSegmentLocationsAndScales = new Vector2[][]
        {
            new Vector2[]{new Vector2(0.00f, segmentMiddle + segmentHeight + segmentWidth), new Vector2(segmentHeight, segmentWidth) }, //1
            new Vector2[]{new Vector2(segmentHeight/2+segmentWidth/2, segmentMiddle + (segmentHeight + segmentWidth)/2), new Vector2(segmentWidth, segmentHeight) }, //2
            new Vector2[]{new Vector2(segmentHeight / 2 + segmentWidth / 2, segmentMiddle - (segmentHeight + segmentWidth) / 2), new Vector2(segmentWidth, segmentHeight) }, //3
            new Vector2[]{new Vector2(0.00f, segmentMiddle - segmentHeight - segmentWidth), new Vector2(segmentHeight, segmentWidth) }, //4
            new Vector2[]{new Vector2(-(segmentHeight / 2 + segmentWidth / 2), segmentMiddle - (segmentHeight + segmentWidth) / 2), new Vector2(segmentWidth, segmentHeight) }, //5
            new Vector2[]{new Vector2(-(segmentHeight / 2 + segmentWidth / 2), segmentMiddle + (segmentHeight + segmentWidth) / 2), new Vector2(segmentWidth, segmentHeight) }, //6
            new Vector2[]{new Vector2(0.00f, segmentMiddle), new Vector2(segmentHeight, segmentWidth) }, //7
        };
        protected override void DataUpdate()
        {
            if (SizeX != prevSize)
            {
                float newScale = SizeX;
                float offset = (SizeX - 1) * 0.5f;
                for (int i = 0; i < 7; i++)
                {
                    var locAndScale = sevenSegSegmentLocationsAndScales[i];
                    var x = locAndScale[0].x;
                    var y = locAndScale[0].y;
                    var w = locAndScale[1].x;
                    var h = locAndScale[1].y;
                    SetBlockPosition(i, new Vector3(x * newScale + offset, 0.25f, y * newScale + offset));
                    SetBlockScale(i, new Vector3(w * newScale, 0.25f, h * newScale));
                }
                SetBlockScale(7, new Vector3(newScale, 0.25f, 2 * newScale));
                SetBlockPosition(7, new Vector3(offset, 0, offset * 2 + 0.5f));
                for (int i = 0; i < 7; i++)
                {
                    var row = i / (SizeX * 3);
                    var col = i % (SizeX * 3);
                    byte ii = Convert.ToByte(i);
                    SetInputPosition(ii, new Vector3((col - 1) / 3f + 0.0416666667f, -1f, (row - 1) / 3f + 0.0416666667f));
                }
                SetBlockScale(8, new Vector3(1f, 1f, newScale));
                prevSize = SizeX;
            }
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
                }
                else
                {
                    SetBlockColor(Color24.Black, i);
                }
            }
            base.FrameUpdate();
        }
        public override PlacingRules GenerateDynamicPlacingRules()
        {
            return PlacingRules.FlippablePanelOfSize(SizeX, SizeX*2);
        }

        protected override void SetDataDefaultValues()
        {
            Data.color = Color24.Amber;
            Data.size = 1;
        }

        public interface IData
        {
            Color24 color { get; set; }
            int size { get; set; }
        }
    }
}
