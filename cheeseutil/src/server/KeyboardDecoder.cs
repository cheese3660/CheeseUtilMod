using CheeseUtilMod.Shared.CustomData;
using LogicWorld.Server.Circuitry;
using LogicAPI.Server.Components;
using System;
using System.Timers;
using System.IO;
using System.IO.Compression;

namespace CheeseUtilMod.Components
{
    public class KeyboardDecoder : LogicComponent
    {
        // All of the input pegs from keys
        static int KEY_GRAVE = 0;
        static int KEY_1 = 1;
        static int KEY_2 = 2;
        static int KEY_3 = 3;
        static int KEY_4 = 4;
        static int KEY_5 = 5;
        static int KEY_6 = 6;
        static int KEY_7 = 7;
        static int KEY_8 = 8;
        static int KEY_9 = 9;
        static int KEY_0 = 10;
        static int KEY_MINUS = 11;
        static int KEY_EQUALS = 12;
        static int KEY_BACKSPACE = 13;
        static int KEY_TAB = 14;
        static int KEY_Q = 15;
        static int KEY_W = 16;
        static int KEY_E = 17;
        static int KEY_R = 18;
        static int KEY_T = 19;
        static int KEY_Y = 20;
        static int KEY_U = 21;
        static int KEY_I = 22;
        static int KEY_O = 23;
        static int KEY_P = 24;
        static int KEY_LBRACKET = 25;
        static int KEY_RBRACKET = 26;
        static int KEY_BACKSLASH = 27;
        static int KEY_A = 28;
        static int KEY_S = 29;
        static int KEY_D = 30;
        static int KEY_F = 31;
        static int KEY_G = 32;
        static int KEY_H = 33;
        static int KEY_J = 34;
        static int KEY_K = 35;
        static int KEY_L = 36;
        static int KEY_SEMICOLON = 37;
        static int KEY_QUOTE = 38;
        static int KEY_RETURN = 39;
        static int KEY_Z = 40;
        static int KEY_X = 41;
        static int KEY_C = 42;
        static int KEY_V = 43;
        static int KEY_B = 44;
        static int KEY_N = 45;
        static int KEY_M = 46;
        static int KEY_COMMA = 47;
        static int KEY_PERIOD = 48;
        static int KEY_SLASH = 49;
        static int KEY_SHIFT = 50;
        static int KEY_SPACE = 51;
        static int KEY_CONTROL = 52;
        static int NEXT_KEY = 53;
        static int CLEAR_BUFFER = 54;

