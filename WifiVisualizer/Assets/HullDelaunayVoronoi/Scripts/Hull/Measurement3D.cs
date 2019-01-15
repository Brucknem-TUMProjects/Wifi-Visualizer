using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurement3D : Vertex3
{
    private Location location;
    public Location Location {
        get
        {
            return location;
        }
        set
        {
            location = value;
            Position = new float[] { value.PosX, value.PosY, value.PosZ };
        }
    }
    public Signal Signal { get; set; }
    public long Timestamp
    {
        get
        {
            if (Location.Timestamp != -1)
            {
                return Location.Timestamp;
            }
            return Signal.Timestamp;
        }
    }

    public Color Color
    {
        get
        {
            return Signal.GetColor();
        }
    }

    public float Fallout
    {
        get
        {
            return Signal.GetFallout();
        }
    }
    
    public Measurement3D() : this(new Location(), new Signal()) { }
    public Measurement3D(Location location) : this(location, new Signal()) { }
    public Measurement3D(Signal signal) : this(new Location(), signal) { }

    public Measurement3D(Location location, Signal signal)
    {
        Location = location;
        Signal = signal;
    }
}