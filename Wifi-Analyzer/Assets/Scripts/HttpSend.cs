using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpSend : MonoBehaviour
{

    public InputField field;
    public Button autoSendToggle;
    
    private IWifiInfo wifi;

    private short autoSend = 0;
    private readonly Color[] colors = { Color.white, Color.gray };
    private float lastSend = 0;

    private void Start()
    {
        wifi = new WifiInfo();
    }

    private void FixedUpdate()
    {
        if(Time.time - lastSend < 0.2f)
        {
            return;
        }

        lastSend = Time.time;
        if (autoSend == 1)
        {
            SendUnity();
        }
    }
    
    public void SendUnity()
    {
        if (string.IsNullOrEmpty(field.text))
        {
            return;
        }
        StartCoroutine(SendRoutine());
    }

    public void ToggleAutoSend()
    {
        autoSend++;
        autoSend %= 2;

        autoSendToggle.image.color = colors[autoSend];
    }

    IEnumerator SendRoutine()
    {
        string uri = "http://" + field.text + ":8000/capture" + "?";
        uri += "ssid=" + wifi.GetSSID() + "&";
        uri += "mac=" + wifi.GetMAC() + "&";
        uri += "db=" + wifi.GetDecibel() + "&";
        uri += "auto=" + autoSend;

        UnityWebRequest request = UnityWebRequest.Get(uri);
        request.timeout = 1;
        yield return request.SendWebRequest();

    }
}
