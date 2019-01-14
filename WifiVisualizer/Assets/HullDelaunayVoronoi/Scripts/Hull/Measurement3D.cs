using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurement3D : Vertex3
{
    //public Location Location { get; set; }
    public Signal Signal { get; set; }
    public Color Color
    {
        get
        {
            return Signal.Color();
        }
    }

    public Measurement3D() : this(new Location(), new Signal()) { }

    public Measurement3D(Location location, Signal signal)
    {
        //this.Location = location;
        this.Signal = signal;
        Position = new float[] { location.PosX, location.PosY, location.PosZ };
    }



    

    //public Vector3 Position
    //{
    //    get
    //    {
    //        return new Vector3(Location.PosX, Location.PosY, Location.PosZ);
    //    }
    //    set
    //    {
    //        Location.PosX = value.x;
    //        Location.PosY = value.y;
    //        Location.PosZ = value.z;
    //    }
    //}

    //public int Dimension
    //{
    //    get
    //    {
    //        return 3;
    //    }
    //}

    //public int Id { get; set; }
    //public int Tag { get; set; }
    //float[] IVertex.Position
    //{
    //    get
    //    {
    //        float[] pos = new float[3];
    //        pos[0] = Position.x;
    //        pos[1] = Position.y;
    //        pos[2] = Position.z;
    //        return pos;
    //    }

    //    set
    //    {
    //        Position = new Vector3(value[0], value[1], value[2]);
    //    }
    //}

    //public float Magnitude
    //{
    //    get
    //    {
    //        return Position.magnitude;
    //    }
    //}

    //public float SqrMagnitude
    //{
    //    get
    //    {
    //        return Position.sqrMagnitude;
    //    }
    //}

    //public override bool Equals(object other)
    //{
    //    if(!(other is Measurement3D)){
    //        return false;
    //    }

    //    Measurement3D otherMeasurement = (Measurement3D)other;
    //    return (Location.GetPosition() - otherMeasurement.Location.GetPosition()).sqrMagnitude < 0.00000001;
    //}

    //public override int GetHashCode()
    //{
    //    return Position.GetHashCode();
    //}

    //public float Distance(IVertex v)
    //{
    //    Vector3 other = new Vector3(v.Position[0], v.Position[1], v.Position[2]);
    //    return Vector3.Distance(other, Position);
    //}

    //public float SqrDistance(IVertex v)
    //{
    //    return Mathf.Pow(Distance(v), 2);
    //}
}