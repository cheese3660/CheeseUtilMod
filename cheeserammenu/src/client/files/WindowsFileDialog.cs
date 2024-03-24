using System.Runtime.InteropServices;

namespace CheeseRamMenu.Client.Files
{
    public static class WindowsFileDialog
    {
        [DllImport("Comdlg32.dll")]
        private static extern bool GetOpenFileNameA(ref OpenFileName arg);
        
        [DllImport("Comdlg32.dll")]
        private static extern bool GetSaveFileNameA(ref OpenFileName arg);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        public static string GetOpenFile(string title, string defaultExtension,
            params (string title, string filter)[] filters)
        {
            var box = OpenFileName.GetDefaultOpenFileDialog(title, defaultExtension, filters);
            var result = GetOpenFileNameA(ref box);
            var le = GetLastError();
            var filename = "";
            if (result) filename = box.File;
            box.Free();
            return filename;
        }
        
        public static string GetSaveFile(string title, string defaultExtension,
            params (string title, string filter)[] filters) {
            
            var box = OpenFileName.GetDefaultSaveFileDialog(title, defaultExtension, filters);
            var result = GetSaveFileNameA(ref box);
            var le = GetLastError();
            var filename = "";
            if (result) filename = box.File;
            box.Free();
            return filename;
        }
    }
}