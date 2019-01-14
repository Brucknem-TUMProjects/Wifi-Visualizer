using System.Collections.Generic;
using System;
using UnityEngine;
using HullDelaunayVoronoi.Primitives;

public class Tetrahedron
{
    public List<Measurement3D> Measurements { get; private set; }
    public List<int> Triangles { get; private set; }
    public List<Vector3> Vertices { get; private set; }

    public Tetrahedron(params Measurement3D[] measurements) : this(new List<Measurement3D>(measurements)) { }

    public Tetrahedron(List<Measurement3D> measurements)
    {
        this.Measurements = measurements;
        CalculateVertices();
        CalculateTriangles();
    }

    private void CalculateVertices()
    {
        Vertices = new List<Vector3>();
        foreach (Vertex3 vertex in Measurements)
        {
            Vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
        }
    }
    private void CalculateTriangles()
    {
        Triangles = new List<int>();
        CalculateTriangle(0, 1, 2, 3);
        CalculateTriangle(0, 1, 3, 2);
        CalculateTriangle(0, 2, 3, 1);
        CalculateTriangle(3, 1, 2, 0);
    }

    private void CalculateTriangle(int p1, int p2, int p3, int vv)
    {
        //Triangles.AddRange(new int[] {
        //    0,1,2,
        //    0,2,1,
        //    0,1,3,
        //    0,3,1,
        //    0,2,3,
        //    0,3,2,
        //    1,2,3,
        //    1,3,2 });
        Vector3 normal = CalculateNormal(p1, p2, p3);
        Vector3 v = Vertices[vv];

        float dotp = Vector3.Dot(normal, v - Vertices[p1]);
        Triangles.Add(p1);
        if (dotp < 0)
        {
            Triangles.Add(p2);
            Triangles.Add(p3);
        }
        else
        {
            Triangles.Add(p3);
            Triangles.Add(p2);
        }
    }

    public Vector3 CalculateNormal(int p1, int p2, int p3)
    {
        return Vector3.Cross(Vertices[p2] - Vertices[p1], Vertices[p3] - Vertices[p1]);
    }  
}