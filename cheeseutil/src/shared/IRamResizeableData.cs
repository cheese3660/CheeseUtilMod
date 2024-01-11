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
}
