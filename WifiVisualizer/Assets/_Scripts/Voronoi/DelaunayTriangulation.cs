using HullDelaunayVoronoi.Delaunay;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelaunayTriangulation : IDelaunayTriangulation
{
    public DelaunayTriangulation() : base() { }
    
    public override void Add(Measurement3D measurement)
    {
        Measurements.Add(measurement);
        UpdateExtremes(measurement);

        //      badTriangles:= empty set
        List<Tetrahedron> badTetrahedrons = new List<Tetrahedron>();
        //   for each triangle in triangulation do // first find all the triangles that are no longer valid due to the insertion
        foreach (Tetrahedron tetrahedron in Triangulation)
        {
            //          if point is inside circumcircle of triangle
            if (tetrahedron.Includes(measurement))
            {
                //         add triangle to badTriangles
                badTetrahedrons.Add(tetrahedron);
            }
        }
        //   polygon := empty set
        List<Triangle> polygon = new List<Triangle>();
        //   for each triangle in badTriangles do // find the boundary of the polygonal hole
        foreach (Tetrahedron tetrahedron in badTetrahedrons)
        {
            //          for each edge in triangle do
            foreach (Triangle triangle in tetrahedron.Triangles)
            {
                //                  if edge is not shared by any other triangles in badTriangles
                if (!EdgeInBadTriangles(badTetrahedrons.Where(t => !t.Equals(tetrahedron)), triangle))
                {
                    //                     add edge to polygon
                    polygon.Add(triangle);
                }
            }
        }
        //   for each triangle in badTriangles do // remove them from the data structure
        foreach (Tetrahedron tetrahedron in badTetrahedrons)
        {
            //          remove triangle from triangulation
            Triangulation.Remove(tetrahedron);
        }
        //   for each edge in polygon do // re-triangulate the polygonal hole
        foreach (Triangle triangle in polygon)
        {
            //      newTri:= form a triangle from edge to point
            //     add newTri to triangulation
            if (triangle.InSamePlane(measurement))
            {
                Debug.Log("In same plane");
                continue;
            }
            Triangulation.Add(
                new Tetrahedron(
                    measurement,
                    triangle.Measurements[0],
                    triangle.Measurements[1],
                    triangle.Measurements[2]));
        }
    }

    protected override void UpdateExtremes(Measurement3D measurement)
    {
        float[] pos = measurement.PositionArray;

        for (int i = 0; i < Extremes.GetLength(0); i++)
        {
            if (pos[i] < Extremes[i, 0])
            {
                Extremes[i, 0] = pos[i];
            }
            if (pos[i] > Extremes[i, 1])
            {
                Extremes[i, 1] = pos[i];
            }
        }
    }

    public override void Generate(List<Measurement3D> measurements)
    {
        //triangulation:= empty triangle mesh data structure
        //add super-triangle to triangulation // must be large enough to completely contain all the points in pointList
        Init();
        //for each point in pointList do // add all the points one at a time to the triangulation
        AddAll(measurements);
        //for each triangle in triangulation // done inserting points, now clean up
        //foreach (Tetrahedron tetrahedron in Triangulation)
        //{
        //    //   if triangle contains a vertex from original super-triangle
        //    if (tetrahedron.IsArtificial)
        //    {
        //        //      remove triangle from triangulation
        //        Triangulation.Remove(tetrahedron);
        //    }
        //}
        //return triangulation
    }

    public bool EdgeInBadTriangles(IEnumerable<Tetrahedron> tetrahedrons, Triangle triangle)
    {
        //                  if edge is not shared by any other triangles in badTriangles
        foreach (Tetrahedron otherTetrahedron in tetrahedrons)
        {
            foreach(Triangle otherTriangle in otherTetrahedron.Triangles)
            {
                if (otherTriangle.Equals(triangle))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void AddAll(List<Measurement3D> measurements)
    {
        foreach (Measurement3D measurement in measurements)
        {
            Add(measurement);
        }
    }
}