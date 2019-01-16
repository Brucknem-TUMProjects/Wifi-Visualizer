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
    public float[,] Extremes { get; protected set; }
    
    public IDelaunayTriangulation()
    {
        Init();
    }
    
    private Tetrahedron SuperTetrahedron
    {
        get
        {
            return new Tetrahedron(
                new Measurement3D(0f, 0f, -100f, true),
                new Measurement3D(-1000, -1000, 1000, true),
                new Measurement3D(1000, -1000, 1000, true),
                new Measurement3D(0, 1000, 1000, true)
                );
        }
    }

    protected void Init()
    {
        Measurements = new List<Measurement3D>();
        InitTriangulation();
        InitExtremes();
    }

    private void InitTriangulation()
    {
        Triangulation = new List<Tetrahedron>
        {
            SuperTetrahedron
        };
    }

    private void InitExtremes()
    {
        Extremes = new float[3, 2];
        for (int i = 0; i < Extremes.GetLength(0); i++)
        {
            Extremes[i, 0] = float.MaxValue;
            Extremes[i, 1] = float.MinValue;
        }
    }

    public abstract void Generate(List<Measurement3D> measurements);
    public abstract void Add(Measurement3D measurement);
    public abstract void AddAll(List<Measurement3D> measurement);
    protected abstract void UpdateExtremes(Measurement3D measurement);

    public float AverageDistance
    {
        get
        {
            Vector3 all = Vector3.one;
            Measurements.ForEach(m => all += m);
            return (all / Measurements.Count).magnitude;
        }
    }
}
