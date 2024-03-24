using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CheeseRamMenu.Client.Files
{
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        public static int StructSize = Marshal.SizeOf<OpenFileName>();
        public uint         lStructSize = (uint)StructSize;
        public IntPtr       hwndOwner;
        public IntPtr       hInstance;
        public string       lpstrFilter;
        public (string title, string filter)[] Filter
        {
            get
            {
                var result = new List<(string title, string filter)>();
                var split = lpstrFilter.Split('\0');
                for (int i = 0; i < split.Length - 1; i += 2)
                {
                    result.Add((split[0],split[1]));
                }
                return result.ToArray();
            }
            set
            {
                var result = value.Aggregate("", (current, filter) => current + $"{filter.title}\0{filter.filter}\0");
                lpstrFilter = result;
            }
        }
        public string lpstrCustomFilter = null;
        public uint         nMaxCustFilter = 0;
        //  We will use this to automatically allocate the buffer
        public uint         nFilterIndex;
        public string       lpstrFile;

        public string File
        {
            get => lpstrFile;
            set
            {
                lpstrFile = value;
                if (value == null) nMaxFile = 0;
                else nMaxFile = (uint)value.Length;
            }
        }
        
        public uint         nMaxFile;
        public uint FileBufferSize
        {
            get => nMaxFile;
            set
            {
                nMaxFile = value;
                lpstrFile = new string(new char[value]);
            }
        }
        public string       lpstrFileTitle;
        public string FileTitle
        {
            get => lpstrFileTitle;
            set
            {
                lpstrFileTitle = value;
                if (value == null) nMaxFileTitle = 0;
                else nMaxFileTitle = (uint)value.Length;
            }
        }
        public uint         nMaxFileTitle;

        public uint FileTitleBufferSize
        {
            get => nMaxFileTitle;
            set {
                nMaxFileTitle = value;
                lpstrFileTitle = new string(new char[value]);
            }
        }
        public string       lpstrInitialDir;
        public string       lpstrTitle;
        public uint         Flags;

        public bool HasFlag(OpenFileNameFlags flag) => (Flags & (uint)flag) != 0;

        public void SetFlags(params OpenFileNameFlags[] flags)
        {
            foreach (var flag in flags) Flags |= (uint)flag;
        }
        public ushort       nFileOffset;
        public string FileName => File.Substring(nFileOffset);

        public ushort       nFileExtension;
        public string FileNameWithoutExt => File.Substring(nFileOffset, nFileExtension - nFileOffset);
        public string FileExt => File.Substring(nFileExtension);
        
        public string       lpstrDefExt;
        
        public IntPtr       lCustData;
        public IntPtr       lpfnHook;
        public IntPtr       lpTemplateName;
        public IntPtr       pvReserved;
        public uint         dwReserved;
        
        public uint         FlagsEx;
        public static OpenFileName GetDefaultOpenFileDialog(string title, string defaultExtension, params (string title, string filter)[] filters)
        {
            if (filters.Length == 0) filters = new (string title, string filter)[] { ("All Files", "*.*") };
            var dialog = new OpenFileName
            {
                Filter = filters,
                lpstrTitle = title,
                lpstrDefExt = defaultExtension,
                FileBufferSize = 1024,
            };
            dialog.SetFlags(OpenFileNameFlags.Explorer, OpenFileNameFlags.FileMustExist);
            return dialog;
        }
        
        public static OpenFileName GetDefaultSaveFileDialog(string title, string defaultExtension, params (string title, string filter)[] filters)
        {
            if (filters.Length == 0) filters = new (string title, string filter)[] { ("All Files", "*.*") };
            var dialog = new OpenFileName
            {
                Filter = filters,
                lpstrTitle = title,
                lpstrDefExt = defaultExtension,
                FileBufferSize = 1024,
            };
            dialog.SetFlags(OpenFileNameFlags.Explorer, OpenFileNameFlags.OverwritePrompt);
            return dialog;
        }
    }
}