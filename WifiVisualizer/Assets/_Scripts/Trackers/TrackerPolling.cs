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
using UnityEditor;
using UnityEngine.UI;

public class TrackerPolling : MonoBehaviour
{
    private readonly ITrackerConnector trackerConnection = new TrackerConnector();
    TrackableBehaviour trackable;

    long lastStarted = 0;
    readonly long deltaMillis = 100;
    //int id;

    IDBConnector database;

    private void Start()
    {
        trackable = transform.GetComponent<TrackableBehaviour>();
    }


    public void Setup(string host, int port, int id, IDBConnector database, Action<int, bool> callback)
    {
        Debug.Log("Started Tracker: " + host + ":" + port);
        gameObject.SetActive(true);
        this.database = database;
        //this.id = id;
        trackerConnection.ConnectServer(host, port, id, callback);
    }

    private void FixedUpdate()
    {
        //if (actions.Count > 0)
        //{
        //    actions.Dequeue()();
        //}

        if (IsTracked && (Environment.TickCount - lastStarted) > deltaMillis)
        {
            long timestamp = Environment.TickCount;
            lastStarted = timestamp;
            try
            {
                Location location = new Location(timestamp,
                                                            transform.position.x,
                                                            transform.position.y,
                                                            transform.position.z,
                                                            transform.rotation.eulerAngles.x,
                                                            transform.rotation.eulerAngles.y,
                                                            transform.rotation.eulerAngles.z);
                Request(location, timestamp);
            }
            catch { };
        }
        else if (!trackerConnection.IsConnected())
        {
            Remove();
        }
    }

    private void Request(Location location, long timestamp)
    {
        new Thread(() =>
        {
            Signal signal = trackerConnection.RequestServer(timestamp);
            if (signal != null)
            {
                database.Add(signal);
                database.Add(location);
            }
        }).Start();
    }


    private void OnApplicationQuit()
    {
        Remove();
    }

    public void Remove()
    {
        trackerConnection.CloseConnection();
        gameObject.SetActive(false);
    }

    private bool IsTracked
    {
        get
        {
            var status = trackable.CurrentStatus;
            return status == TrackableBehaviour.Status.TRACKED;
        }
    }

    public bool IsConnected
    {
        get
        {
            return trackerConnection.IsConnected();
        }
    }
}