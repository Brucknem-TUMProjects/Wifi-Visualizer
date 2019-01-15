using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangle
{
    HashSet<Measurement3D> set;

    public Triangle(Measurement3D a, Measurement3D b, Measurement3D c)
    {
        if (!set.Add(a) || !set.Add(b) || !set.Add(c))
        {
            throw new ArgumentException("Two points are equal!");
        }
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Triangle))
        {
            return false;
        }
        Triangle other = (Triangle)obj;
        return set.SetEquals(other.set);
    }

    public List<Measurement3D> Corners
    {
        get
        {
            return set.ToList();
        }
    }
}
