namespace Halcyon.WLED
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class WLEDSettings
    {
        public string stripUrl = "wled-table.local";
        public int stripPort = 21324;

        public int ledAmount = 60;
        public int offset = 0;

        public bool mirror = false;
        public bool center = false;
    }
}