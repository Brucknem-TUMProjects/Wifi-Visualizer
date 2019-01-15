using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Location : SQLable, IComparable<Location>
{
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public Location() : this(-1, 0, 0, 0) { }

    public Location(long timestamp, Transform transform) : this(timestamp, transform.position.x, transform.position.y, transform.position.z) { }

    public Location(long timestamp, Vector3 position) : this(timestamp, position.x, position.y, position.z) { }

    public Location(long Timestamp, float PosX, float PosY, float PosZ)
    {
        this.Timestamp = Timestamp;

        this.PosX = PosX;
        this.PosY = PosY;
        this.PosZ = PosZ;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(PosX, PosY, PosZ);
    }
    
    public override string ToString()
    {
        return Timestamp + ": Pos => [" + PosX + "," + PosY + "," + PosZ;
    }
   
    public int CompareTo(Location other)
    {
        return (int)(Timestamp - other.Timestamp);
    }

    public static implicit operator Vector3(Location other)
    {
        return new Vector3(other.PosX, other.PosY, other.PosZ);
    }
}
