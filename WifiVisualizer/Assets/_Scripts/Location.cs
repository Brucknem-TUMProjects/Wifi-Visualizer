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

    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public Location() : this(0, 0, 0, 0, 0, 0, 0) { }

    public Location(long timestamp, Transform transform) : this(timestamp, transform.position.x, transform.position.y, transform.position.z, transform.rotation.x, transform.rotation.y, transform.rotation.z) { }

    public Location(long timestamp, Vector3 position) : this(timestamp, position.x, position.y, position.z) { }

    public Location(long Timestamp, float PosX, float PosY, float PosZ) : this(Timestamp, PosX, PosY, PosZ, 0, 0, 0) { }

    public Location(long Timestamp, float PosX, float PosY, float PosZ, float RotX, float RotY, float RotZ)
    {
        this.Timestamp = Timestamp;

        this.PosX = PosX;
        this.PosY = PosY;
        this.PosZ = PosZ;

        this.RotX = RotX;
        this.RotY = RotY;
        this.RotZ = RotZ;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(PosX, PosY, PosZ);
    }
    
    public override string ToString()
    {
        return Timestamp + ": Pos => [" + PosX + "," + PosY + "," + PosZ + "] Rot => [" + RotX + "," + RotY + "," + RotZ + "]";
    }
   
    public int CompareTo(Location other)
    {
        return (int)(Timestamp - other.Timestamp);
    }
}
