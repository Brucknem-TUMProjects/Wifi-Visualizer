using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITrackerConnector
{
    public enum ConnectionStatus
    {
        DISCONNECTED,
        CONNECTED,
        CONNECTING
    }


    public abstract void ConnectServer(string host, int port, Action<bool> onFinish);
    public abstract Signal RequestServer(long timestamp);
    public abstract void CloseConnection(string message = "");

    /** Weather there is a currently a connection to the host */
    protected ConnectionStatus status = ConnectionStatus.DISCONNECTED;
    public ConnectionStatus Status
    {
        get
        {
            return status;
        }
    }

    public Signal ParseResponse(long timestamp, string response)
    {
        string[] rawSignals = response.Split('|');
        foreach (string rawSignal in rawSignals)
        {
            string[] values = rawSignal.Split(';');
            Signal signal = new Signal(timestamp, values[0], values[1], -1 * int.Parse(values[2]));
            return signal;
           // parsed.Add(signal);
        }

        return null;
    }
}
