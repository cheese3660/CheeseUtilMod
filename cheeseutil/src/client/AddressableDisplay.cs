using LogicWorld.Rendering.Components;
using JimmysUnityUtilities;
using LogicWorld.ClientCode;
using System.Linq;
using UnityEngine;
using LogicWorld.Interfaces.Building;

namespace CheeseUtilMod.Client
{
    class AddressableDisplay : ComponentClientCode, IColorableClientCode
    {
        public Color24 Color { get; set; }

        public string ColorsFileKey => "AddressableDisplays";

        public float MinColorValue => 0.0f;

        protected int resolution;
        protected int address_size; //The address size for x *or* y
        protected byte[] pixels; //This is a bitarray basically;

        private bool textureUpdated = false;
        private Texture2D tex;
        private Texture2D blk;
        private static Color32 black = new Color32(0, 0, 0, 255);
        //private Material material;
        private Transform objTrans;
        uint frame = 0;
        bool toggle = false;
        protected override void Initialize()
        {

            address_size = (InputCount - 2) >> 1;
            resolution = 1 << address_size;
            pixels = new byte[(resolution * resolution) >> 3];
            Color = Color24.Amber;
            tex = new Texture2D(resolution, resolution);
            blk = new Texture2D(resolution, resolution);
            tex.filterMode = FilterMode.Point;
            updatePixels();
        }

        protected override void InitializeInWorld()
        {
            Material material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = tex;
            var block0 = GetBlockEntity(0);
            var obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            obj.transform.position = block0.WorldPosition+block0.up*(block0.Scale.y/2f) - block0.forward*(block0.Scale.z/2f + 0.001f);
            obj.transform.localScale = new Vector3(block0.Scale.x,0,block0.Scale.y)/10f;
            obj.transform.rotation = block0.WorldRotation * Quaternion.Euler(0,-90,90);
            obj.GetComponent<Renderer>().material = material;
            objTrans = obj.transform;
        }

        protected override void OnComponentDestroyed()
        {
            if (objTrans)
            {
                Object.Destroy(objTrans.gameObject);
            }
        }

        protected override void DeserializeData(byte[] data)
        {
            if (data != null)
            {
                var colorBase = data.Take(3).ToArray();
                Color = new Color24(colorBase[0], colorBase[1], colorBase[2]);
                pixels = data.Skip(3).ToArray();
                //Now set every single pixel
                textureUpdated = true;
                QueueFrameUpdate();
            }
        }

        protected void updatePixels()
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int address = (y << address_size) | x;
                    int byteAddress = address >> 3;
                    int bitIndex = address & 0b111;
                    byte mask = (byte)(1 << bitIndex);
                    if ((pixels[byteAddress] & mask) != 0)
                    {
                        tex.SetPixel(x, y, new Color32(Color.r, Color.g, Color.b, 255));
                    }
                    else
                    {
                        tex.SetPixel(x, y, black);
                    }
                }
            }
            textureUpdated = true;
            QueueFrameUpdate();
        }

        protected override void DataUpdate()
        {
            QueueFrameUpdate();
        }

        protected override void FrameUpdate()
        {
            if (GetInputState(address_size * 2 + 1))
            {
                int address = 0;
                for (int i = 0; i < address_size * 2; i++)
                {
                    address |= (GetInputState(i) ? 1 : 0) << i;
                }
                int byteAddress = address >> 3;
                int bitIndex = address & 0b111;
                int x = (address) & (1 << address_size) - 1;
                int y = (address) >> address_size;
                byte mask = (byte)(1 << bitIndex);
                if (GetInputState(address_size * 2))
                {
                    tex.SetPixel(x, y, new Color32(Color.r, Color.g, Color.b, 255));
                    pixels[byteAddress] |= mask;
                }
                else
                {
                    tex.SetPixel(x, y, black);
                    pixels[byteAddress] &= (byte)~mask;
                }
                textureUpdated = true;
            }
            if (textureUpdated && objTrans)
            {
                tex.Apply();
                textureUpdated = false;
            }
        }

        public override byte[] SerializeCustomData()
        {
            var data = new byte[pixels.Length + 3];
            data[0] = Color.r;
            data[1] = Color.g;
            data[2] = Color.b;
            pixels.CopyTo(data, 3);
            return data;
        }
        protected override ChildPlacementInfo GenerateChildPlacementInfo()
        {
            return base.GenerateChildPlacementInfo();
        }
    }
}
