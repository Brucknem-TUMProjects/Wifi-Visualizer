﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerConnectorMock : ITrackerConnector
{ 
    override
    public void ConnectServer(string host, int port, Action<bool> onFinish)
    {
        onFinish(true);
        return;
    }

    public override void CloseConnection(string message = "")
    {
        return;
    }

    override
    public Signal RequestServer(long timestamp, string message = "")
    {
            string response = "88:88:88:88:88:88;FritzBox! TC7590;50";
            return ParseResponse(timestamp, response);
    }

    public override bool IsConnected()
    {
        return true;
    }
}
