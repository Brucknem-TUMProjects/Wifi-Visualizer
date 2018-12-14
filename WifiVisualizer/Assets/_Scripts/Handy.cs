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

    private void Start()
    {
        panel.sizeDelta = new Vector2(800,GetComponent<RectTransform>().rect.height - marker.rectTransform.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        ip.text = "" + WifiInfo.Instance.IP;
        mac.text = "" + WifiInfo.Instance.MAC;
        ssid.text = "" + WifiInfo.Instance.SSID;
        db.text = "" + WifiInfo.Instance.DB;
    }    
}
