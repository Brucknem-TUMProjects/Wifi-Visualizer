using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPiConnector
{
    public abstract void ConnectServer(bool isIp, string host, int port);
    public abstract List<Signal> RequestServer(long timestamp);
    public abstract void CloseConnection(string message = "");

    public List<Signal> ParseResponse(long timestamp, string response)
    {
        List<Signal> parsed = new List<Signal>();
        string[] rawSignals = response.Split('|');
        foreach (string rawSignal in rawSignals)
        {
            string[] values = rawSignal.Split(';');
            Signal signal = new Signal(timestamp, values[0], values[1], -1 * int.Parse(values[2]));
            parsed.Add(signal);
        }

        return parsed;
    }
}
