using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DBConnectorMock : IDBConnector
{
    List<Location> locations;
    List<Signal> signals;


    public override void ConnectDatabase(string file)
    {
        ClearTables();
        Debug.Log("Mock connect DB connection");
    }

    public override void CloseConnection()
    {
        Debug.Log("Mock close DB connection");
    }

    public override void AddLocation(Location location)
    {
        Debug.Log("Mock LOCATION added " + location);
        locations.Add(location);
    }

    public override List<Location> QueryLocations(long timestamp = -1)
    {
        Debug.Log("Mock LOCATIONS returned");
        return locations;
    }

    public override void AddSignal(Signal signal)
    {
        Debug.Log("Mock SIGNAL added " + signal);
        signals.Add(signal);
    }

    public override List<Signal> QuerySignals(long timestamp = -1)
    {
        Debug.Log("Mock SIGNALS returned");
        if (timestamp == -1)
        {
            return signals;
        }
        return signals.Where(signal => signal.Timestamp == timestamp).ToList<Signal>();
    }

    public override void AddSignals(List<Signal> signals)
    {
        foreach(Signal signal in signals)
        {
            AddSignal(signal);
        }
    }

    public override void ClearTables()
    {
        locations = new List<Location>();
        signals = new List<Signal>();
    }
}
