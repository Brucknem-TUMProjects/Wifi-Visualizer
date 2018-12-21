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
#if UNITY_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
        server = GetComponent<TCPServer>();
        server.SetupServer(Width);
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

    float Width
    {
        get
        {
#if UNITY_EDITOR
            return 0.059f;
#elif UNITY_ANDROID
            return 0.0681f;
            //using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            //{
            //    using (AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            //    {

            //        using (AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics"))
            //        {
            //            activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);

            //            return metrics.Get<float>("xdpi");
            //        }
            //    }
            //}

#endif
        }
    }
}
