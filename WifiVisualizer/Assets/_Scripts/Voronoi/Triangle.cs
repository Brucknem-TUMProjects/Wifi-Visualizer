using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangle
{
    public List<Measurement3D> Measurements{get; private set;}

    public Vector3 Normal
    {
        get
        {
            return Vector3.Normalize(Vector3.Cross(Measurements[1].Position - Measurements[0], Measurements[2].Position - Measurements[0]));
        }
    }

    public Triangle(Measurement3D a, Measurement3D b, Measurement3D c)
    {
        Measurements = new List<Measurement3D>
        {
            a
        };
        if (Measurements.Contains(b))
        {
            throw new ArgumentException("Two points are equal!");
        }
        Measurements.Add(b);

        if (Measurements.Contains(c))
        {
            throw new ArgumentException("Two points are equal!");
        }
        Measurements.Add(c);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Triangle))
        {
            return false;
        }
        Triangle other = (Triangle)obj;
        return new HashSet<Measurement3D>(Measurements).SetEquals(new HashSet<Measurement3D>(other.Measurements));
    }

    public override int GetHashCode()
    {
        return -191684997 + EqualityComparer<HashSet<Measurement3D>>.Default.GetHashCode(new HashSet<Measurement3D>(Measurements));
    }

    public bool InSamePlane(Measurement3D other)
    {
        return Vector3.Dot(Normal, other.Position - Measurements[0]) == 0;
    }
}
