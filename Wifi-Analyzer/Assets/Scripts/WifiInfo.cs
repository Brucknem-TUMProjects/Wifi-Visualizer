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

    public int GetDecibel()
    {

#if !UNITY_ANDROID
        return 0;
#else

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

    public string GetSSID()
    {
#if !UNITY_ANDROID
        return "Mock SSID";
#else

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

    public string GetMAC()
    {
#if !UNITY_ANDROID
        return "Mock MAC";
#else

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

    public string GetIP()
    {
#if !UNITY_ANDROID
        return "localhost";
#else

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