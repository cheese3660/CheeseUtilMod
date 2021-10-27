using LogicWorld.ClientCode;
using LogicWorld.Interfaces;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using JimmysUnityUtilities;
using System;

namespace CheeseUtilMod.Client {
    public class RamVariantInfo : PrefabVariantInfo {
        public override string ComponentTextID => "CheeseUtilMod.VariantRam";
        private static Color24 blockColor = new Color24(127,127,127);
        public override PrefabVariantIdentifier GetDefaultComponentVariant()
        {
            return new PrefabVariantIdentifier(11, 8); //One address
        }
        public override ComponentVariant GenerateVariant(PrefabVariantIdentifier identifier) {
            if (GameObject.Find("---Variant Ram Menu Object---") == null) {
                var obj = (GameObject)UnityEngine.Object.Instantiate(new GameObject());
                obj.name = "---Variant Ram Menu Object---";
                obj.AddComponent(typeof(CheeseUtilMod.UI.VariantRamMenu));
            }
            if (identifier.OutputCount != 8) {
                throw new Exception("Variant rams must have exactly 8 outputs");
            }
            if (identifier.InputCount < 11) {
                throw new Exception("Variant rams must have at least 1 address line");
            }
            int numAddrs = identifier.InputCount - 10;
            PlacingRules placingRules = new PlacingRules();
            placingRules.AllowFineRotation = false;
            var prefabBlock = new Block
            {
                RawColor = RamVariantInfo.blockColor
            };
            List<ComponentOutput> outputs = new List<ComponentOutput>();
            float current_width = 8.0f;
            if (numAddrs > 8) {
                current_width = (float)numAddrs;
            }
            float baseOutputX = (-current_width/2f)+0.5f;
            //Generate all the outputs
            for (int i = 0; i < 8; i++) {
                outputs.Add(new ComponentOutput{
                    Position = new Vector3(baseOutputX, 1f, 1f),
                    Rotation = new Vector3(90f, 0f, 0f),
                });
                baseOutputX += 1;
            }
            //Generate the chip select and write lines
            List<ComponentInput> inputs = new List<ComponentInput>();
            //Chip select
            inputs.Add(new ComponentInput{
                Position = new Vector3(-0.5f, 2f, 0f),
                Rotation = new Vector3(0f,0f,0f),
                Length = 0.6f,
            });
            //Write
            inputs.Add(new ComponentInput{
                Position = new Vector3(0.5f, 2f, 0f),
                Rotation = new Vector3(0f,0f,0f),
                Length = 0.6f
            });
            //Data input pins
            float baseInputX = (-current_width/2f)+0.5f;
            for (int i = 0; i < 8; i++) {
                inputs.Add(new ComponentInput{
                    Position = new Vector3(baseInputX, 0.5f, -1f),
                    Rotation = new Vector3(-90f, 0f, 0f),
                    Length = 0.6f
                });
                baseInputX += 1;
            }
            //Address pins
            baseInputX = (-current_width/2f)+0.5f;
            for (int i = 0; i < numAddrs; i++) {
                inputs.Add(new ComponentInput{
                    Position = new Vector3(baseInputX, 1.5f, -1f),
                    Rotation = new Vector3(-90f, 0f, 0f),
                    Length = 0.6f
                });
            }
            prefabBlock.Scale = new Vector3(current_width, 2f, 2f);
            placingRules.GridPlacingDimensions = new Vector2Int((int)current_width, 2);
            return new ComponentVariant
            {
                VariantPlacingRules = placingRules,
                VariantPrefab = new Prefab {
                    Blocks = new Block[] {prefabBlock},
                    Outputs = outputs.ToArray(),
                    Inputs = inputs.ToArray()
                }
            };
        }
    }
}