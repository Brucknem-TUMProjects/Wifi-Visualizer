using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerConnectorMock : ITrackerConnector
{ 
    override
    public void ConnectServer(string host, int port, int id, Action<int, float, bool> onFinish, Action<int> onClosed)
    {
        onFinish(id, 5, true);
        return;
    }

    public override void CloseConnection(string message = "")
    {
        return;
    }

    public override bool IsConnected()
    {
        return true;
    }

    public override Signal RequestServer(long timestamp)
    {
        return ParseResponse(timestamp, RequestServer("Yeet me dbsss"));
    }

    public override string RequestServer(string message)
    {
        try
        {
           int.Parse(message);
            //marker.sprite = markers[markerID];
            return "Success";
        }
        catch
        {
            return "88:88:88:88:88:88;FritzBox! TC7590;50";
        }
    }
}
