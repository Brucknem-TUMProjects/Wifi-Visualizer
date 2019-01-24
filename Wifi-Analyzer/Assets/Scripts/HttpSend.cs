using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpSend : MonoBehaviour {

    public InputField field;
    private IWifiInfo wifi;

    private void Start()
    {
        wifi = new WifiInfo();
    }

    public void SendUnity()
    {
        if (string.IsNullOrEmpty(field.text))
        {
            return;
        }
        //StartCoroutine(SendRoutine());
        Send();
    }

    IEnumerator SendRoutine()
    {
        string uri = "http://" + field.text + ":8000/capture" + "?";
        uri += "ssid=" + wifi.GetSSID() + "&";
        uri += "mac=" + wifi.GetMAC() + "&";
        uri += "db=" + wifi.GetDecibel();

        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();
    }

    void Send()
    {
        string uri = "http://" + field.text + ":8000/capture" + "?";
        uri += "ssid=" + wifi.GetSSID() + "&";
        uri += "mac=" + wifi.GetMAC() + "&";
        uri += "db=" + wifi.GetDecibel();

        UnityWebRequest.Get(uri).SendWebRequest();
    }
}
