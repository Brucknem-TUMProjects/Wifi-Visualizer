using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Linq;
using System;

public class TrackerManager : MonoBehaviour {

    public RectTransform trackerViewPrefab;
    public RectTransform listView;

    public InputField hostIF;
    public InputField portIF;
    public Button connect;

    public DatasetLoader dataset;

    private Dictionary<int, TrackerPolling> connectedTrackers;
    private Dictionary<int, RectTransform> connectedTrackerViews;
    private IDBConnector database;

    Queue<Action> actions = new Queue<Action>();

    private void Start()
    {
        connectedTrackers = new Dictionary<int, TrackerPolling>();
        connectedTrackerViews = new Dictionary<int, RectTransform>();
        database = new DBConnector();
        database.ConnectDatabase("/Database/database.db");
        database.ClearTables();

        connect.onClick.AddListener(CreateTracker);
    }

    private void Update()
    {
        if(actions.Count > 0)
        {
            actions.Dequeue()();
        }
    }

    private void CreateTracker()
    {
        if(dataset.TrackerTargets.Count == connectedTrackers.Count)
        {
            return;
        }
        string host = hostIF.text;
        int port = -1;
        int.TryParse(portIF.text, out port);

        if(host == "" || port == -1)
        {
            return;
        }

        List<int> all = range(dataset.TrackerTargets.Count);
        all.RemoveAll(x => connectedTrackers.Keys.Contains(x));

        int id = all[0];
        TrackerPolling trackerTarget = dataset.TrackerTargets[id];
        RectTransform trackerView = Instantiate(trackerViewPrefab);

        trackerTarget.gameObject.SetActive(true);
        trackerTarget.Setup(host, port, id, database, ConnectionSuccessful, Remove);
        connectedTrackers.Add(id, trackerTarget);

        trackerView.SetParent(listView);
        trackerView.localScale = new Vector3(1, 1, 1);
        trackerView.localPosition = new Vector3(trackerView.localPosition.x, trackerView.localPosition.y, 1);

        trackerView.GetComponent<Button>().onClick.AddListener(delegate { Remove(id); });

        trackerView.GetComponent<TrackerViewButton>().Setup(trackerTarget);
        trackerView.GetComponentInChildren<Text>().text = host + ":" + port;
        connectedTrackerViews.Add(id, trackerView);

        hostIF.text = "";
        portIF.text = "";
    }

    public static List<int> range(int b)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < b; i++)
        {
            result.Add(i);
        }

        return result;
    }

    public void ConnectionSuccessful(int id, float width, bool connected)
    {
        if (!connected)
        {
            Remove(id);
        }
        else
        {
            actions.Enqueue(() => connectedTrackerViews[id].GetComponent<UnityEngine.UI.Image>().color = Color.green);
            //actions.Enqueue(() => connectedTrackers[id].transform.localScale = new Vector3(width, connectedTrackers[id].transform.localScale.y, width));
            actions.Enqueue(() => dataset.ResizeMarkers(width));
        }
    }

    private void Remove(int id)
    {
        try
        {
            var view = connectedTrackerViews[id];
            var tracker = connectedTrackers[id];
            connectedTrackers.Remove(id);
            connectedTrackerViews.Remove(id);
            actions.Enqueue(tracker.Remove);
            actions.Enqueue(() =>
            {
                try { Destroy(view.gameObject); } catch { };
            });
        }
        catch { }
    }
    
    private void OnApplicationQuit()
    {
        database.CloseConnection();
    }
}
