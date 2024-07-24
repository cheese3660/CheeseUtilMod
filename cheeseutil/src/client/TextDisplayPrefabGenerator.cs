using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System;
using UnityEngine;

namespace CheeseUtilMod.Client
{
    //Modified: https://github.com/ShadowAA55/HMM/blob/main/HMM/src/client/Output.cs
    public class TextDisplayPrefabGenerator : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
    {
        protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
            => (componentData.InputCount, componentData.OutputCount);

        public override (int inputCount, int outputCount) GetDefaultPegCounts()
            => (28, 0);

        protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) identifier)
        {
            if (identifier.OutputCount != 0)
            {
                throw new Exception("Text Displays cannot have any outputs");
            }
            if (identifier.InputCount != 28)
            {
                throw new Exception("Text Displays must have 28 inputs");
            }
            ComponentInput[] array = new ComponentInput[identifier.InputCount];
            for (int i = 0; i < array.Length; i++)
            {
                int row, col;
                if (i < 12)
                {
                    row = i / 6;
                    col = i % 6;
                }
                else
                {
                    var i2 = i - 12;
                    row = (i2 / 8) + 2;
                    col = i2 % 8;
                }
                float length = col / 8f * 0.6f + 0.4f;
                array[i] = new ComponentInput
                {
                    Position = new Vector3(col, row, 0f),
                    Rotation = new Vector3(180f, 0f, 0f),
                    Length = length
                };
            }
            return new Prefab
            {
                Blocks = new Block[2]
                {
                    new Block
                    {
                        Position = new Vector3(-0.5f, 0.333333343f, -0.5f),
                        Rotation = new Vector3(0f, 180f, 180f),
                        MeshName = "OriginCube_OpenBottom",
                        RawColor = Color24.Black
                    },
                    new Block
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
                    }
                },
                Inputs = array
            };
        }
    }
}
