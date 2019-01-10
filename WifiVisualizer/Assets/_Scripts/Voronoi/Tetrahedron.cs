using System.Collections.Generic;
using System;
using UnityEngine;
using HullDelaunayVoronoi.Primitives;

public class Tetrahedron
{
    public Vertex3[] Corners { get; private set; }
    private double a, dx, dy, dz, c;
    public Circumsphere Circumsphere { get; private set; }

    public Tetrahedron(Vertex3[] corners)
    {
        if(corners.Length != 4)
        {
            throw new ArgumentException("A tetrahedron has 4 corners!");
        }
        Corners = corners;
        //CalculateCircumsphere();
    }

    public Tetrahedron(List<Vertex3> corners) : this(corners.ToArray()) { }

    public Vector3[] Vertices
    {
        get
        {
            Vector3[] verts = new Vector3[Corners.Length];
            for (int i = 0; i < Corners.Length; i++)
            {
                verts[i] = new Vector3(Corners[i].X, Corners[i].Y, Corners[i].Z);
            }
            return verts;
        }
    }

    //private void CalculateCircumsphere()
    //{
    //    Circumsphere = new Circumsphere();
    //    CalculateA();
    //    CalculateC();
    //    CalculateDx();
    //    CalculateDy();
    //    CalculateDz();
    //    CalculateCenter();
    //    CalculateRadius();
    //}

    //private void CalculateA() { 
    //    a = new Matrix(
    //            Vertices[0].x, Vertices[0].y, Vertices[0].z, 1,
    //            Vertices[1].x, Vertices[1].y, Vertices[1].z, 1,
    //            Vertices[2].x, Vertices[2].y, Vertices[2].z, 1,
    //            Vertices[3].x, Vertices[3].y, Vertices[3].z, 1
    //        ).Determinante;
    //}

    //private void CalculateC()
    //{
    //    c = -new Matrix(
    //        Math.Pow(Vertices[0].x,2) + Math.Pow(Vertices[0].y,2) + Math.Pow(Vertices[0].z,2), Vertices[0].x, Vertices[0].y, Vertices[0].z,
    //        Math.Pow(Vertices[1].x,2) + Math.Pow(Vertices[1].y,2) + Math.Pow(Vertices[1].z,2), Vertices[1].x, Vertices[1].y, Vertices[1].z,
    //        Math.Pow(Vertices[2].x,2) + Math.Pow(Vertices[2].y,2) + Math.Pow(Vertices[2].z,2), Vertices[2].x, Vertices[2].y, Vertices[2].z,
    //        Math.Pow(Vertices[3].x,2) + Math.Pow(Vertices[3].y,2) + Math.Pow(Vertices[3].z,2), Vertices[3].x, Vertices[3].y, Vertices[3].z
    //        ).Determinante;
    //}

    //private void CalculateDx()
    //{
    //    dx = new Matrix(
    //        Math.Pow(Vertices[0].x,2) + Math.Pow(Vertices[0].y,2) + Math.Pow(Vertices[0].z,2), Vertices[0].y, Vertices[0].z, 1,
    //        Math.Pow(Vertices[1].x,2) + Math.Pow(Vertices[1].y,2) + Math.Pow(Vertices[1].z,2), Vertices[1].y, Vertices[1].z, 1,
    //        Math.Pow(Vertices[2].x,2) + Math.Pow(Vertices[2].y,2) + Math.Pow(Vertices[2].z,2), Vertices[2].y, Vertices[2].z, 1,
    //        Math.Pow(Vertices[3].x,2) + Math.Pow(Vertices[3].y,2) + Math.Pow(Vertices[3].z,2), Vertices[3].y, Vertices[3].z, 1
    //        ).Determinante;
    //}

    //private void CalculateDy()
    //{
    //    dy = -new Matrix(
    //        Math.Pow(Vertices[0].x,2) + Math.Pow(Vertices[0].y,2) + Math.Pow(Vertices[0].z,2), Vertices[0].x, Vertices[0].z, 1,
    //        Math.Pow(Vertices[1].x,2) + Math.Pow(Vertices[1].y,2) + Math.Pow(Vertices[1].z,2), Vertices[1].x, Vertices[1].z, 1,
    //        Math.Pow(Vertices[2].x,2) + Math.Pow(Vertices[2].y,2) + Math.Pow(Vertices[2].z,2), Vertices[2].x, Vertices[2].z, 1,
    //        Math.Pow(Vertices[3].x,2) + Math.Pow(Vertices[3].y,2) + Math.Pow(Vertices[3].z,2), Vertices[3].x, Vertices[3].z, 1
    //        ).Determinante;
    //}

    //private void CalculateDz()
    //{
    //    dz = new Matrix(
    //        Math.Pow(Vertices[0].x,2) + Math.Pow(Vertices[0].y,2) + Math.Pow(Vertices[0].z,2), Vertices[0].x, Vertices[0].y, 1,
    //        Math.Pow(Vertices[1].x,2) + Math.Pow(Vertices[1].y,2) + Math.Pow(Vertices[1].z,2), Vertices[1].x, Vertices[1].y, 1,
    //        Math.Pow(Vertices[2].x,2) + Math.Pow(Vertices[2].y,2) + Math.Pow(Vertices[2].z,2), Vertices[2].x, Vertices[2].y, 1,
    //        Math.Pow(Vertices[3].x,2) + Math.Pow(Vertices[3].y,2) + Math.Pow(Vertices[3].z,2), Vertices[3].x, Vertices[3].y, 1
    //        ).Determinante;
    //}

    //private void CalculateCenter()
    //{
    //    Circumsphere.Center = new Vector3((float)(dx / (2 * a)), (float)(dy/ (2 * a)), (float)(dz / (2 * a)));
    //}

    //private void CalculateRadius()
    //{
    //    double dx2 = Math.Pow(dx, 2);
    //    double dy2 = Math.Pow(dy, 2);
    //    double dz2 = Math.Pow(dz, 2);
    //    double ac4 = (4 * a * c);
    //    double absa = Math.Abs(a);
    //    double absa2 = (2 * absa);
    //    double sum = dx2 + dy2 + dz2 - ac4;
    //    Circumsphere.Radius = Math.Sqrt(sum) / absa2;
    //}

    //private void RenderCircumsphere()
    //{
    //    Circumsphere.Render();
    //}

    public void Render()
    {
        int[] indices = {
            0,2,1,
            1,2,3,
            0,1,3,
            0,3,2
        };
        Mesh mesh = new Mesh
        {
            vertices = Vertices,
            triangles = indices
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject super = new GameObject("Tetrahedron");
        MeshRenderer rend = super.AddComponent<MeshRenderer>();
        MeshFilter mf = super.AddComponent<MeshFilter>();
        rend.material = new Material(Shader.Find("Standard"));
        mf.mesh = mesh;

        //foreach (Measurement3D measurement in Corners)
        //{
        //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    //obj.transform.position = measurement.Position;
        //    obj.transform.localScale = Vector3.one * 0.01f;
        //}

        //RenderCircumsphere();
    }
}