using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DBConnector
{
    private SQLiteConnection dbconn;
    private List<Location> locations;
    private List<Signal> signals;

    public bool isConnected = false;
    public string file;
        
    public void ConnectDatabase(string file)
    {
        Debug.Log("Connect DB connection");
        Reset();
        dbconn = new SQLiteConnection(Application.dataPath + file);
    }

    public void Reset()
    {
        locations = new List<Location>();
        signals = new List<Signal>();
    }

    public void Add(SQLable value)
    {
        if (value.GetType() == typeof(Location))
        {
            locations.Add((Location) value);
        }
        else if (value.GetType() == typeof(Signal))
        {
            signals.Add((Signal) value);
        }
    }

    public void AddAll(List<SQLable> values)
    {
        foreach (SQLable value in values)
        {
            Add(value);
        }
    }

    public List<Location> GetLocations()
    {
        return locations;
    }

    public List<Signal> GetSignals()
    {
        return signals;
    }

    private void Write<T>(T value) where T : SQLable, new()
    {
        Write(new List<T>() { value });
    }

    private void Write<T>(List<T> values) where T : SQLable, new()
    {
        if (values.Count <= 0)
        {
            return;
        }

        dbconn.CreateTable<T>();
        string query = "INSERT INTO " + values[0].GetType().ToString() + " VALUES\n";

        foreach (T value in values)
        {
            query += value.ToSqlValueList() + ",\n";
        }

        query = query.Substring(0, query.Length - 2);
        Debug.Log(query);
        dbconn.Query<T>(query);
    }

    public List<T> Select<T>(long timestamp = -1) where T : SQLable, new()
    {
        string name = typeof(T).ToString();
        
        string query = "SELECT * FROM " + name;

        if (timestamp != -1)
        {
            query += " WHERE timestamp = " + timestamp;
        }
        List<T> result = dbconn.Query<T>(query);
        Debug.Log(result.Count + " " + name.ToUpper() + "S returned");
        return result;
    }

    public void ClearTables()
    {
        dbconn.Query<Location>("DELETE FROM Location");
        dbconn.Query<Signal>("DELETE FROM Signal");
    }

    public void CloseConnection()
    {
        Write(locations);
        Write(signals);

        Select<Location>();
        Select<Signal>();

        dbconn.Close();
    }
}