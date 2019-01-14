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
    Material rend;

    long lastStarted = 0;
    readonly long deltaMillis = 100;
    //int id;

    IDBConnector database;
    DelaunayTriangulator triangulator;
    private static List<Location> tracked = new List<Location>();

    private void Start()
    {
        trackable = transform.GetComponent<TrackableBehaviour>();
        rend = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }


    public void Setup(string host, int port, int id, IDBConnector database, DelaunayTriangulator triangulator, Action<int, float, bool> callback, Action<int> onClosed)
    {
        Debug.Log("Started Tracker: " + host + ":" + port);
        gameObject.SetActive(true);
        this.database = database;
        this.triangulator = triangulator;
        trackerConnection.ConnectServer(host, port, id, callback, onClosed);
    }

    private void FixedUpdate()
    {
        if (IsTracked && (Environment.TickCount - lastStarted) > deltaMillis)
        {
            long timestamp = Environment.TickCount;
            lastStarted = timestamp;
            try
            {
                Location location = new Location(timestamp,
                                                            transform.position.x,
                                                            transform.position.y,
                                                            transform.position.z
                                                           );
                foreach(Location other in tracked)
                {
                    if(Vector3.Distance(other, location) < 0.25f)
                    {
                        return;
                    }
                }
                tracked.Add(location);
                StartCoroutine(Flash());
                Request(location, timestamp);
            }
            catch { };
        }
        else if (!trackerConnection.IsConnected())
        {
            Remove();
        }
    }
    private IEnumerator Flash()
    {
        rend.color = Color.red;
        yield return new WaitForSeconds(1);
        rend.color = Color.green;
    }

    private void Request(Location location, long timestamp)
    {
        new Thread(() =>
        {
            Signal signal = trackerConnection.RequestServer(timestamp);
            if (signal != null)
            {
                database.Add(new Measurement3D(location, signal));
            }
        }).Start();
        triangulator.Recalculate();
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