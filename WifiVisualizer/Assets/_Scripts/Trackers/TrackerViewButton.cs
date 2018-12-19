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
}
