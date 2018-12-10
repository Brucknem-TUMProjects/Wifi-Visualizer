using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using SQLite4Unity3d;

public class DBConnector : IDBConnector
{
    private SQLiteConnection dbconn;

    public bool isConnected = false;
    public string file;

    override
    public void ConnectDatabase(string file)
    {
        Debug.Log("Connect DB connection");

        dbconn = new SQLiteConnection(Application.dataPath + file);
        dbconn.CreateTable<Location>();
        dbconn.CreateTable<Signal>();

        ClearTables();
    }

    override
    public void AddLocation(Location location)
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

    override
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

    public override void AddSignal(Signal signal)
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

    public override void AddSignals(List<Signal> signals)
    {
        foreach (Signal signal in signals)
        {
            AddSignal(signal);
        }
    }

    public override List<Signal> QuerySignals(long timestamp = -1)
    {
        Debug.Log("SIGNALS returned");

        string query = "SELECT * FROM Signal";
        if (timestamp != -1)
        {
            return dbconn.Query<Signal>(query + " WHERE timestamp = ?", timestamp);
        }
        return dbconn.Query<Signal>(query);
    }

    public override void CloseConnection()
    {
        dbconn.Close();
        Debug.Log("Closed database connection");
    }

    public override void ClearTables()
    {
        List<Location> locations = dbconn.Query<Location>("DELETE FROM Location");
        List<Signal> signals = dbconn.Query<Signal>("DELETE FROM Signal");
        Debug.Log("LOCATIONS: " + locations.Count);
        Debug.Log("SIGNALS: " + signals.Count);
    }
}
