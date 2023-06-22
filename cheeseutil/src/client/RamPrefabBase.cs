using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities;


namespace CheeseUtilMod.Client
{
    public abstract class RamPrefabBase : PrefabVariantInfo
    {
        public override abstract string ComponentTextID { get; }
        public abstract int addressSize { get; }
        public abstract int dataSize { get; }
        private static Color24 blockColor = new Color24(127, 127, 127);
        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(3+addressSize+dataSize, dataSize);
        }
        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier)
        {
            PlacingRules placingRules = new PlacingRules();
            placingRules.AllowFineRotation = false;
            var prefabBlock = new Block
            {
                RawColor = RamPrefabBase.blockColor
            };
            List<ComponentOutput> outputs = new List<ComponentOutput>();
            float current_width = dataSize;
            if (addressSize > dataSize)
            {
                current_width = addressSize;
            }
            float baseOutputX = (-current_width / 2f) + 1f;
            //Generate all the outputs
            for (int i = 0; i < dataSize; i++)
            {
                outputs.Add(new ComponentOutput
                {
                    Position = new Vector3(baseOutputX, 1f, 1.5f),
                    Rotation = new Vector3(90f, 0f, 0f),
                });
                baseOutputX += 1;
            }
            //Generate the chip select and write lines
            List<ComponentInput> inputs = new List<ComponentInput>();
            //Chip select
            inputs.Add(new ComponentInput
            {
                Position = new Vector3(-1f, 2f, 0f),
                Rotation = new Vector3(0f, 0f, 0f),
                Length = 0.6f,
            });
            //Write
            inputs.Add(new ComponentInput
            {
                Position = new Vector3(0f, 2f, 0f),
                Rotation = new Vector3(0f, 0f, 0f),
                Length = 0.5f
            });
            //Load
            inputs.Add(new ComponentInput
            {
                Position = new Vector3(1f, 2f, 0f),
                Rotation = new Vector3(0f, 0f, 0f),
                Length = 0.4f
            });
            //Data input pins
            float baseInputX = (-current_width / 2f) + 1f;
            //Make the inputs different lengths to signal endianness
            //Start with the smallest length one being little endian
            float start_length = 0.2f;
            float end_length = 0.6f;
            float step_length = (end_length - start_length) / dataSize;
            float length = start_length;
            for (int i = 0; i < dataSize; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(baseInputX, 0.5f, -0.5f),
                    Rotation = new Vector3(-90f, 0f, 0f),
                    Length = length
                });
                baseInputX += 1;
                length += step_length;
            }
            //Address pins
            baseInputX = (-current_width / 2f) + 1f;
            step_length = (end_length - start_length) / addressSize;
            length = start_length;
            for (int i = 0; i < addressSize; i++)
            {
                inputs.Add(new ComponentInput
                {
                    Position = new Vector3(baseInputX, 1.5f, -0.5f),
                    Rotation = new Vector3(-90f, 0f, 0f),
                    Length = length
                });
                baseInputX += 1;
                length += step_length;
            }
            prefabBlock.Scale = new Vector3(current_width, 2f, 2f);
            prefabBlock.Position = new Vector3(0.5f, 0f, 0.5f);
            placingRules.GridPlacingDimensions = new Vector2Int((int)current_width, 2);
            return new ComponentVariant
            {
                VariantPlacingRules = placingRules,
                VariantPrefab = new Prefab
                {
                    Blocks = new Block[] { prefabBlock },
                    Outputs = outputs.ToArray(),
                    Inputs = inputs.ToArray()
                }
            };
        }
    }
}
