using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITrackerConnector 
{
    public abstract void ConnectServer(string host, int port, int id, Action<int, float, bool> onFinish, Action<int> onClosed);
    public abstract Signal RequestServer();
    public abstract string RequestServer(string message);
    public abstract void CloseConnection(string message = "");
    public abstract bool IsConnected();

    public Signal ParseResponse(string response)
    {
        try
        {
                string[] values = response.Split(';');
                Signal signal = new Signal(values[0], values[1], int.Parse(values[2]));
                return signal;
        }
        catch
        {
        }

        return null;
    }
}
