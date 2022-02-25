using LICC;

namespace CheeseUtilMod.Server
{
    public interface FileLoadable 
    {
        void Load(byte[] filedata,LineWriter writer);
    }
}
