using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CheeseRamMenu.Client.Files
{
    public static class IntPtrExtensions
    {
        public static (string title, string filter)[] GetStringPairs(this IntPtr ptr, ref int offset)
        {
            if (ptr == (IntPtr)0) return null;
            var strings = new List<(string title, string filter)>();
            while (true)
            {
                strings.Add((ptr.GetString(ref offset),ptr.GetString(ref offset)));
                var check = Marshal.ReadByte(ptr, offset);
                if (check == 0) break;
            }
            
            return strings.ToArray();
        }

        public static (string title, string filter)[] GetStringPairs(this IntPtr ptr)
        {
            int _ = 0;
            return GetStringPairs(ptr, ref _);
        }


        public static string GetString(this IntPtr ptr, ref int offset)
        {
            if (ptr == (IntPtr)0) return null;
            var bytes = new List<byte>();
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, offset);
                offset += 1;
                if (ch == 0) break;
                bytes.Add(ch);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        public static string GetString(this IntPtr ptr)
        {
            int _ = 0;
            return GetString(ptr, ref _);
        }
        
        public static (string title, string filter) GetStringPair(this IntPtr ptr)
        {
            int _ = 0;
            return GetStringPair(ptr, ref _);
        }
        
        public static (string title, string filter) GetStringPair(this IntPtr ptr, ref int offset)
        {
            return (ptr.GetString(), ptr.GetString());
        }



        public static void WriteBytes(this IntPtr ptr, ref int offset, IEnumerable<byte> bytes)
        {
            foreach (var by in bytes)
            {
                Marshal.WriteByte(ptr, offset, by);
                offset += 1;
            }
        }

        public static void WriteBytes(this IntPtr ptr, IEnumerable<byte> bytes)
        {
            int _ = 0;
            WriteBytes(ptr, ref _, bytes);
        }


        public static IntPtr Allocate(this IEnumerable<byte> enumerable)
        {
            var array = enumerable.ToArray();
            var buffer = Marshal.AllocHGlobal(array.Length);
            buffer.WriteBytes(array);
            return buffer;
        }

        public static IntPtr Allocate(this string str)
        {
            if (str == null) return (IntPtr)0;
            List<byte> bytes = Encoding.ASCII.GetBytes(str).ToList();
            bytes.Add(0); // Add a null terminator
            return Allocate(bytes);
        }

        public static IntPtr Allocate(this (string title, string filter) pair)
        {
            List<byte> bytes = Encoding.ASCII.GetBytes(pair.title).ToList();
            bytes.Add(0);
            bytes.AddRange(Encoding.ASCII.GetBytes(pair.filter));
            bytes.Add(0);
            return Allocate(bytes);
        }

        public static IntPtr Allocate(this (string title, string filter)[] pairs)
        {
            if (pairs == null) return (IntPtr)0;
            var bytes = new List<byte>();
            foreach (var (title, filter) in pairs)
            {
                bytes.AddRange(Encoding.ASCII.GetBytes(title));
                bytes.Add(0);
                bytes.AddRange(Encoding.ASCII.GetBytes(filter));
                bytes.Add(0);
            }
            bytes.Add(0);
            return Allocate(bytes);
        }

        public static void Free(this IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}