using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDBConnector<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    protected List<Location> locations;
    protected List<Signal> signals;

    protected bool isConnected = false;
    protected string file;

    public virtual void ConnectDatabase(string file)
    {
        this.file = file;
        Reset();
    }

    public virtual void CloseConnection()
    {
        //Default implementation for override
        Debug.Log(locations + "" + signals);
    }

    public virtual void ClearTables()
    {
        Reset();
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
            locations.Add((Location)value);
        }
        else if (value.GetType() == typeof(Signal))
        {
            signals.Add((Signal)value);
        }
    }

    public void AddAll(List<SQLable> values)
    {
        foreach (SQLable value in values)
        {
            Add(value);
        }
    }

    public void AddAll(params SQLable[] values)
    {
        AddAll(new List<SQLable>(values));
    }

    public List<Location> GetLocations()
    {
        return locations;
    }

    public List<Signal> GetSignals()
    {
        return signals;
    }

    public abstract List<U> Select<U>(long timestamp = -1) where U : SQLable, new();
}