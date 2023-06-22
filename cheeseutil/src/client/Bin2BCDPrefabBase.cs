using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities;

namespace CheeseUtilMod.Client
{
    public abstract class Bin2BCDPrefabBase : PrefabVariantInfo
    {
        public override abstract string ComponentTextID { get; }
        public abstract int bits { get; }

        public static int digitsFromBits(int bits)
        {
            ulong maxNum = (1ul << bits)-1ul;
            int numDigits = 1;
            while (maxNum > 10ul)
            {
                numDigits += 1;
                maxNum /= 10ul;
            }
            return numDigits;
        }

        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(bits, digitsFromBits(bits)*4);
        }

        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier)
        {
            PlacingRules placingRules = new PlacingRules();
            var block = new Block
            {
                RawColor = new Color24(127, 127, 127),
            };
            block.Scale = new Vector3(identifier.OutputCount / 4, 4, 1);
            int w = identifier.OutputCount / 4;
            int w2 = identifier.InputCount / 4;
            if (w % 2 == 0)
                block.Position = new Vector3(0.5f, 0f, 0f);
            float outputPositionX = 0.5f;
            float outputPositionZ = 0.5f;
            List<ComponentOutput> outputs = new List<ComponentOutput>();
            float offset = 0f;
            if (w > 2)
            {
                offset = 0.5f;
            }
            for (int i = 0; i < identifier.OutputCount; i+=4)
            {
                float outputPositionY = 0.5f;
                for (int j = 0; j < 4; j++)
                {
                    outputs.Add(new ComponentOutput
                    {
                        Position = new Vector3(outputPositionX-(w2/2f)- offset, outputPositionY, outputPositionZ),
                        Rotation = new Vector3(90f, 0f, 0f),
                    });
                    outputPositionY += 1f;
                }
                outputPositionX += 1f;
            }
            float currentX = 0.166666666666666666666666f;
            float currentY = 0.166666666666666666666666f;
            List<ComponentInput> inputs = new List<ComponentInput>();
            for (int i = 0; i < identifier.InputCount; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(currentX-(w2/2f)- offset, currentY, -0.5f),
                    Rotation = new Vector3(-90f, 0f, 0f),
                    Length = 0.5f
                });
                currentX += 0.33333333333333333333f;
                if (currentX >= w)
                {
                    currentX = 0.16666666666666666f;
                    currentY += 0.3333333333333333f;
                }
            }
            return new ComponentVariant
            {
                VariantPlacingRules = placingRules,
                VariantPrefab = new Prefab
                {
                    Blocks = new Block[] { block },
                    Inputs = inputs.ToArray(),
                    Outputs = outputs.ToArray()
                }
            };
        }
    }
}
