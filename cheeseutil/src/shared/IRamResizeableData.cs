namespace CheeseUtilMod.Shared.CustomData
{
    public interface IRamResizableData
    {
        byte[] Data { get; set; }
        byte State { get; set; }
        byte[] ClientIncomingData { get; set; }
        int BitWidth { get; set; }
        int AddressWidth { get; set; }
    }

    public static class RamResizableDataInit
    {
        public static void initialize(this IRamResizableData data)
        {
            data.AddressWidth = 1;
            data.BitWidth = 1;
            data.State = 0;
            data.ClientIncomingData = new byte[0];
            data.Data = new byte[0];
        }
    }
}
