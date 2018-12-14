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

public class DBPolling : MonoBehaviour
{
    private readonly IPiConnector pi = new PiConnector();
    private readonly IDBConnector database = new DBConnector();

    public Text locationsView;
    public Text signalsView;

    bool abort = false;

    TrackableBehaviour trackable;
    Thread requestThread;

    Queue<long> timestampQueue = new Queue<long>();
    Queue<Signal> signalsQueue = new Queue<Signal>();
    Queue<KeyValuePair<long, Transform>> transformQueue = new Queue<KeyValuePair<long, Transform>>();

    private void Start()
    {
        Debug.Log("Started DB Polling");
        trackable = transform.GetComponent<TrackableBehaviour>();
        
        pi.ConnectServer(true, "192.168.2.23", 5005);
        database.ConnectDatabase("/Database/database.db");
        database.ClearTables();
        
        requestThread = new Thread(RequestThread)
        {
            IsBackground = true
        };
        requestThread.Start();
    }

    void RequestThread()
    {
        while (true)
        {
            if (abort)
            {
                return;
            }

            if (IsTracked)
            {
                long timestamp = Environment.TickCount;
                Signal signal = pi.RequestServer(timestamp);

                signalsQueue.Enqueue(signal);
                timestampQueue.Enqueue(timestamp);
            }

            Debug.Log("----------------- SLEEEEPING ----------------------");
            Thread.Sleep(100);
        }
    }

    private void AddLocation(long timestamp)
    {
        KeyValuePair<long, Transform> first;
        KeyValuePair<long, Transform> second = transformQueue.Dequeue();
        KeyValuePair<long, Transform> nearest = new KeyValuePair<long, Transform>(-1, null);

        while (transformQueue.Count > 0)
        {
            first = second;
            second = transformQueue.Dequeue();

            if (first.Key <= timestamp && second.Key >= timestamp)
            {
                if (timestamp - first.Key < second.Key - timestamp)
                {
                    nearest = first;
                }
                else
                {
                    nearest = second;
                }
                break;
            }
        }
        if(nearest.Key == -1)
        {
            nearest = second;
        }
        Transform correctedTransform = nearest.Value;
        Location location = new Location(timestamp, 
            correctedTransform.position.x, 
            correctedTransform.position.y, 
            correctedTransform.position.z, 
            correctedTransform.rotation.eulerAngles.x, 
            correctedTransform.rotation.eulerAngles.y, 
            correctedTransform.rotation.eulerAngles.z);
        database.Add(location);
    }

    private void AddSignal(Signal signals)
    {
        database.Add(signals);
    }

    private void Update()
    {
        transformQueue.Enqueue(new KeyValuePair<long, Transform>(Environment.TickCount, transform));

        if (timestampQueue.Count > 0)
        {
            long timestamp = timestampQueue.Dequeue();
            AddLocation(timestamp);
            AddSignal(signalsQueue.Dequeue());

        //    UpdateUI();
        }
    }

    private void UpdateUI()
    {
        List<Location> locations = database.Select<Location>();
        List<Signal> signals = database.Select<Signal>();

        locationsView.text = "";
        signalsView.text = "";

        foreach (Location location in locations)
        {
            locationsView.text += location.ToString();
        }
        foreach (Signal signal in signals)
        {
            signalsView.text += signal.ToString();
        }
    }

    private void OnApplicationQuit()
    {
        abort = true;

        pi.CloseConnection();
        database.CloseConnection();
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