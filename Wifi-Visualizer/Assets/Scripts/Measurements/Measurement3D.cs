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
    private static readonly float MIN_TRANSPARENCY = 0.3f;
    private static readonly float MAX_TRANSPARENCY = 1.0f;
    private static readonly int EXPONENT = 4;

    public Color Color
    {
        get
        {
            float value = Mathf.Clamp((float)Decibel, MAX_DB, MIN_DB);
            value -= MIN_DB;
            value *= -1;

            float r = 0f;
            float g = 255f;

            if (value <= Delta / 2)
            {
                r = value / 25f * 255f;
            }
            else
            {
                r = 255f;
                g = 255f - ((value / 25f) * 255f);
            }

            return new Color(r / 255f, g / 255f, 0, 1);
        }
    }

    public float Falloff
    {
        get
        {
            return Normalized * 18 + 2;
        }
    }

    private float Normalized
    {
        get
        {
            return (float)(Decibel + MIN_DB) / (MAX_DB - MIN_DB);
        }
    }
    
    public float Transparency
    {
        get
        {
            float tmp = (float)-(Decibel - MIN_DB);
            return ((MAX_TRANSPARENCY - MIN_TRANSPARENCY) / (Delta * Delta)) * (tmp * tmp) + MIN_TRANSPARENCY;
        }
    }

    public float Delta
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