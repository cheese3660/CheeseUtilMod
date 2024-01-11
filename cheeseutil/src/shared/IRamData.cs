namespace CheeseUtilMod.Shared.CustomData
{
    public interface IRamData
    {
        byte[] Data { get; set; }
        byte State { get; set; }
        byte[] ClientIncomingData { get; set; }
    }
}
