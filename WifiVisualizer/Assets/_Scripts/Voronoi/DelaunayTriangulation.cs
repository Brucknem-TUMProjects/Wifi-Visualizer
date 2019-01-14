using HullDelaunayVoronoi.Delaunay;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelaunayTriangulation : IDelaunayTriangulation
{
    public override Vector3 Centroid
    {
        get
        {
            Vector3 sum = Vector3.zero;
            if(Measurements.Count == 0)
            {
                return sum;
            }
            foreach(Measurement3D measurement in Measurements)
            {
                sum += measurement.Location;
            }
            return sum / Measurements.Count;
        }
    }

    public override void Generate(List<Measurement3D> measurements)
    {
        Measurements = measurements;
        Triangulation = new List<Tetrahedron>();
        DelaunayTriangulation3<Measurement3D> triangulation = new DelaunayTriangulation3<Measurement3D>();
        triangulation.Generate(measurements);
        foreach (DelaunayCell<Measurement3D> cell in triangulation.Cells)
        {
            Triangulation.Add(new Tetrahedron(cell.Simplex.Vertices));
        }
    }
}