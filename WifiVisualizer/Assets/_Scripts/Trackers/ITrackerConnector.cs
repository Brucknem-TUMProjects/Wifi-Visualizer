using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITrackerConnector 
{
    public abstract void ConnectServer(string host, int port, int id, Action<int, bool> onFinish);
    public abstract Signal RequestServer(long timestamp);
    public abstract string RequestServer(string message);
    public abstract void CloseConnection(string message = "");
    public abstract bool IsConnected();
    
    public Signal ParseResponse(long timestamp, string response)
    {
        try
        {
            string[] rawSignals = response.Split('|');
            foreach (string rawSignal in rawSignals)
            {
                string[] values = rawSignal.Split(';');
                Signal signal = new Signal(timestamp, values[0], values[1], int.Parse(values[2]));
                return signal;
                // parsed.Add(signal);
            }
        }
        catch
        {
        }

        return null;
    }
}
