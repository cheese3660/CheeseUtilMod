namespace CheeseUtilMod.Shared.CustomData
{
    public interface IRamData
    {
        byte[] Data { get; set; }
        byte State { get; set; }
        byte[] ClientIncomingData { get; set; }
    }

    public static class RamDataInit
    {
        public static void initialize(this IRamData data)
        {
            data.Data = new byte[0];
            data.State = 0;
            data.ClientIncomingData = new byte[0];
        }
    }
}
