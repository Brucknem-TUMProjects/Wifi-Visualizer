using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiInfo : IWifiInfo
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

#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID

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
#endif
    }

    public override string GetSSID()
    {
#if UNITY_EDITOR
        return "Mock SSID";
#elif UNITY_ANDROID

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
#endif
    }

    public override string GetMAC()
    {
#if UNITY_EDITOR
        return "Mock MAC";
#elif UNITY_ANDROID

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
#endif
    }

    public override string GetIP()
    {
#if UNITY_EDITOR
        return "localhost";
#elif UNITY_ANDROID

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
#endif
    }
}