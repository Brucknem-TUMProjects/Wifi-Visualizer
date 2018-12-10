using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DBConnectorMock
{
    protected List<Location> locations;
    protected List<Signal> signals;
    
    public void ConnectDatabase(string file)
    {
        ClearTables();
        Debug.Log("Mock connect DB connection");
    }

    public void CloseConnection()
    {
        Debug.Log("Mock close DB connection");
    }

    public void AddLocation(Location location)
    {
        Debug.Log("Mock LOCATION added " + location);
        locations.Add(location);
    }

    public List<Location> GetLocations(long timestamp = -1)
    {
        Debug.Log("Mock LOCATIONS returned");
        return locations;
    }

    public void AddSignal(Signal signal)
    {
        Debug.Log("Mock SIGNAL added " + signal);
        signals.Add(signal);
    }

    public List<Signal> GetSignals(long timestamp = -1)
    {
        Debug.Log("Mock SIGNALS returned");
        if (timestamp == -1)
        {
            return signals;
        }
        return signals.Where(signal => signal.Timestamp == timestamp).ToList<Signal>();
    }

    public void AddSignals(List<Signal> signals)
    {
        foreach(Signal signal in signals)
        {
            AddSignal(signal);
        }
    }

    public void ClearTables()
    {
        locations = new List<Location>();
        signals = new List<Signal>();
    }
}
