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
        string host = hostIF.text;
        int port = -1;
        int.TryParse(portIF.text, out port);

        if(host == "" || port == -1)
        {
            return;
        }

        int id = UnityEngine.Random.Range(0, dataset.TrackerTargets.Count);
        TrackerPolling trackerTarget = dataset.TrackerTargets[id];
        RectTransform trackerView = Instantiate(trackerViewPrefab);

        trackerTarget.gameObject.SetActive(true);
        trackerTarget.Setup(host, port, id, database, ConnectionSuccessful);
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

    public void ConnectionSuccessful(int id, bool connected)
    {
        if (!connected)
        {
            Remove(id);
        }
        else
        {
            actions.Enqueue(() => connectedTrackerViews[id].GetComponent<UnityEngine.UI.Image>().color = Color.green);
        }
    }

    private void Remove(int id)
    {
        actions.Enqueue(connectedTrackers[id].Remove);
        connectedTrackers.Remove(id);
        connectedTrackerViews.Remove(id);
    }

    private void OnApplicationQuit()
    {
        database.CloseConnection();
    }
}
