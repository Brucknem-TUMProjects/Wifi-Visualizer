using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Vuforia;

public class DBPolling : MonoBehaviour
{
    bool requested = false;
    bool abort = false;
    TrackableBehaviour trackable;
    Thread requestThread;

    private void Start()
    {
        Debug.Log("Started DB Polling");
        trackable = transform.GetComponent<TrackableBehaviour>();
      //  PiConnector.Instance.ConnectServer(true, "192.168.2.45", 5005);
        DatabaseConnector.Instance.ConnectDatabase("/Database/wifiAnalyzer.db");
        DatabaseConnector.Instance.SelectFrom("*", "location", "", new Location());

        requestThread = new Thread(RequestThread)
        {
            IsBackground = true
        };
        requestThread.Start();
    }

    void RequestThread()
    {
        while(true)
        {
            if (abort)
            {
                return;
            }

            if (IsTracked && !requested)
            {
                DatabaseConnector.Instance.InsertInto("location",  0, 0,0,0,0,0,0);
                // string response = PiConnector.Instance.RequestServer();
            }
        }
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        abort = true;
        PiConnector.Instance.CloseConnection();
        DatabaseConnector.Instance.CloseConnection();
    }

    private bool IsTracked
    {
        get
        {
            var status = trackable.CurrentStatus;
            return status == TrackableBehaviour.Status.TRACKED;
        }
    }
}
