using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CheeseRamMenu.Client.Files
{
    
    [StructLayout(LayoutKind.Explicit)]
    public struct OpenFileName
    {
        public const uint StructSize = 144;
        [FieldOffset(0)]
        public uint         lStructSize;
        [FieldOffset(4)]
        public IntPtr       hwndOwner;
        [FieldOffset(12)]
        public IntPtr       hInstance;
        [FieldOffset(20)]
        public IntPtr       lpstrFilter;
        public (string title, string filter)[] Filter
        {
            get => lpstrFilter.GetStringPairs();
            set
            {
                if (lpstrFilter != (IntPtr)0) lpstrFilter.Free();
                lpstrFilter = value.Allocate();
            }
        }
        [FieldOffset(28)]
        public IntPtr       lpstrCustomFilter;

        public (string title, string filter) CustomFilter
        {
            get => lpstrCustomFilter.GetStringPair();
            set
            {
                if (lpstrCustomFilter != (IntPtr)0) lpstrCustomFilter.Free();
                var bufferSize = Encoding.ASCII.GetBytes(value.title).Length + 1 + Encoding.ASCII.GetBytes(value.filter).Length + 1;
                nMaxCustFilter = (uint)bufferSize;
                lpstrCustomFilter = value.Allocate();
            }
        }
        [FieldOffset(36)]
        public uint         nMaxCustFilter;
        //  We will use this to automatically allocate the buffer
        public uint CustomFilterSize
        {
            get => nMaxCustFilter;
            set
            {
                if (lpstrCustomFilter != (IntPtr)0) lpstrCustomFilter.Free();
                nMaxCustFilter = value;
                lpstrFilter = Marshal.AllocHGlobal((int)value);
            }
        }
        [FieldOffset(40)]
        public uint         nFilterIndex;
        public (string title, string filter) SelectedFilter => nFilterIndex == 0 ? CustomFilter : Filter[nFilterIndex - 1];
        [FieldOffset(44)]
        public IntPtr       lpstrFile;

        public string File
        {
            get => lpstrFile.GetString();
            set
            {
                if (lpstrFile != (IntPtr)0) lpstrFile.Free();
                var size = Encoding.ASCII.GetBytes(value).Length + 1;
                nMaxFile = (uint)size;
                lpstrFile = value.Allocate();
            }
        }
        [FieldOffset(52)]
        public uint         nMaxFile;
        public uint FileBufferSize
        {
            get => nMaxFile;
            set
            {
                if (lpstrFile != (IntPtr)0) lpstrFile.Free();
                nMaxFile = value;
                lpstrFile = Marshal.AllocHGlobal((int)value);
            }
        }
        [FieldOffset(56)]
        public IntPtr       lpstrFileTitle;

        public string FileTitle
        {
            get => lpstrFileTitle.GetString();
            set
            {
                if (lpstrFileTitle != (IntPtr)0) lpstrFileTitle.Free();
                nMaxFileTitle = (uint)(Encoding.ASCII.GetBytes(value).Length + 1);
                lpstrFileTitle = value.Allocate();
            }
        }
        [FieldOffset(64)]
        public uint         nMaxFileTitle;

        public uint FileTitleBufferSize
        {
            get => nMaxFileTitle;
            set {
                if (lpstrFileTitle != (IntPtr)0) lpstrFileTitle.Free();
                nMaxFileTitle = value;
                lpstrFileTitle = Marshal.AllocHGlobal((int)value);
            }
        }
        [FieldOffset(68)]
        public IntPtr       lpstrInitialDir;

        public string InitialDirectory
        {
            get => lpstrInitialDir.GetString();
            set
            {
                if (lpstrInitialDir != (IntPtr)0) lpstrInitialDir.Free();
                lpstrInitialDir = value.Allocate();
            }
        }
        [FieldOffset(76)]
        public IntPtr       lpstrTitle;
        public string Title
        {
            get => lpstrTitle.GetString();
            set
            {
                if (lpstrTitle != (IntPtr)0) lpstrTitle.Free();
                lpstrTitle = value.Allocate();
            }
        }
        [FieldOffset(84)]
        public uint         Flags;

        public bool HasFlag(OpenFileNameFlags flag) => (Flags & (uint)flag) != 0;

        public void SetFlags(params OpenFileNameFlags[] flags)
        {
            foreach (var flag in flags) Flags |= (uint)flag;
        }
        [FieldOffset(88)]
        public ushort       nFileOffset;
        public string FileName => File.Substring(nFileOffset);

        [FieldOffset(92)]
        public ushort       nFileExtension;
        public string FileNameWithoutExt => File.Substring(nFileOffset, nFileExtension - nFileOffset);
        public string FileExt => File.Substring(nFileExtension);
        
        [FieldOffset(96)]
        public IntPtr       lpstrDefExt;

        public string DefaultExtension
        {
            get => lpstrDefExt.GetString();
            set
            {
                if (lpstrDefExt != (IntPtr)0) lpstrDefExt.Free();
                lpstrDefExt = value.Allocate();
            }
        }
        
        [FieldOffset(104)]
        public IntPtr       lCustData;
        [FieldOffset(112)]
        public IntPtr       lpfnHook;
        [FieldOffset(120)]
        public IntPtr       lpTemplateName;
        [FieldOffset(128)]
        public IntPtr       pvReserved;
        [FieldOffset(136)]
        public uint         dwReserved;
        [FieldOffset(140)]
        public uint         FlagsEx;

        public void Free()
        {
            if (lpstrFilter != (IntPtr)0) Marshal.FreeHGlobal(lpstrFilter);
            if (lpstrCustomFilter != (IntPtr)0) Marshal.FreeHGlobal(lpstrCustomFilter);
            if (lpstrFile != (IntPtr)0) Marshal.FreeHGlobal(lpstrFile);
            if (lpstrFileTitle != (IntPtr)0) Marshal.FreeHGlobal(lpstrFileTitle);
            if (lpstrInitialDir != (IntPtr)0) Marshal.FreeHGlobal(lpstrInitialDir);
            if (lpstrTitle != (IntPtr)0) Marshal.FreeHGlobal(lpstrTitle);
            if (lpstrDefExt != (IntPtr)0) Marshal.FreeHGlobal(lpstrDefExt);
        }

        public static OpenFileName GetDefaultOpenFileDialog(string title, string defaultExtension, params (string title, string filter)[] filters)
        {
            if (filters.Length == 0) filters = new (string title, string filter)[] { ("All Files", "*.*") };
            var dialog = new OpenFileName
            {
                lStructSize = StructSize,
                Filter = filters,
                Title = title,
                DefaultExtension = defaultExtension,
                FileBufferSize = 1024,
                CustomFilterSize = 256
            };
            dialog.SetFlags(OpenFileNameFlags.Explorer, OpenFileNameFlags.FileMustExist);
            return dialog;
        }
        
        public static OpenFileName GetDefaultSaveFileDialog(string title, string defaultExtension, params (string title, string filter)[] filters)
        {
            if (filters.Length == 0) filters = new (string title, string filter)[] { ("All Files", "*.*") };
            var dialog = new OpenFileName
            {
                lStructSize = StructSize,
                Filter = filters,
                Title = title,
                DefaultExtension = defaultExtension,
                FileBufferSize = 1024,
                CustomFilterSize = 256
            };
            dialog.SetFlags(OpenFileNameFlags.Explorer, OpenFileNameFlags.OverwritePrompt);
            return dialog;
        }
    }
}