using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWifiInfo
{
    int GetDecibel();
    string GetSSID();
    string GetMAC();
    string GetIP();
}
