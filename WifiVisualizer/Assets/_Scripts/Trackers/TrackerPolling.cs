using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Vuforia;

public class TrackerPolling : MonoBehaviour
{
    private readonly ITrackerConnector trackerConnection = new TrackerConnector();
    TrackableBehaviour trackable;
    Material rend;

    long lastStarted = 0;
    readonly long deltaMillis = 100;

    private static List<Vector3> tracked = new List<Vector3>();

    private void Start()
    {
        trackable = transform.GetComponent<TrackableBehaviour>();
        rend = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }


    public void Setup(string host, int port, int id, Action<int, float, bool> callback, Action<int> onClosed)
    {
        Debug.Log("Started Tracker: " + host + ":" + port);
        gameObject.SetActive(true);
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
                Vector3 position = transform.position;
                foreach(Vector3 other in tracked)
                {
                    if(Vector3.Distance(other, position) < 0.25f)
                    {
                        return;
                    }
                }
                tracked.Add(position);
                StartCoroutine(Flash());
                Request(position, timestamp);
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

    private void Request(Vector3 location, long timestamp)
    {
#if UNITY_EDITOR
        new Thread(() =>
        {
            Signal signal = trackerConnection.RequestServer();
            if (signal != null)
            {
                DelaunayTriangulator.Instance.Add(new Measurement3D(location, signal));
            }
        }).Start();
#endif
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