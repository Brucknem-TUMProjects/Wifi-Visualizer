using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurement3D
{
    public string Id { get; private set; }
    public bool IsArtificial { get; private set; }

    public Vector3 Position { get; private set; }
    public string SSID { get; private set; }
    public string MAC { get; private set; }
    public int Decibel { get; private set; }

    public float[] PositionArray { get { return new float[] { Position.x, Position.y, Position.z }; } }

    public Color Color
    {
        get
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

    public float Fallout
    {
        get
        {
                return (Decibel + 30.0f) / -50.0f;
            }
        }

    public Measurement3D(Vector3 position, string ssid = "", string mac = "", int decibel = -30, bool isArtificial = false)
    {
        Position = position;
        SSID = ssid;
        MAC = mac;
        Decibel = decibel;
        IsArtificial = isArtificial;
        Id = Position.ToString() + " - " + SSID + " - " + MAC + " - " + Decibel + " - " + isArtificial;
    }
    public Measurement3D(float x, float y, float z, string ssid = "", string mac = "", int decibel = -30, bool isArtificial = false) : this(new Vector3(x, y, z), ssid, mac, decibel, isArtificial) { }
    public Measurement3D(float x, float y, float z, bool isArtificial = false) : this(new Vector3(x, y, z), "", "", 0, isArtificial) { }
    public Measurement3D(float x, float y, float z) : this(new Vector3(x, y, z), "", "", 0, false) { }
    public Measurement3D(Vector3 position, Signal signal) : this(new Vector3(position.x, position.y, position.z), signal.SSID, signal.MAC, signal.Decibel, false) { }

    public static implicit operator Vector3(Measurement3D measurement)
    {
        return measurement.Position;
    }

    public static implicit operator float[](Measurement3D measurement)
    {
        return measurement.PositionArray;
    }

    public override string ToString()
    {
        return Id;
    }
}