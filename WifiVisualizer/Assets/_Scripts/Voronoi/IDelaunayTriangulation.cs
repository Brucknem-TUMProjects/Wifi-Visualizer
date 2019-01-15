using HullDelaunayVoronoi.Primitives;
using HullDelaunayVoronoi.Voronoi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IDelaunayTriangulation
{
    public List<Tetrahedron> Triangulation { get; protected set; }
    public List<Measurement3D> Measurements { get; protected set; }
    public List<Vector3> Extremes { get; protected set; }

    public IDelaunayTriangulation()
    {
        Triangulation = new List<Tetrahedron>();
        Measurements = new List<Measurement3D>();
        Extremes = new List<Vector3>();
    }

    private void CalculateExtremes()
    {
        float[] xx = {Measurements.Min(m => m.Position[0]), Measurements.Max(m => m.Position[0])};
        float[] yy = {Measurements.Min(m => m.Position[1]), Measurements.Max(m => m.Position[1])};
        float[] zz = {Measurements.Min(m => m.Position[2]), Measurements.Max(m => m.Position[2])};
        Extremes = new List<Vector3>();

        for(int x = 0; x < xx.Length; x++)
        {
            for (int y = 0; y < yy.Length; y++)
            {
                for (int z = 0; z < zz.Length; z++)
                {
                    Extremes.Add(new Vector3(xx[x], yy[y], zz[z]));
                }
            }
        }
    }

    public abstract void Generate(List<Measurement3D> measurements);

    public abstract Vector3 Centroid { get; }
}
