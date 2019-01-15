using HullDelaunayVoronoi.Delaunay;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelaunayTriangulation : IDelaunayTriangulation
{
    public DelaunayTriangulation() : base() { }

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
        DelaunayTriangulation3<Measurement3D> triangulation = new DelaunayTriangulation3<Measurement3D>();
        triangulation.Generate(measurements);
        foreach (DelaunayCell<Measurement3D> cell in triangulation.Cells)
        {
            Triangulation.Add(new Tetrahedron(cell.Simplex.Vertices));
        }
    }

    public void BowyerWatson()
    {
        //triangulation:= empty triangle mesh data structure
        //add super-triangle to triangulation // must be large enough to completely contain all the points in pointList
        Triangulation.Add(SuperTetrahedron);
        //for each point in pointList do // add all the points one at a time to the triangulation
        foreach (Measurement3D measurement in Measurements)
        {
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
                foreach (Triangle edge in tetrahedron.Edges)
                {
                    //                  if edge is not shared by any other triangles in badTriangles
                    if (!EdgeInBadTriangles(badTetrahedrons.Where(t => !t.Equals(tetrahedron)), edge))
                    {
                        //                     add edge to polygon
                        polygon.Add(edge);
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
                Triangulation.Add(new Tetrahedron(
                        measurement,
                        triangle.Corners[0],
                        triangle.Corners[1],
                        triangle.Corners[2]));
            }
        }
        //for each triangle in triangulation // done inserting points, now clean up
        //   if triangle contains a vertex from original super-triangle
        //      remove triangle from triangulation
        //return triangulation

    }

    public bool EdgeInBadTriangles(IEnumerable<Tetrahedron> tetrahedrons, Triangle edge)
    {
        //                  if edge is not shared by any other triangles in badTriangles
        foreach (Tetrahedron otherTetrahedron in tetrahedrons)
        {
            if (otherTetrahedron.Edges.Contains(edge))
            {
                return true;
            }
        }
        return false;
    }

    public Tetrahedron SuperTetrahedron
    {
        get
        {
            return new Tetrahedron(
                new Measurement3D(new Location(0, 0, 0, -10)),
                new Measurement3D(new Location(0, -100, -100, 100)),
                new Measurement3D(new Location(0, 100, -100, 100)),
                new Measurement3D(new Location(0, 0, 100, 100))
                );
        }
    }
}