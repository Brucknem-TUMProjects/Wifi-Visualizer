using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiInfo : IWifiInfo<WifiInfo>
{

    private AndroidJavaObject GetWifiInfo()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                return wifiManager.Call<AndroidJavaObject>("getConnectionInfo");
            }
        }
    }

    public override int GetDecibel()
    {
        try
        {
            using (var wifiInfo = GetWifiInfo())
            {
                return wifiInfo.Call<int>("getRssi");
            }
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public override string GetSSID()
    {
        try
        {
            using (var wifiInfo = GetWifiInfo())
            {
                return wifiInfo.Call<string>("getSSID");
            }
        }
        catch (System.Exception)
        {
            return "Error SSID";
        }
    }

    public override string GetMAC()
    {
        try
        {
            using (var wifiInfo = GetWifiInfo())
            {
                return wifiInfo.Call<string>("getBSSID");
            }
        }
        catch (Exception)
        {
            return "Error MAC";
        }
    }

    public override string GetIP()
    {
        try
        {
            using (var wifiInfo = GetWifiInfo())
            {
                using (AndroidJavaClass formatter = new AndroidJavaClass("android.text.format.Formatter"))
                {
                    return formatter.CallStatic<string>("formatIpAddress", wifiInfo.Call<int>("getIpAddress"));
                }
            }
        }
        catch (Exception)
        {
            return "Error IP";
        }
    }
}