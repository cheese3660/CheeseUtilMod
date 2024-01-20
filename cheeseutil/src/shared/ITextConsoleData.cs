using JimmysUnityUtilities;

namespace CheeseUtilMod.Shared.CustomData
{
    public interface ITextConsoleData
    {
        int SizeX { get; set; }
        int SizeZ { get; set; }
        int CursorX { get; set; } //Used for the displayed cursor, between 0 and 63
        int CursorY { get; set; } //Used for the displayed cursor, between 0 and 63
        byte[] TextData { get; set; }

        Color24 Color { get; set; } //The color of set pixels on the screen, unset pixels are black
    }

    public static class TextConsoleDataInit
    {
        public static void initialize(this ITextConsoleData data)
        {
            data.SizeX = 4;
            data.SizeZ = 4;
            data.TextData = null;
            data.Color = Color24.Amber;
            data.CursorX = 0;
            data.CursorY = 0;
        }
    }
}

//What all inputs do I want for the text display
//6 bit X
//6 bit Y
//8 bit Character
//1 bit clear
//1 bit set
//1 bit set cursor position
//1 bit cursor enabled
//1 bit scroll up
//1 bit scroll down
//1 bit scroll left
//1 bit scroll right
