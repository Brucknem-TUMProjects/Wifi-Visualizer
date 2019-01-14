using HullDelaunayVoronoi.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeHull {
    
    public CubeHull(IList<Vertex3> measurements)
    {
        foreach (Vertex3 measurement in measurements)
        {
            //measurement.Position *= 100;
        }
        Measurements = measurements;
        CalculateHull();
        //CreateSuperTetrahedron();
    }

    public Measurement3D[] Hull { get; private set; }
    public IList<Vertex3> Measurements { get; private set; }
    public Tetrahedron SuperTetrahedron { get; private set; }

    private void CalculateHull()
    {
        Hull = new Measurement3D[8];
        float[] x = new float[]
        {
            Measurements.Min(measurement => measurement.Position[0]),
            Measurements.Max(measurement => measurement.Position[0])
        };

        float[] y = new float[]
        {
            Measurements.Min(measurement => measurement.Position[1]),
            Measurements.Max(measurement => measurement.Position[1])
        };

        float[] z = new float[]
        {
            Measurements.Min(measurement => measurement.Position[2]),
            Measurements.Max(measurement => measurement.Position[2])
        };

        Hull[0] = new Measurement3D(new Location(0, x[0], y[0], z[0]), new Signal());
        Hull[1] = new Measurement3D(new Location(0, x[1], y[0], z[0]), new Signal());
        Hull[2] = new Measurement3D(new Location(0, x[1], y[1], z[0]), new Signal());
        Hull[3] = new Measurement3D(new Location(0, x[0], y[1], z[0]), new Signal());
        Hull[4] = new Measurement3D(new Location(0, x[0], y[0], z[1]), new Signal());
        Hull[5] = new Measurement3D(new Location(0, x[1], y[0], z[1]), new Signal());
        Hull[6] = new Measurement3D(new Location(0, x[1], y[1], z[1]), new Signal());
        Hull[7] = new Measurement3D(new Location(0, x[0], y[1], z[1]), new Signal());

        //foreach (Measurement3D corner in Hull)
        //{
        //    if (Measurements.Contains(corner))
        //    {
        //        Measurement3D current = Measurements.Find(measurement => measurement.Equals(corner));
        //        corner.Position = current.Position;
        //        corner.Signal = current.Signal;
        //        Measurements.Remove(current);
        //    }
        //}
    }

    private void CreateSuperTetrahedron()
    {
        Measurement3D[] superTetrahedron = new Measurement3D[4];
        superTetrahedron[0] = Hull[0];
        superTetrahedron[1] = TripleDistance(Hull[0], Hull[1]);
        superTetrahedron[2] = TripleDistance(Hull[0], Hull[3]);
        superTetrahedron[3] = TripleDistance(Hull[0], Hull[4]);
        //SuperTetrahedron = new Tetrahedron(superTetrahedron);
    }

    private Measurement3D TripleDistance(Measurement3D origin, Measurement3D toDouble)
    {
        //Vector3 originPos = new Vector3(origin.Position);
        //Vector3 position = toDouble.Position;
        //Vector3 toAdd = (position - originPos);
        //Vector3 newPosition = originPos + 3 * toAdd;
        //return new Measurement3D(new Location(0, newPosition), new Signal());
        return null;
    }
}
