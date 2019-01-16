using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class DelaunayTriangulator : MonoBehaviour
{
    private IDelaunayTriangulation triangulation;

    static DelaunayTriangulator mInstance;

    public static DelaunayTriangulator Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<DelaunayTriangulator>();
                mInstance.Reset();
            }
            return mInstance;
        }
    }

    public void Reset()
    {
        triangulation = new DelaunayTriangulation();
    }

    public void Add(Measurement3D measurement)
    {
        triangulation.Add(measurement);
        Render();
    }

    public void AddAll(List<Measurement3D> measurements)
    {
        triangulation.AddAll(measurements);
        Render();
    }

    public void Generate(List<Measurement3D> measurements)
    {
        triangulation.Generate(measurements);
        Render();
    }

    private void Render()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log(transform.childCount);
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (Tetrahedron tetrahedron in triangulation.Triangulation)
        {
            GameObject obj = new GameObject("Tetrahedron");
            obj.AddComponent<MonoTetrahedron>().Initialize(tetrahedron);
            obj.transform.parent = transform;
        }

        float size = triangulation.AverageDistance * 0.1f;
        foreach (Measurement3D measurement in triangulation.Measurements)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.AddComponent<MonoMeasurement3D>().Initialize(measurement,size);
            obj.transform.parent = transform;
        }
    }

    
}
