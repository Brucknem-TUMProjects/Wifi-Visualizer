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
    private TrackerViewButton view;

    TrackableBehaviour trackable;
    Queue<Action> actions = new Queue<Action>();

    long lastStarted = 0;
    long deltaMillis = 100;

    IDBConnector database;

    private void Start()
    {
        trackable = transform.GetComponent<TrackableBehaviour>();
    }


    public void Setup(string host, int port, IDBConnector database, TrackerViewButton view)
    {
        Debug.Log("Started Tracker: " + host + ":" + port);
        gameObject.SetActive(true);
        this.view = view;
        this.database = database;
        trackerConnection.ConnectServer(host, port, ConnectionSuccessful);
    }

    public void ConnectionSuccessful(bool connected)
    {
        if(!connected)
        {
            actions.Enqueue(Remove);
        }
        else
        {
            actions.Enqueue(view.SetConnected);
        }
    }
    
    private void FixedUpdate()
    {
        if(actions.Count > 0)
        {
            actions.Dequeue()();
        }

        if (IsTracked && (Environment.TickCount - lastStarted) > deltaMillis)
        {
            long timestamp = Environment.TickCount;
            lastStarted = timestamp;
            try {
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

    private void Request(Location location, long timestamp) {
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
        Destroy(view.gameObject);
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