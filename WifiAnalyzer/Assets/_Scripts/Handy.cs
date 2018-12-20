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
    public Button exit;

    public Image marker;
    public RectTransform panel;
    private Image background;

    private TCPServer server;
    private IWifiInfo wifiInfo = new WifiInfo();

    private void Start()
    {
        server = GetComponent<TCPServer>();
        server.SetupServer();
    //    panel.sizeDelta = new Vector2(800, GetComponent<RectTransform>().rect.height - marker.rectTransform.rect.height);
        background = panel.GetComponent<Image>();
        exit.onClick.AddListener(delegate { OnApplicationQuit(); Application.Quit(); });
    }

    // Update is called once per frame
    void Update()
    {
        Colorize();
        ip.text = "" + wifiInfo.GetIP() + ":" + wifiInfo.GetPort();
        mac.text = "" + wifiInfo.GetMAC();
        ssid.text = "" + wifiInfo.GetSSID();
        db.text = "" + wifiInfo.GetDecibel();
    }    

    void Colorize()
    {
        if (server.IsConnected)
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
        server.CloseAllSockets();
    }
}
