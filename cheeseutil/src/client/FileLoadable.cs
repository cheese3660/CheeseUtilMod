using LICC;

namespace CheeseUtilMod.Client
{
    public interface FileLoadable 
    {
        void Load(byte[] filedata,LineWriter writer);
    }
}
