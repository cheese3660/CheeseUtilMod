using JimmysUnityUtilities;
using LogicWorld.ClientCode.Resizing;
using LogicWorld.Interfaces;
using LogicWorld.Rendering.Chunks;
using LogicWorld.Rendering.Components;
using LogicWorld.ClientCode;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

using CheeseUtilMod.Shared.CustomData;
using System.Timers;

namespace CheeseUtilMod.Client
{
    //Modified version of: https://github.com/ShadowAA55/HMM/blob/main/HMM/src/client/Output.cs
    public class TextDisplay : ComponentClientCode<ITextConsoleData>, IResizableX, IResizableZ, ICustomCubeArrowHeight, IColorableClientCode
    {
        byte[] mem;
        Timer cursorUpdateTimer;
        bool cursorState;

        private int previousSizeX;
        public int SizeX { get => Data.SizeX; set => Data.SizeX = value; }

        public int MinX => 4;

        public int MaxX => 64;

        public float GridIntervalX => 1f;

        private int previousSizeZ;
        public int SizeZ { get => Data.SizeZ; set => Data.SizeZ = value; }

        public int MinZ => 4;

        public int MaxZ => 64;

        public float GridIntervalZ => 1f;

        public float CubeArrowHeight => 355f / (678f * (float)Math.PI);

        public Color24 Color { get => Data.color; set => Data.color = value; }

        public string ColorsFileKey => "TextDisplayColors";

        public float MinColorValue => 0.0f;

        Texture2D screen;
        int screenWidth;
        int screenHeight;
        protected override void Initialize()
        {
            mem = new byte[64 * 64];
            screenWidth = 64;
            screenHeight = 64;
            cursorUpdateTimer = new Timer();
            cursorUpdateTimer.Interval = 1000;
            cursorUpdateTimer.Elapsed += new ElapsedEventHandler(cursorSwitch);
            cursorUpdateTimer.AutoReset = true;
            cursorUpdateTimer.Start();
            cursorState = false;
            
            if (screen == null)
            {
                screen = new Texture2D(screenWidth, screenHeight);
                screen.filterMode = FilterMode.Point;
            }
        }
        static int PEG_CURSOR_ENABLED = 23;
        protected void cursorSwitch(object source, ElapsedEventArgs e)
        {
            cursorState = !cursorState;
            QueueFrameUpdate();
        }
        protected override void SetDataDefaultValues()
        {
            Data.SizeX = 4;
            Data.SizeZ = 4;
            //Data.TextData = null;
            Data.color = Color24.Amber;
            Data.CursorX = 0;
            Data.CursorY = 0;
        }
        protected override void DataUpdate()
        {
            QueueFrameUpdate();
            if (SizeX != previousSizeX || SizeZ != previousSizeZ)
            {
                setupInputBlock();
                // Panel
                SetBlockScale(0, new Vector3(SizeX, 0.333333343f, SizeZ));
                previousSizeX = SizeX;
                previousSizeZ = SizeZ;
                // Screen
                SetDecorationPosition(0, new Vector3((SizeX / 2f - 0.5f) * 0.3f, 0.3334f * 0.3f, (SizeZ / 2f - 0.5f) * 0.3f));
                SetDecorationScale(0, new Vector3(SizeX * 0.3f, SizeZ * 0.3f, 1));
                screenWidth = SizeX * 16;
                screenHeight = SizeZ * 16;
                screen.Resize(screenWidth, screenHeight);
            }
            void setupInputBlock()
            {
                int blocksizex = Math.Min(SizeX, 8);
                int blocksizez = Math.Min(SizeZ, 4);
                SetBlockScale(1, new Vector3(blocksizez - 0.1f, 5f / 6f, blocksizex - 0.1f));
                float xoffset = (0.45f - 2.9f / 16f) * (8f - blocksizex) / 5f;
                float zoffset = (0.45f - 1.9f / 12f) * (6f - blocksizez) / 4f;
                for (int i = 0; i < 8; i++)
                {
                    if (i < 6)
                    {
                        SetInputPosition((byte)i, new Vector3(i * (blocksizex - 0.1f) / 8f - xoffset, -5f / 6f, 0f * (blocksizez - 0.1f) / 6f - zoffset));
                        SetInputPosition((byte)(i + 6), new Vector3(i * (blocksizex - 0.1f) / 8f - xoffset, -5f / 6f, 1f * (blocksizez - 0.1f) / 6f - zoffset));
                    }
                    SetInputPosition((byte)(i + 12), new Vector3(i * (blocksizex - 0.1f) / 8f - xoffset, -5f / 6f, 2f * (blocksizez - 0.1f) / 6f - zoffset));
                    SetInputPosition((byte)(i + 20), new Vector3(i * (blocksizex - 0.1f) / 8f - xoffset, -5f / 6f, 3f * (blocksizez - 0.1f) / 6f - zoffset));
                }
            }
        }

        protected override void FrameUpdate()
        {
            if (Data.TextData != null)
            {
                MemoryStream stream = new MemoryStream(Data.TextData);
                stream.Position = 0;
                DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                decompressor.Read(mem, 0, 64 * 64);
                for (int x = 0; x < SizeX; x++)
                {
                    for (int y = 0; y < SizeZ; y++)
                    {
                        bool invert = (GetInputState(PEG_CURSOR_ENABLED) && cursorState) && (x == Data.CursorX && y == Data.CursorY);
                        int index = y * 64 + x;
                        byte chr = mem[index];
                        Font.SetChar(screen, invert, chr, x, (SizeZ-1)-y, new Color(Color.r/255.0f, Color.g/255.0f, Color.b/255.0f));
                    }
                }
                screen.Apply();
            } else if (GetInputState(PEG_CURSOR_ENABLED) && Data.CursorX < SizeX && Data.CursorY < SizeZ)
            {
                Font.SetChar(screen, cursorState, 32, Data.CursorX, (SizeZ - 1)-Data.CursorY, new Color(Color.r / 255.0f, Color.g / 255.0f, Color.b / 255.0f));
                screen.Apply();
            }
        }
        public override PlacingRules GenerateDynamicPlacingRules()
        {
            return PlacingRules.FlippablePanelOfSize(SizeX, SizeZ);
        }
        protected override IList<IDecoration> GenerateDecorations()
        {
            if (screen == null)
            {
                screenWidth = 48;
                screenHeight = 32;
                screen = new Texture2D(screenWidth, screenHeight);
                screen.filterMode = FilterMode.Point;
            }
            Material material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = screen;
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            gameObject.GetComponent<Renderer>().material = material;
            return new Decoration[1]
            {
            new Decoration
            {
                LocalPosition = new Vector3(-0.5f, 0.0f, -0.5f) * 0.3f,
                LocalRotation = Quaternion.Euler(90f, 0f, 0f),
                DecorationObject = gameObject,
                AutoSetupColliders = true,
                IncludeInModels = true
            }
            };
        }
    }
}
