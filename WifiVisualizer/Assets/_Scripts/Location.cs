using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : IComparable<Location>
{

    [PrimaryKey]
    public long Timestamp { get; set; }

    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public Location() : this(0, 0, 0, 0, 0, 0, 0)
    {

    }

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

    override
    public string ToString()
    {
        return Timestamp + ": Pos => [" + PosX + "," + PosY + "," + PosZ + "] Rot => [" + RotX + "," + RotY + "," + RotZ + "]";
    }

    public int CompareTo(Location other)
    {
        return (int)(Timestamp - other.Timestamp);
    }
}