        //ASCII Buffers
        static byte[] ASCII_NORMAL;
        static byte[] ASCII_SHIFT;
        static byte[] ASCII_CONTROL;
        static KeyboardDecoder()
        {
            ASCII_NORMAL = new byte[55];
            ASCII_NORMAL[KEY_GRAVE] = (byte)'`';
            ASCII_NORMAL[KEY_1] = (byte)'1';
            ASCII_NORMAL[KEY_2] = (byte)'2';
            ASCII_NORMAL[KEY_3] = (byte)'3';
            ASCII_NORMAL[KEY_4] = (byte)'4';
            ASCII_NORMAL[KEY_5] = (byte)'5';
            ASCII_NORMAL[KEY_6] = (byte)'6';
            ASCII_NORMAL[KEY_7] = (byte)'7';
            ASCII_NORMAL[KEY_8] = (byte)'8';
            ASCII_NORMAL[KEY_9] = (byte)'9';
            ASCII_NORMAL[KEY_0] = (byte)'0';
            ASCII_NORMAL[KEY_MINUS] = (byte)'-';
            ASCII_NORMAL[KEY_EQUALS] = (byte)'=';
            ASCII_NORMAL[KEY_BACKSPACE] = 8;
            ASCII_NORMAL[KEY_TAB] = 9;
            ASCII_NORMAL[KEY_Q] = (byte)'q';
            ASCII_NORMAL[KEY_W] = (byte)'w';
            ASCII_NORMAL[KEY_E] = (byte)'e';
            ASCII_NORMAL[KEY_R] = (byte)'r';
            ASCII_NORMAL[KEY_T] = (byte)'t';
            ASCII_NORMAL[KEY_Y] = (byte)'y';
            ASCII_NORMAL[KEY_U] = (byte)'u';
            ASCII_NORMAL[KEY_I] = (byte)'i';
            ASCII_NORMAL[KEY_O] = (byte)'o';
            ASCII_NORMAL[KEY_P] = (byte)'p';
            ASCII_NORMAL[KEY_LBRACKET] = (byte)'[';
            ASCII_NORMAL[KEY_RBRACKET] = (byte)']';
            ASCII_NORMAL[KEY_BACKSLASH] = (byte)'\\';
            ASCII_NORMAL[KEY_A] = (byte)'a';
            ASCII_NORMAL[KEY_S] = (byte)'s';
            ASCII_NORMAL[KEY_D] = (byte)'d';
            ASCII_NORMAL[KEY_F] = (byte)'f';
            ASCII_NORMAL[KEY_G] = (byte)'g';
            ASCII_NORMAL[KEY_H] = (byte)'h';
            ASCII_NORMAL[KEY_J] = (byte)'j';
            ASCII_NORMAL[KEY_K] = (byte)'k';
            ASCII_NORMAL[KEY_L] = (byte)'l';
            ASCII_NORMAL[KEY_SEMICOLON] = (byte)';';
            ASCII_NORMAL[KEY_QUOTE] = (byte)'\'';
            ASCII_NORMAL[KEY_RETURN] = 10;
            ASCII_NORMAL[KEY_Z] = (byte)'z';
            ASCII_NORMAL[KEY_X] = (byte)'x';
            ASCII_NORMAL[KEY_C] = (byte)'c';
            ASCII_NORMAL[KEY_V] = (byte)'v';
            ASCII_NORMAL[KEY_B] = (byte)'b';
            ASCII_NORMAL[KEY_N] = (byte)'n';
            ASCII_NORMAL[KEY_M] = (byte)'m';
            ASCII_NORMAL[KEY_COMMA] = (byte)',';
            ASCII_NORMAL[KEY_PERIOD] = (byte)'.';
            ASCII_NORMAL[KEY_SLASH] = (byte)'/';
            ASCII_NORMAL[KEY_SPACE] = (byte)' ';
            ASCII_SHIFT = new byte[55];
            ASCII_SHIFT[KEY_GRAVE] = (byte)'~';
            ASCII_SHIFT[KEY_1] = (byte)'!';
            ASCII_SHIFT[KEY_2] = (byte)'@';
            ASCII_SHIFT[KEY_3] = (byte)'#';
            ASCII_SHIFT[KEY_4] = (byte)'$';
            ASCII_SHIFT[KEY_5] = (byte)'%';
            ASCII_SHIFT[KEY_6] = (byte)'^';
            ASCII_SHIFT[KEY_7] = (byte)'&';
            ASCII_SHIFT[KEY_8] = (byte)'*';
            ASCII_SHIFT[KEY_9] = (byte)'(';
            ASCII_SHIFT[KEY_0] = (byte)')';
            ASCII_SHIFT[KEY_MINUS] = (byte)'_';
            ASCII_SHIFT[KEY_EQUALS] = (byte)'+';
            ASCII_SHIFT[KEY_Q] = (byte)'Q';
            ASCII_SHIFT[KEY_W] = (byte)'W';
            ASCII_SHIFT[KEY_E] = (byte)'E';
            ASCII_SHIFT[KEY_R] = (byte)'R';
            ASCII_SHIFT[KEY_T] = (byte)'T';
            ASCII_SHIFT[KEY_Y] = (byte)'Y';
            ASCII_SHIFT[KEY_U] = (byte)'U';
            ASCII_SHIFT[KEY_I] = (byte)'I';
            ASCII_SHIFT[KEY_O] = (byte)'O';
            ASCII_SHIFT[KEY_P] = (byte)'P';
            ASCII_SHIFT[KEY_LBRACKET] = (byte)'{';
            ASCII_SHIFT[KEY_RBRACKET] = (byte)'}';
            ASCII_SHIFT[KEY_BACKSLASH] = (byte)'|';
            ASCII_SHIFT[KEY_A] = (byte)'A';
            ASCII_SHIFT[KEY_S] = (byte)'S';
            ASCII_SHIFT[KEY_D] = (byte)'D';
            ASCII_SHIFT[KEY_F] = (byte)'F';
            ASCII_SHIFT[KEY_G] = (byte)'G';
            ASCII_SHIFT[KEY_H] = (byte)'H';
            ASCII_SHIFT[KEY_J] = (byte)'J';
            ASCII_SHIFT[KEY_K] = (byte)'K';
            ASCII_SHIFT[KEY_L] = (byte)'L';
            ASCII_SHIFT[KEY_SEMICOLON] = (byte)':';
            ASCII_SHIFT[KEY_QUOTE] = (byte)'"';
            ASCII_SHIFT[KEY_Z] = (byte)'Z';
            ASCII_SHIFT[KEY_X] = (byte)'X';
            ASCII_SHIFT[KEY_C] = (byte)'C';
            ASCII_SHIFT[KEY_V] = (byte)'V';
            ASCII_SHIFT[KEY_B] = (byte)'B';
            ASCII_SHIFT[KEY_N] = (byte)'N';
            ASCII_SHIFT[KEY_M] = (byte)'M';
            ASCII_SHIFT[KEY_COMMA] = (byte)'<';
            ASCII_SHIFT[KEY_PERIOD] = (byte)'>';
            ASCII_SHIFT[KEY_SLASH] = (byte)'?';
            ASCII_CONTROL = new byte[55];
            ASCII_CONTROL[KEY_A] = 1;
            ASCII_CONTROL[KEY_B] = 2;
            ASCII_CONTROL[KEY_C] = 3;
            ASCII_CONTROL[KEY_D] = 4;
            ASCII_CONTROL[KEY_E] = 5;
            ASCII_CONTROL[KEY_F] = 6;
            ASCII_CONTROL[KEY_G] = 7;
            ASCII_CONTROL[KEY_H] = 8;
            ASCII_CONTROL[KEY_I] = 9;
            ASCII_CONTROL[KEY_J] = 10;
            ASCII_CONTROL[KEY_K] = 11;
            ASCII_CONTROL[KEY_L] = 12;
            ASCII_CONTROL[KEY_M] = 13;
            ASCII_CONTROL[KEY_N] = 14;
            ASCII_CONTROL[KEY_O] = 15;
            ASCII_CONTROL[KEY_P] = 16;
            ASCII_CONTROL[KEY_Q] = 17;
            ASCII_CONTROL[KEY_R] = 18;
            ASCII_CONTROL[KEY_S] = 19;
            ASCII_CONTROL[KEY_T] = 20;
            ASCII_CONTROL[KEY_U] = 21;
            ASCII_CONTROL[KEY_V] = 22;
            ASCII_CONTROL[KEY_W] = 23;
            ASCII_CONTROL[KEY_X] = 24;
            ASCII_CONTROL[KEY_Y] = 25;
            ASCII_CONTROL[KEY_Z] = 26;
            ASCII_CONTROL[KEY_LBRACKET] = 27;
            ASCII_CONTROL[KEY_BACKSLASH] = 28;
            ASCII_CONTROL[KEY_RBRACKET] = 29;
            ASCII_CONTROL[KEY_6] = 30;
            ASCII_CONTROL[KEY_MINUS] = 31;
        }

