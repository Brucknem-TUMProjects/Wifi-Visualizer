using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Handy : MonoBehaviour
{
    public Text ip;
    public Text mac;
    public Text ssid;
    public Text db;


    private IWifiInfo wifiInfo;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        wifiInfo = new WifiInfo();
    }

    // Update is called once per frame
    void Update()
    {
        ip.text = "" + wifiInfo.GetIP();
        mac.text = "" + wifiInfo.GetMAC();
        ssid.text = "" + wifiInfo.GetSSID();
        db.text = "" + wifiInfo.GetDecibel();
    }    

    float Width
    {
        get
        {
            return 0.0681f;
        }
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
