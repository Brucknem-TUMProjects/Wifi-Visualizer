using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : SQLable, IComparable<Signal>
{
    public string Mac { get; set; }
    public string Ssid { get; set; }
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
        this.Ssid = Name;
        this.Decibel = Decibel;
    }

    public override string ToString()
    {
        return Timestamp + ": " + Mac + "; " + Ssid + "; " + Decibel;
    }

    public int CompareTo(Signal other)
    {
        return (int)(Timestamp - other.Timestamp);
    }
}