        //Output pegs
        static int ASCII_0 = 0;
        static int ASCII_1 = 1;
        static int ASCII_2 = 2;
        static int ASCII_3 = 3;
        static int ASCII_4 = 4;
        static int ASCII_5 = 5;
        static int ASCII_6 = 6;

        static int HAS_CHAR = 7;

        byte buffer_read_pos = 0;
        byte buffer_write_pos = 0;
        byte[] kb_buffer; //0 == no character
        bool[] prev_on;
        bool[] curr_on;
        protected override void Initialize()
        {
            kb_buffer = new byte[256];
            prev_on = new bool[55];
            curr_on = new bool[55];
        }
        private void UpdateBufferPins()
        {
            byte current_key = kb_buffer[buffer_read_pos];
            Outputs[HAS_CHAR].On = current_key != 0;
            Outputs[ASCII_0].On = (current_key & 0b0000001) != 0;
            Outputs[ASCII_1].On = (current_key & 0b0000010) != 0;
            Outputs[ASCII_2].On = (current_key & 0b0000100) != 0;
            Outputs[ASCII_3].On = (current_key & 0b0001000) != 0;
            Outputs[ASCII_4].On = (current_key & 0b0010000) != 0;
            Outputs[ASCII_5].On = (current_key & 0b0100000) != 0;
            Outputs[ASCII_6].On = (current_key & 0b1000000) != 0;

        }
        protected override void DoLogicUpdate()
        {
            for (int i = 0; i < 55; i++)
            {
                curr_on[i] = Inputs[i].On;
            }
            if (curr_on[NEXT_KEY] && !prev_on[NEXT_KEY])
            {
                if (buffer_read_pos < buffer_write_pos) buffer_read_pos += 1;
                UpdateBufferPins();
            } else if (curr_on[CLEAR_BUFFER] && !prev_on[CLEAR_BUFFER])
            {
                kb_buffer = new byte[256];
                buffer_write_pos = 0;
                buffer_read_pos = 0;
                UpdateBufferPins();
            } else if (Inputs[55].On)
            {
                bool has_new_key = false;
                int new_key = 0;
                for (int i = 0; i < KEY_CONTROL; i++)
                {
                    if (i == KEY_SHIFT) continue;
                    if (curr_on[i] && !prev_on[i])
                    {
                        has_new_key = true;
                        new_key = i;
                        break;
                    }
                }
                if (has_new_key)
                {
                    if (curr_on[KEY_CONTROL])
                    {
                        byte key = ASCII_CONTROL[new_key];
                        if (key != 0)
                        {
                            kb_buffer[buffer_write_pos] = key;
                            buffer_write_pos += 1;
                        }
                    }
                    else if (curr_on[KEY_SHIFT])
                    {
                        byte key = ASCII_SHIFT[new_key];
                        if (key != 0)
                        {
                            kb_buffer[buffer_write_pos] = key;
                            buffer_write_pos += 1;
                        }
                    }
                    else
                    {
                        byte key = ASCII_NORMAL[new_key];
                        if (key != 0)
                        {
                            kb_buffer[buffer_write_pos] = key;
                            buffer_write_pos += 1;
                        }
                    }
                    UpdateBufferPins();
                }
            }
            prev_on = curr_on;
            curr_on = new bool[55];
        }
    }
}
