using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerViewButton : Button {

    private TrackerPolling trackerObject;

    protected override void Start()
    {
        base.Start();
        onClick.AddListener(OnButton);
    }

    public void Setup(TrackerPolling trackerObject)
    {
        this.trackerObject = trackerObject;
        SetConnectionStatus(ITrackerConnector.ConnectionStatus.CONNECTING);
    }

    private void OnButton()
    {
        trackerObject.Remove();
        Destroy(gameObject);
    }

    public void SetConnected()
    {
        targetGraphic.color = Color.green;
    }

    public void SetConnectionStatus(ITrackerConnector.ConnectionStatus status)
    {
            switch (status)
            {
                case ITrackerConnector.ConnectionStatus.CONNECTED:
                SetConnected();
                break;
                case ITrackerConnector.ConnectionStatus.CONNECTING:
                    targetGraphic.color = Color.yellow;
                    break;
                case ITrackerConnector.ConnectionStatus.DISCONNECTED:
                    targetGraphic.color = Color.red;
                    break;
            }
    }
}
