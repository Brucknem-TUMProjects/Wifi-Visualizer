using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : IComparable<Signal>
{

    [PrimaryKey]
    public long Timestamp { get; set; }

    public string Mac { get; set; }
    public string Name { get; set; }
    public int Decibel { get; set; } 

    public Signal() : this(0, "", "", 0)
    {

    }

    public Signal(string Mac, string Name, int Decibel) : this(0, Mac, Name, Decibel)
    {
        
    }

    public Signal(long Timestamp, string Mac, string Name, int Decibel)
    {
        this.Timestamp = Timestamp;
        this.Mac = Mac;
        this.Name = Name;
        this.Decibel = Decibel;
    }

    public override string ToString()
    {
        return Timestamp + ": " + Mac + "; " + Name + "; " + Decibel;
    }

    public int CompareTo(Signal other)
    {
        return (int)(Timestamp - other.Timestamp);
    }
}
