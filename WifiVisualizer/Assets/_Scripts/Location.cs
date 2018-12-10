using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : IComparable<Location>
{

    [PrimaryKey]
    public long Timestamp { get; set; }

    public double PosX { get; set; }
    public double PosY { get; set; }
    public double PosZ { get; set; }

    public double RotX { get; set; }
    public double RotY { get; set; }
    public double RotZ { get; set; }

    public Location() : this(0, 0, 0, 0, 0, 0, 0)
    {

    }

    public Location(long Timestamp, double PosX, double PosY, double PosZ, double RotX, double RotY, double RotZ)
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
