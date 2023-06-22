using CheeseUtilMod.Shared.CustomData;
using LogicWorld.Server.Circuitry;
using System.Timers;
using System.IO;
using System.IO.Compression;

namespace CheeseUtilMod.Components
{
    //Modified: https://github.com/ShadowAA55/HMM/blob/main/HMM/src/server/Output.cs
    public class TextDisplay : LogicComponent<ITextConsoleData>
    {
        Timer screenupdatetimer;
        bool timertick = false;
        bool ismemdirty = false;
        bool loadfromsave = false;
        bool hasBeenDeleted = false;
        byte[] mem;
        static int PEG_X_START = 0;
        static int PEG_Y_START = 6;
        static int PEG_CHAR_START = 12;
        static int PEG_CLEAR = 20;
        static int PEG_SET = 21;
        static int PEG_SET_CURSOR = 22;
        static int PEG_CURSOR_ENABLED = 23;
        static int PEG_SCROLL_UP = 24;
        static int PEG_SCROLL_DOWN = 25;
        static int PEG_SCROLL_LEFT = 26;
        static int PEG_SCROLL_RIGHT = 27;
        byte cursorX;
        byte cursorY;
        MemoryStream memstream;

        protected override void Initialize()
        {
            memstream = new MemoryStream();
            mem = new byte[64 * 64];
            screenupdatetimer = new Timer(500); //Do 2 text updates per second
            screenupdatetimer.Elapsed += OnTimerElapsed;
            screenupdatetimer.AutoReset = true;
            screenupdatetimer.Start();
            loadfromsave = true;
        }

        public override void Dispose()
        {
            hasBeenDeleted = true;
            screenupdatetimer.Stop();
            screenupdatetimer.Dispose();
            WriteScreenToData();
            base.Dispose();
        }

        public void OnTimerElapsed(object source, ElapsedEventArgs args)
        {
            if (ismemdirty)
            {
                timertick = true;
            }
        }

        public int getDataShifted(int start_peg, int num_bits)
        {
            int value = 0;
            for (int shift = 0; shift < num_bits; shift++)
            {
                if (Inputs[start_peg + shift].On) value |= 1 << shift;
            }
            return value;
        }

        protected override void DoLogicUpdate()
        {
            if (Inputs[PEG_CLEAR].On)
            {
                for (int i = 0; i < 64 * 64; i++)
                {
                    mem[i] = 0;
                    ismemdirty = true;
                }
            }
            if (Inputs[PEG_SET].On)
            {
                int x = getDataShifted(PEG_X_START, 6);
                int y = getDataShifted(PEG_Y_START, 6);
                byte chr = (byte)getDataShifted(PEG_CHAR_START, 8);
                int index = (y * 64) + x;
                ismemdirty = true;
                mem[index] = chr;
            }
            if (Inputs[PEG_SET_CURSOR].On)
            {
                int x = getDataShifted(PEG_X_START, 6);
                int y = getDataShifted(PEG_Y_START, 6);
                cursorX = (byte)x;
                cursorY = (byte)y;
                ismemdirty = true;
            }
            if (Inputs[PEG_SCROLL_UP].On)
            {
                for (int y = 1; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        int idx0  = y * 64 + x;
                        int idx1 = (y-1) * 64 + x;
                        mem[idx1] = mem[idx0];
                        mem[idx0] = 0;
                    }
                }
                ismemdirty = true;
            }
            if (Inputs[PEG_SCROLL_DOWN].On)
            {
                for (int y = 62; y >= 0; y--)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        int idx0 = y * 64 + x;
                        int idx1 = (y + 1) * 64 + x;
                        mem[idx1] = mem[idx0];
                        mem[idx0] = 0;
                    }
                }
                ismemdirty = true;
            }
            if (Inputs[PEG_SCROLL_LEFT].On)
            {
                for (int row = 0; row < 64*64; row+=64)
                {
                    for (int col = 1; col < 64; col++)
                    {
                        mem[row + col - 1] = mem[row+col];
                        mem[row + col] = 0;
                    }
                }
                ismemdirty = true;
            }
            if (Inputs[PEG_SCROLL_RIGHT].On)
            {
                for (int row = 0; row < 64*64; row+=64)
                {
                    for (int col = 62; col >= 0; col--)
                    {
                        mem[row + col + 1] = mem[row + col];
                        mem[row + col] = 0;
                    }
                }
                ismemdirty = true;
            }
            if (ismemdirty)
                QueueLogicUpdate();
            if (timertick)
            {
                WriteScreenToData();
                timertick = false;
                ismemdirty = false;
            }
        }

        public override bool InputAtIndexShouldTriggerComponentLogicUpdates(int inputIndex) => inputIndex >= PEG_CLEAR && inputIndex != PEG_CURSOR_ENABLED;

        protected override void SetDataDefaultValues()
        {
            Data.SizeX = 4;
            Data.SizeZ = 4;
            Data.TextData = null;
            Data.color = JimmysUnityUtilities.Color24.Amber;
            Data.CursorX = 0;
            Data.CursorY = 0;
        }

        protected override void OnCustomDataUpdated()
        {
            if (loadfromsave && Data.TextData != null)
            {
                MemoryStream stream = new MemoryStream(Data.TextData);
                stream.Position = 0;
                DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                decompressor.Read(mem, 0, 64 * 64);
                loadfromsave = false;
            }
        }

        private void WriteScreenToData()
        {
            memstream.Position = 0;
            DeflateStream compressor = new DeflateStream(memstream, CompressionLevel.Fastest, true);
            compressor.Write(mem, 0, 64 * 64);
            compressor.Flush();
            int length = (int)memstream.Position;
            memstream.Position = 0;
            byte[] bytes = new byte[length];
            memstream.Read(bytes, 0, length);
            if(!hasBeenDeleted){
                Data.TextData = bytes;
                Data.CursorX = cursorX;
                Data.CursorY = cursorY;
            }
        }
    }
}
