using System;
using LICC;

namespace CheeseUtilMod.Client
{
    public interface FileSavable
    {
        void Save(bool force, Action<byte[]> onSave);
    }
}