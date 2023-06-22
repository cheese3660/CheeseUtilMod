using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities;

namespace CheeseUtilMod.Client
{
    public abstract class SevenSegPrefabBase : PrefabVariantInfo
    {
        public abstract override string ComponentTextID { get; }
        public abstract int scale { get; } //This is 1, 2, or 4
        public abstract bool hex { get; }

        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(hex ? 4 : 7, 0);
        }

        /* Display Order
         *  111
         * 6   2
         * 6   2
         * 6   2
         *  777
         * 5   3
         * 5   3
         * 5   3
         *  444
         */
        private static float segmentWidth = 0.17f; //0.17 at a normal 1x2 scale
        private static float segmentHeight = 0.7f; //0.7 at a normal 1x2 scale

        //Contains X and Y along display
        private static Vector2[][] sevenSegSegmentLocationsAndScales = new Vector2[][]
        {
            new Vector2[]{new Vector2(0.00f, 1.80f), new Vector2(segmentHeight, segmentWidth) }, //1
            new Vector2[]{new Vector2(0.40f, 1.10f), new Vector2(segmentWidth, segmentHeight) }, //2
            new Vector2[]{new Vector2(0.40f, 0.20f), new Vector2(segmentWidth, segmentHeight) }, //3
            new Vector2[]{new Vector2(0.00f, 0.03f), new Vector2(segmentHeight, segmentWidth) }, //4
            new Vector2[]{new Vector2(-0.37f, 0.20f), new Vector2(segmentWidth, segmentHeight) }, //5
            new Vector2[]{new Vector2(-0.37f, 1.10f), new Vector2(segmentWidth, segmentHeight) }, //6
            new Vector2[]{new Vector2(0.00f, 0.90f), new Vector2(segmentHeight, 0.2f) }, //7
        };

        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier)
        {
            PlacingRules placingRules = new PlacingRules();
            placingRules.AllowFineRotation = scale == 1;
            placingRules.CanBeFlipped = false;
            var blocks = new List<Block>();
            for (int i = 0; i < 7; i++)
            {
                var locAndScale = sevenSegSegmentLocationsAndScales[i];
                var x = locAndScale[0].x * scale;
                var y = locAndScale[0].y * scale;
                var w = locAndScale[1].x * scale;
                var h = locAndScale[1].y * scale;
                blocks.Add(
                    new Block
                    {
                        RawColor = Color24.Black,
                        Position = new Vector3(x, y,-0.5f + 0.125f),
                        Scale = new Vector3(w, h, 0.25f)
                    }
                );
            }
            blocks.Add(
                    new Block
                    {
                        RawColor = Color24.Black,
                        Position = new Vector3(0, 0, -0.125f),
                        Scale = new Vector3(scale, 2f * scale, 0.25f)
                    }
                );
            List<ComponentInput> inputs = new List<ComponentInput>();
            float currentX = 0.166666666666666666666666f;
            float currentY = 0.166666666666666666666666f;
            float lengthStart = 0.4f;
            float lengthEnd = 0.7f;
            float lengthStep = (lengthEnd - lengthStart) / identifier.InputCount;
            float length = lengthStart;
            for (int i = 0; i < identifier.InputCount; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(currentX - scale / 2f, currentY, 0f),
                    Rotation = new Vector3(90f, 0f, 0f),
                    Length = length
                });
                currentX += 0.33333333333333333333f;
                if (currentX >= scale)
                {
                    currentX = 0.16666666666666666f;
                    currentY += 0.3333333333333333f;
                }
                length += lengthStep;
            }

            return new ComponentVariant
            {
                VariantPlacingRules = placingRules,
                VariantPrefab = new Prefab
                {
                    Blocks = blocks.ToArray(),
                    Inputs = inputs.ToArray(),
                    Outputs = new ComponentOutput[] { }
                }
            };
        }
    }
}
