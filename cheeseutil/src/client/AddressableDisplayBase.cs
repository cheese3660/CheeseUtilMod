using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities;
using LogicWorld.References;

namespace CheeseUtilMod.Client
{
    public abstract class AddressableDisplayBase : PrefabVariantInfo
    {
        public override abstract string ComponentTextID { get; }
        public abstract int addressLines { get; }
        private static float pixelScale = 0.25f; // 4 pixels per tile

        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(addressLines * 2 + 2, 0);
        }

        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier)
        {
            var blocks = new List<Block>();
            int resolution = 1 << addressLines;
            float scale = resolution * pixelScale;
            float blockY = resolution * (pixelScale - 1);
            /*for (int y = 0; y < resolution; y++)
            {
                float blockX = 0;
                for (int x = 0; x < resolution; x++)
                {
                    blocks.Add(
                        new Block
                        {
                            RawColor = Color24.Black,
                            Position = new Vector3(x, y, 0),
                            Scale = new Vector3(pixelScale, pixelScale, 0.25f)
                        }
                    );
                    blockX += pixelScale;
                }
                blockY -= pixelScale;
            }*/
            bool odd = (int)(scale % 2) == 1;
            blocks.Add(
                new Block
                {
                    RawColor = new Color24(0x7f7f7f),
                    Position = new Vector3(odd ? 0 : 0.5f, 0, -0.25f),
                    Scale = new Vector3(scale, scale, 0.25f),
                }
            );
            List<ComponentInput> inputs = new List<ComponentInput>();
            float currentX = 0.1666666666666666666666f;
            float currentY = 0.1666666666666666666666f;
            for (int i = 0; i < addressLines; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(currentX, currentY, -0.125f),
                    Rotation = new Vector3(90f,0f,0f),
                    Length = 0.5f,
                });
                currentX += 0.3333333333333333333333333333f;
                if (currentX >= scale)
                {
                    currentX = 0.166666666666666666666666666f;
                    currentY += 0.33333333333333333333333333f;
                }
            }
            currentX = 0.166666666666666666666666666666666666f;
            currentY += 0.33333333333333333333333333333333f;
            for (int i = 0; i < addressLines; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(currentX, currentY, -0.125f),
                    Rotation = new Vector3(90f, 0f, 0f),
                    Length = 0.5f,
                });
                currentX += 0.3333333333333333333333333333f;
                if (currentX >= scale)
                {
                    currentX = 0.166666666666666666666666666f;
                    currentY += 0.33333333333333333333333333f;
                }
            }
            currentX = 0.166666666666666666666666666666666666f;
            currentY += 0.33333333333333333333333333333333f;
            for (int i = 0; i < 2; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(currentX, currentY, -0.125f),
                    Rotation = new Vector3(90f, 0f, 0f),
                    Length = 0.5f,
                });
                currentX += 0.3333333333333333333333333333f;
                if (currentX >= scale)
                {
                    currentX = 0.166666666666666666666666666f;
                    currentY += 0.33333333333333333333333333f;
                }
            }
            return new ComponentVariant
            {
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
