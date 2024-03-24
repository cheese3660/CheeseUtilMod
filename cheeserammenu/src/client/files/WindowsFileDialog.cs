using System;
using System.Runtime.InteropServices;
using LICC;

namespace CheeseRamMenu.Client.Files
{
    public static class WindowsFileDialog
    {
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName arg);
        
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out]  OpenFileName arg);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        public static string GetOpenFile(string title, string defaultExtension,
            params (string title, string filter)[] filters)
        {
            var box = OpenFileName.GetDefaultOpenFileDialog(title, defaultExtension, filters);
            var result = GetOpenFileName(box);
            var le = GetLastError();
            LConsole.WriteLine($"Last error: {le}");
            var filename = "";
            if (result) filename = box.File;
            return filename;
        }
        
        public static string GetSaveFile(string title, string defaultExtension,
            params (string title, string filter)[] filters) {
            
            var box = OpenFileName.GetDefaultSaveFileDialog(title, defaultExtension, filters);
            var result = GetSaveFileName(box);
            var le = GetLastError();
            LConsole.WriteLine($"Last error: {le}");
            var filename = "";
            if (result) filename = box.File;
            return filename;
        }
    }
}