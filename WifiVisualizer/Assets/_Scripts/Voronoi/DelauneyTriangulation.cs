using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelauneyTriangulation {
    
    IList<Measurement3D> allMeasuerements;
    CubeHull cubeHull;
    private IList<DelaunayCell<Vertex3>> cells;

    //List<Tetrahedron> triangulation;

    public DelauneyTriangulation(IList<Measurement3D> measurements)
    {
        DelaunayTriangulation3 triangulation = new DelaunayTriangulation3();
        IList<Vertex3> vertices = new List<Vertex3>();

        foreach(Measurement3D measurement in measurements)
        {
            vertices.Add(measurement);
        }

        triangulation.Generate(vertices);

        cells = triangulation.Cells;
        Render();
        //cubeHull = new CubeHull(measurements);
        //allMeasuerements = new List<Measurement3D>();
        //allMeasuerements.AddRange(measurements);
        //allMeasuerements.AddRange(cubeHull.Hull);
        //Triangulate();
    }

    public void Render()
    {
        foreach(DelaunayCell<Vertex3> cell in cells)
        {
            Tetrahedron tetra = new Tetrahedron(cell.Simplex.Vertices);
            tetra.Render();
        }
    }
    
    private void Triangulate()
    {
        //triangulation = new List<Tetrahedron>() { cubeHull.SuperTetrahedron };

        Measurement3D[] corners =
        {
            //new Measurement3D(new Location(0,-1,-4,7), new Signal()),
            //new Measurement3D(new Location(0,8,-9,3), new Signal()),
            //new Measurement3D(new Location(0,7,4,-2), new Signal()),
            //new Measurement3D(new Location(0,5,1,6), new Signal()),
            new Measurement3D(new Location(0,0,0,0), new Signal()),
            new Measurement3D(new Location(0,30,0,0), new Signal()),
            new Measurement3D(new Location(0,0,10,0), new Signal()),
            new Measurement3D(new Location(0,0,0,5), new Signal()),
        };

        TestSuperTetrahedron(new Tetrahedron(corners));
        //TestSuperTetrahedron(triangulation[0]);

        //foreach (Measurement3D measurement in allMeasuerements)
        //{
        //    List<Tetrahedron> badTriangles = new List<Tetrahedron>();
        //    foreach(Tetrahedron tetrahedron in triangulation)
        //    {

        //    }
        //}

        //Debug.Log(triangulation);
    }

    class MatrixDeterminant
    {
                //this method determines the sign of the elements
        static int SignOfElement(int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        //this method determines the sub matrix corresponding to a given element
        static double[,] CreateSmallerMatrix(double[,] input, int i, int j)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            double[,] output = new double[order - 1, order - 1];
            int x = 0, y = 0;
            for (int m = 0; m < order; m++, x++)
            {
                if (m != i)
                {
                    y = 0;
                    for (int n = 0; n < order; n++)
                    {
                        if (n != j)
                        {
                            output[x, y] = input[m, n];
                            y++;
                        }
                    }
                }
                else
                {
                    x--;
                }
            }
            return output;
        }
        //this method determines the value of determinant using recursion
        static double Determinant(double[,] input)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            if (order > 2)
            {
                double value = 0;
                for (int j = 0; j < order; j++)
                {
                    double[,] Temp = CreateSmallerMatrix(input, 0, j);
                    value = value + input[0, j] * (SignOfElement(0, j) * Determinant(Temp));
                }
                return value;
            }
            else if (order == 2)
            {
                return ((input[0, 0] * input[1, 1]) - (input[1, 0] * input[0, 1]));
            }
            else
            {
                return (input[0, 0]);
            }
        }
    }

    //function BowyerWatson(pointList)
    //  // pointList is a set of coordinates defining the points to be triangulated
    //  --> triangulation := empty triangle mesh data structure
    //  --> add super-triangle to triangulation // must be large enough to completely contain all the points in pointList
    //  --> for each point in pointList do // add all the points one at a time to the triangulation
    //  -->   badTriangles := empty set
    //  -->   for each triangle in triangulation do // first find all the triangles that are no longer valid due to the insertion
    //        if point is inside circumcircle of triangle
    //           add triangle to badTriangles
    //     polygon := empty set
    //     for each triangle in badTriangles do // find the boundary of the polygonal hole
    //        for each edge in triangle do
    //           if edge is not shared by any other triangles in badTriangles
    //              add edge to polygon
    //     for each triangle in badTriangles do // remove them from the data structure
    //        remove triangle from triangulation
    //     for each edge in polygon do // re-triangulate the polygonal hole
    //        newTri := form a triangle from edge to point
    //        add newTri to triangulation
    //  for each triangle in triangulation // done inserting points, now clean up
    //     if triangle contains a vertex from original super-triangle
    //        remove triangle from triangulation
    //  return triangulation

    

    private void TestSuperTetrahedron(Tetrahedron tetrahedron)
    {
        //foreach(Measurement3D measurement in allMeasuerements)
        //{
        //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    obj.transform.position = measurement.Position;
        //    obj.transform.localScale = Vector3.one * 0.01f;
        //}

        tetrahedron.Render();
    }
}
