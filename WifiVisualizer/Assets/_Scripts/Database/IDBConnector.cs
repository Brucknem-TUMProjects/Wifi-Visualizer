using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDBConnector
{
    public List<Location> Locations { get; protected set; }
    public List<Signal> Signals { get; protected set; }
    public List<Measurement3D> Measurements { get; protected set; }

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
        Debug.Log("Default Connection close used");
    }

    public virtual void ClearTables()
    {
        Reset();
    }

    public void Reset()
    {
        Locations = new List<Location>();
        Signals = new List<Signal>();
        Measurements = new List<Measurement3D>();
    }

    public void Add(Location location)
    {
        Locations.Add(location);
        UpdateMeasurements(location);
    }

    public void Add(Signal signal)
    {
        Signals.Add(signal);
        UpdateMeasurements(signal);
    }

    public void Add(Measurement3D measurement)
    {
        Measurements.Add(measurement);
        Signals.Add(measurement.Signal);
        Locations.Add(measurement.Location);
    }

    public void UpdateMeasurements(Location location)
    {
        Measurement3D measurement = Find(location);

        if (measurement != null)
        {
            measurement.Location = location;
        }
        else
        {
            Measurements.Add(new Measurement3D(location, new Signal()));
        }
    }

    private Measurement3D Find(SQLable value)
    {
        foreach (Measurement3D m in Measurements)
        {
            if (m.Timestamp == value.Timestamp)
            {
                return m;
            }
        }
        return null;
    }

    public void UpdateMeasurements(Signal signal)
    {
        Measurement3D measurement = Find(signal);

        if (measurement != null)
        {
            measurement.Signal = signal;
        }
        else
        {
            Measurements.Add(new Measurement3D(new Location(), signal));
        }
    }

    public void AddAll(List<Location> locations)
    {
        foreach (Location location in locations)
        {
            Add(location);
        }
    }

    public void AddAll(List<Signal> signals)
    {
        foreach (Signal signal in signals)
        {
            Add(signal);
        }
    }
    
    public abstract List<U> Select<U>(long timestamp = -1) where U : SQLable, new();
}