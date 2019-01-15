using System.Collections.Generic;
using System;
using UnityEngine;
using HullDelaunayVoronoi.Primitives;

public class Tetrahedron
{
    public List<Measurement3D> Measurements { get; private set; }
    public List<int> Triangles { get; private set; }
    public List<Vector3> Vertices { get; private set; }
    public List<Triangle> Edges { get; private set; }
    public CircumSphere CircumSphere { get; private set; }


    public Tetrahedron(Measurement3D a, Measurement3D b, Measurement3D c, Measurement3D d)
    {
        Measurements = new List<Measurement3D>()
        {
            a,b,c,d
        };
        CalculateVertices();
        CalculateTriangles();
        CalculateEdges();
        CircumSphere = new CircumSphere(Vertices);
    }

    public Tetrahedron(params Measurement3D[] measurements) : this(measurements[0], measurements[1], measurements[2], measurements[3]) { }

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
    public void CalculateEdges()
    {
        Edges.Add(new Triangle(Measurements[0], Measurements[1], Measurements[2]));
        Edges.Add(new Triangle(Measurements[0], Measurements[1], Measurements[3]));
        Edges.Add(new Triangle(Measurements[0], Measurements[2], Measurements[3]));
        Edges.Add(new Triangle(Measurements[1], Measurements[2], Measurements[3]));
    }
    public Vector3 CalculateNormal(int p1, int p2, int p3)
    {
        return Vector3.Cross(Vertices[p2] - Vertices[p1], Vertices[p3] - Vertices[p1]);
    }
    public static float Volume(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        return Vector3.Dot(Vector3.Cross(b - a, c - a), d - a) / 6f;
    }
    public static float Volume(Measurement3D a, Measurement3D b, Measurement3D c, Vector3 d)
    {
        return Volume(a.Location.GetPosition(), b.Location.GetPosition(), c.Location.GetPosition(), d);
    }
    public Color BarycentricInterpolation(Vector3 measurement)
    {
        Color color = new Color(0,0,0,0);

        color += Multiply(Measurements[0].Color, Volume(Measurements[1], Measurements[2], Measurements[3], measurement));
        color += Multiply(Measurements[1].Color, Volume(Measurements[2], Measurements[3], Measurements[0], measurement));
        color += Multiply(Measurements[2].Color, Volume(Measurements[3], Measurements[0], Measurements[1], measurement));
        color += Multiply(Measurements[3].Color, Volume(Measurements[0], Measurements[1], Measurements[2], measurement));

        return color;
    }
    private static Color Multiply(Color color, float value)
    {
        return new Color(
        color.a * value,
        color.r * value,
        color.g * value,
        color.b * value);
    }
    public bool Includes(Measurement3D measurement)
    {
        return CircumSphere.Includes(measurement.Location);
    }
}