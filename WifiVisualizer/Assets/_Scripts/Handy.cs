using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handy : MonoBehaviour
{
    public Text text;
    public Button SetButton;
    public Button GetButton;

    private bool toggleWifi = false;

    // Use this for initialization
    void Start()
    {
        SetButton.onClick.AddListener(delegate { OnSetButton(); });
        GetButton.onClick.AddListener(delegate { OnGetButton(); });
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GetNetworkMAC() + ";" + GetNetworkSSID() + ";" + GetNetworkDBM();
    }

    public void OnSetButton()
    {
        try
        {
            text.text = "Wifi successfully set: " + SetWifiEnabled(toggleWifi = !toggleWifi);
        }catch(Exception e)
        {
            text.text = "Error in SetWifiEnabled()\n" + e.Message;
        }
    }

    public void OnGetButton()
    {
        try
        {
            text.text = "Wifi is enabled: " + IsWifiEnabled();
        }
        catch (Exception e)
        {
            text.text = "Error in IsWifiEnabled()\n" + e.Message;
        }
    }

    public bool SetWifiEnabled(bool enabled)
    {
        bool success = false;
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                success = wifiManager.Call<bool>("setWifiEnabled", enabled);
            }
        }

        return success;
    }

    public bool IsWifiEnabled()
    {
        bool success = false;
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                success = wifiManager.Call<bool>("isWifiEnabled");
            }
        }
        return success;
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

    public int GetNetworkDBM()
    {
        using (var wifiInfo = GetWifiInfo())
        {
            return wifiInfo.Call<int>("getRssi");
        }
    }

    public string GetNetworkSSID()
    {
        using (var wifiInfo = GetWifiInfo())
        {
            return wifiInfo.Call<string>("getSSID");
        }
    }

    public string GetNetworkMAC()
    {
        using (var wifiInfo = GetWifiInfo())
        {
            return wifiInfo.Call<string>("getBSSID");
        }
    }
}
