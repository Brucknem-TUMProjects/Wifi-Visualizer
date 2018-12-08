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
    TrackableBehaviour trackable;

    private void Start()
    {
        trackable = transform.GetComponent<TrackableBehaviour>();
        PiConnector.Instance.ConnectServer(true, "192.168.2.45", 5005);

        new Thread(RequestThread)
        {
            IsBackground = true
        }.Start();
    }

    void RequestThread()
    {
        while(true)
        {
            if (IsTracked && !requested)
            {
                string response = PiConnector.Instance.RequestServer();
            }
        }
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PiConnector.Instance.CloseConnection();
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
