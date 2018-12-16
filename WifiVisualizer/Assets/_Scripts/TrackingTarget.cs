using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class TrackingTarget : NetworkBehaviour
{
    [Range(0.001f, 3)]
    public float updateDelta = 0.1f;

    private TrackableBehaviour trackable;
    private float lastUpdate = 0;

    private void Start()
    {
        trackable = GetComponent<TrackableBehaviour>();
        if (isServer)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.time - lastUpdate > updateDelta && IsTracked)
        {
            CmdRetrieve(transform.position.x, 
                transform.position.y, 
                transform.position.z, 
                transform.rotation.x, 
                transform.rotation.y, 
                transform.rotation.z, 
                WifiInfo.Instance.MAC, 
                WifiInfo.Instance.SSID, 
                WifiInfo.Instance.DB);
            lastUpdate = Environment.TickCount;
        }
    }

    private bool IsTracked
    {
        get
        {
            return trackable.CurrentStatus == TrackableBehaviour.Status.TRACKED;
        }
    }

    [Command]
    public void CmdRetrieve(float posX, float posY, float posZ, float rotX, float rotY, float rotZ, string mac, string ssid, int decibel)
    {
        Debug.Log("received: " + mac + "; " + ssid + "; " + decibel);
        IDBConnector<DBConnector>.Instance.AddAll(new Location(Environment.TickCount, posX, posY, posZ, rotX, rotY, rotZ), new Signal(Environment.TickCount, mac, ssid, decibel));
    }
}