using SQLite4Unity3d;
using System;
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

    public Color GetColor()
    {
        float value = Mathf.Clamp(Decibel, -80f, -30f);
        value += 30;
        value *= -1;

        float r = 0f;
        float g = 255f;

        if (value <= 25)
        {
            r = value / 25f * 255f;
        }
        else
        {
            r = 255f;
            g = 255f - ((value / 25f) * 255f);
        }

        return new Color(r / 255f, g / 255f, 0);
    }
}
