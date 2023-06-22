namespace CheeseUtilMod.Shared.CustomData
{
    public interface IRamResizableData
    {
        byte[] Data { get; set; }
        byte state { get; set; }
        byte[] ClientIncomingData { get; set; }
        int bitWidth { get; set; }
        int addressWidth { get; set; }
    }
}
