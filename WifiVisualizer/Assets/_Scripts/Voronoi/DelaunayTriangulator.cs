using System.Collections;
using System.Collections.Generic;
using HullDelaunayVoronoi.Delaunay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelaunayTriangulator : MonoBehaviour{

    public MonoTetrahedron tetrahedronPrefab;
    public MonoMeasurement3D measurementPrefab;
    private IDelaunayTriangulation triangulation;
    private IDBConnector database;

    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Capture")
        {
            Debug.Log("Start");
            database = new DBConnectorMock();
            database.ConnectDatabase("/Database/database.db");
            Initialize(database);
        }
    }

    private void SetDatabase(IDBConnector dBConnector)
    {
        database = dBConnector;
    }

    public void Initialize(IDBConnector dBConnector)
    {
        SetDatabase(dBConnector);
        Recalculate();
    }

    public void Recalculate()
    {
        Debug.Log(database.Measurements.Count);
        triangulation = new DelaunayTriangulation();
        triangulation.Generate(database.Measurements);
        //GameObject spinner = new GameObject("Spinner");
        //spinner.transform.position = new Vector3(triangulation.Centroid.x, triangulation.Centroid.y, triangulation.Centroid.z);
        //transform.parent = spinner.transform;
        //spinner.AddComponent<Spin>();
        Render();
    }

    private void Render()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0));
        }

        Debug.Log("Creating tetras");
        foreach(Tetrahedron tetrahedron in triangulation.Triangulation)
        {
            MonoTetrahedron monoTetrahedron = Instantiate(tetrahedronPrefab);
            monoTetrahedron.Initialize(tetrahedron);
            monoTetrahedron.transform.parent = transform;
        }

        foreach (Measurement3D vertex in triangulation.Measurements)
        {
            MonoMeasurement3D obj = Instantiate(measurementPrefab, vertex.Location, Quaternion.identity, transform);
            obj.transform.localScale = Vector3.one * 0.5f;
            obj.Initialize(vertex);
        }
    }
}
