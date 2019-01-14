using HullDelaunayVoronoi.Primitives;
using HullDelaunayVoronoi.Voronoi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDelaunayTriangulation
{
    public List<Tetrahedron> Triangulation { get; set; }
    public List<Measurement3D> Measurements { get; set; }

    public IDelaunayTriangulation()
    {
    }

    public abstract void Generate(List<Measurement3D> measurements);

    public abstract Vector3 Centroid { get; }
}
