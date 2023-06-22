using JimmysUnityUtilities;
using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System;
using UnityEngine;

namespace CheeseUtilMod.Client
{
    public abstract class ThroughPanelSegmentDisplayVariantInfo : PrefabVariantInfo
    {
        public abstract bool Hex { get; }
        public override abstract string ComponentTextID { get; }
        //Generate with a default size of 1x2

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

        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier)
        {
            Block[] blocks = new Block[9];
            ComponentInput[] inputs = new ComponentInput[identifier.InputCount];
            for (int i = 0; i < 7; i++)
            {
                var locAndScale = sevenSegSegmentLocationsAndScales[i];
                var x = locAndScale[0].x;
                var y = locAndScale[0].y;
                var w = locAndScale[1].x;
                var h = locAndScale[1].y;
                blocks[i] =new Block
                    {
                        RawColor = Color24.Black,
                        Position = new Vector3(x, 0.25f, y),
                        Scale = new Vector3(w, 0.125f, h)
                    };
            }
            blocks[7] = new Block
            {
                RawColor = Color24.Black,
                Position = new Vector3(0, 0f, 0.5f),
                Scale = new Vector3(1f,0.25f, 2f)
            };
            blocks[8] = new Block
            {
                Position = new Vector3(-0.45f, 0f, -0.45f),
                Rotation = new Vector3(180f, 270f, 0f),
                MeshName = "OriginCube_OpenBottom",
                ColliderData = new ColliderData
                {
                    Transform = new ColliderTransform
                    {
                        LocalScale = new Vector3(1f, 0.4f, 1f),
                        LocalPosition = new Vector3(0f, 0.6f, 0f)
                    }
                }
            };
            for (int i = 0; i < inputs.Length; i++)
            {
                var row = i / 3;
                var col = i % 3;
                var length = i / Convert.ToSingle(inputs.Length) * 0.6f + 0.4f;
                inputs[i] = new ComponentInput
                {
                    Position = new Vector3((col-1)/3f + 0.0416666667f, -1f, (row-1)/ 3f + 0.0416666667f),
                    Rotation = new Vector3(180f, 0f, 0f),
                    Length = length
                };
            }
            ComponentVariant componentVariant = new ComponentVariant();
            componentVariant.VariantPrefab = new Prefab
            {
                Blocks = blocks,
                Inputs = inputs,
            };
            return componentVariant;
        }

        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(Hex ? 4 : 7, 0);
        }
    }
}
