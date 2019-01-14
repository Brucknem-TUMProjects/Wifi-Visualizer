using System.Collections;
using System.Collections.Generic;
using HullDelaunayVoronoi.Delaunay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelaunayTriangulator : MonoBehaviour{

    public MonoTetrahedron prefab;
    private IDelaunayTriangulation triangulation;
    public IDBConnector database;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "EditorView")
        {
            database = new DBConnector();
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
        CreateMonoTetrahedrons();
    }

    private void CreateMonoTetrahedrons()
    {
        foreach(Tetrahedron tetrahedron in triangulation.Triangulation)
        {
            MonoTetrahedron monoTetrahedron = Instantiate(prefab);
            monoTetrahedron.Initialize(tetrahedron);
            monoTetrahedron.transform.parent = transform;
            monoTetrahedron.Render();
        }
    }
}
