using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Linq;

public class TrackerManager : MonoBehaviour {

    public RectTransform trackerViewPrefab;
    public RectTransform listView;

    public InputField hostIF;
    public InputField portIF;
    public Button connect;

    public DatasetLoader dataset;

    private List<TrackerPolling> connectedTrackers;
    private IDBConnector database;

    private void Start()
    {
        connectedTrackers = new List<TrackerPolling>();
        database = new DBConnector();
        database.ConnectDatabase("/Database/database.db");
        database.ClearTables();

        connect.onClick.AddListener(CreateTracker);
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

        TrackerPolling trackerTarget = dataset.TrackerTargets[0];
        trackerTarget.gameObject.SetActive(true);
        RectTransform trackerView = Instantiate(trackerViewPrefab);

        trackerTarget.Setup(host, port, database, trackerView.GetComponent<TrackerViewButton>());
        connectedTrackers.Add(trackerTarget);

        trackerView.SetParent(listView);
        trackerView.localScale = new Vector3(1, 1, 1);
        trackerView.localPosition = new Vector3(trackerView.localPosition.x, trackerView.localPosition.y, 1);

        trackerView.GetComponent<TrackerViewButton>().Setup(trackerTarget);
        trackerView.GetComponentInChildren<Text>().text = host + ":" + port;

        hostIF.text = "";
        portIF.text = "";
    }

    private void OnApplicationQuit()
    {
        database.CloseConnection();
    }
}
