public class Signal
{
    public string SSID { get; private set; }
    public string MAC { get; private set; }
    public int Decibel { get; private set; }

    public Signal(string SSID, string MAC, int Decibel)
    {
        this.SSID = SSID;
        this.MAC = MAC;
        this.Decibel = Decibel;
    }
}