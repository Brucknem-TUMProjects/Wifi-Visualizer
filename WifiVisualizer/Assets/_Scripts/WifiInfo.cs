using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiInfo : MonoBehaviour {

    private static WifiInfo instance = null;
    private static readonly object padlock = new object();

    private WifiInfo()
    {
    }

    public static WifiInfo Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new WifiInfo();
                }
                return instance;
            }
        }
    }

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

    public int DB
    {
        get
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
    }

    public string SSID
    {
        get
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
    }

    public string MAC
    {
        get
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
    }

    public string IP
    {
        get
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
}
