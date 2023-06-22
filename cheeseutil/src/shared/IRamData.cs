namespace CheeseUtilMod.Shared.CustomData
{
    public interface IRamData
    {
        byte[] Data { get; set; }
        byte state { get; set; }
        byte[] ClientIncomingData { get; set; }
    }
}
