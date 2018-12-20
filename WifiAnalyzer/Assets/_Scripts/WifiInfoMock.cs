using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiInfoMock : IWifiInfo {
    public override int GetDecibel()
    {
        return 5;
    }

    public override string GetIP()
    {
        return "Mock IP";
    }

    public override string GetMAC()
    {
        return "Mock MAC";
    }
    
    public override string GetSSID()
    {
        return "Mock SSID";
    }
}
