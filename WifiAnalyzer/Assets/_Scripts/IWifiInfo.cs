using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWifiInfo
{
    public abstract int GetDecibel();
    public abstract string GetSSID();
    public abstract string GetMAC();
    public abstract string GetIP();

    public int GetPort()
    {
        return 12345;
    }
}
