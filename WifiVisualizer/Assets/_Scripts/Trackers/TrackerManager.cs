using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerManager : MonoBehaviour {

    public TrackerPolling trackerPrefab;
    public RectTransform trackerViewPrefab;
    public RectTransform listView;

    public InputField hostIF;
    public InputField portIF;
    public Button connect;

    private List<TrackerPolling> connectedTrackers;

    private void Start()
    {
        connectedTrackers = new List<TrackerPolling>();
        IDBConnector<DBConnectorMock>.Instance.ConnectDatabase("/Database/database.db");
        IDBConnector<DBConnectorMock>.Instance.ClearTables();

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

        TrackerPolling trackerTarget = Instantiate(trackerPrefab);
        RectTransform trackerView = Instantiate(trackerViewPrefab);

        trackerTarget.Setup(host, port, trackerView.GetComponent<TrackerViewButton>());
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
        IDBConnector<DBConnectorMock>.Instance.CloseConnection();
    }
}
