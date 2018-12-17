using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handy : MonoBehaviour
{
    public Text ip;
    public Text mac;
    public Text ssid;
    public Text db;

    public Image marker;
    public RectTransform panel;
    private Image background;

    private void Start()
    {
        panel.sizeDelta = new Vector2(800, GetComponent<RectTransform>().rect.height - marker.rectTransform.rect.height);
        background = panel.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Colorize();
        ip.text = "" + IWifiInfo<WifiInfoMock>.Instance.GetIP() + ":" + IWifiInfo<WifiInfoMock>.Instance.GetPort();
        mac.text = "" + IWifiInfo<WifiInfoMock>.Instance.GetMAC();
        ssid.text = "" + IWifiInfo<WifiInfoMock>.Instance.GetSSID();
        db.text = "" + IWifiInfo<WifiInfoMock>.Instance.GetDecibel();
    }    

    void Colorize()
    {
        if (TCPServer.Instance.IsConnected)
        {
            background.color = Color.green;
        }
        else
        {
            background.color = Color.red;
        }
    }

    private void OnApplicationQuit()
    {
        TCPServer.Instance.CloseAllSockets();
    }
}
