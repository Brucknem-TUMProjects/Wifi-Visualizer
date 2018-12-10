using SQLite4Unity3d;
using System.Collections.Generic;
using UnityEngine;

public class DBConnector : DBConnectorMock
{
    private SQLiteConnection dbconn;
    
    public bool isConnected = false;
    public string file;

    
    public new void ConnectDatabase(string file)
    {
        Debug.Log("Connect DB connection");
        base.ConnectDatabase(file);
        dbconn = new SQLiteConnection(Application.dataPath + file);
        dbconn.CreateTable<Location>();
        dbconn.CreateTable<Signal>();
    }

    
    public void WriteLocation(Location location)
    {
        try
        {
            dbconn.Insert(location);
            Debug.Log("LOCATION added " + location);
        }
        catch (SQLiteException)
        {
            Debug.LogError("Location already in database: " + location);
        }
    }

    public void WriteLocations(List<Location> locations)
    {
        foreach(Location location in locations)
        {
            WriteLocation(location);
        }
    }

    
    public List<Location> QueryLocations(long timestamp = -1)
    {
        Debug.Log("LOCATIONS returned");
        string query = "SELECT * FROM Location";
        if (timestamp != -1)
        {
            return dbconn.Query<Location>(query + " WHERE timestamp = ?", timestamp);
        }
        return dbconn.Query<Location>(query);
    }

    public  void WriteSignal(Signal signal)
    {
        try
        {
            dbconn.Insert(signal);
            Debug.Log("SIGNAL added " + signal);
        }
        catch (SQLiteException)
        {
            Debug.LogError("Signal already in database: " + signal);
        }
    }

    public  void WriteSignals(List<Signal> signals)
    {
        foreach (Signal signal in signals)
        {
            WriteSignal(signal);
        }
    }

    public List<Signal> QuerySignals(long timestamp = -1)
    {
        Debug.Log("SIGNALS returned");

        string query = "SELECT * FROM Signal";
        if (timestamp != -1)
        {
            return dbconn.Query<Signal>(query + " WHERE timestamp = ?", timestamp);
        }
        return dbconn.Query<Signal>(query);
    }

    public new void CloseConnection()
    {
        WriteLocations(locations);
        WriteSignals(signals);
        dbconn.Close();
        Debug.Log("Closed database connection");
    }

    public new void ClearTables()
    {
        base.ClearTables();
        dbconn.Query<Location>("DELETE FROM Location");
        dbconn.Query<Signal>("DELETE FROM Signal");
    }
}
