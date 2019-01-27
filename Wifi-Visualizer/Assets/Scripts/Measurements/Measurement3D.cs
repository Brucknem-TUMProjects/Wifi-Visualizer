using UnityEngine;

public class Measurement3D
{
    public string Id { get; private set; }

    public Vector3 Position { get; private set; }
    public string SSID { get; private set; }
    public string MAC { get; private set; }
    public double Decibel { get; private set; }

    public float[] PositionArray { get { return new float[] { Position.x, Position.y, Position.z }; } }

    private static readonly float MIN_DB = -20;
    private static readonly float MAX_DB = -80;
    private static readonly float MIN_TRANSPARENCY = 0.2f;
    private static readonly float MAX_TRANSPARENCY = 1.2f;
    private static readonly int EXPONENT = 4;

    public Color Color
    {
        get
        {
            float r = Normalized;
            float g = 1 - r;

            if (r > g)
            {
                g /= r;
                r = 1;
            }
            else
            {
                r /= g;
                g = 1;
            }

            return new Color(r, g, 0, 1);
        }
    }

    private float Normalized
    {
        get
        {
            return -(Mathf.Clamp((float)Decibel, MAX_DB, MIN_DB) - MIN_DB) / Delta;
        }
    }
    
    public float Transparency
    {
        get
        {
            float t = Normalized;
            float a = MAX_TRANSPARENCY - MIN_TRANSPARENCY;
            return a * Mathf.Pow(t, EXPONENT) + MIN_TRANSPARENCY;
        }
    }

    private float Delta
    {
        get
        {
            return -(MAX_DB - MIN_DB);
        }
    }
    
    public Measurement3D(Vector3 position, string ssid = "", string mac = "", double decibel = -80)
    {
        Position = position;
        SSID = ssid;
        MAC = mac;
        Decibel = decibel;
        Id = Position.ToString() + " - " + SSID + " - " + MAC + " - " + Decibel;
    }

    public static implicit operator Vector3(Measurement3D measurement)
    {
        return measurement.Position;
    }

    public static implicit operator float[] (Measurement3D measurement)
    {
        return measurement.PositionArray;
    }

    public override string ToString()
    {
        return Id;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return ((Measurement3D)obj).Position.Equals(Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}