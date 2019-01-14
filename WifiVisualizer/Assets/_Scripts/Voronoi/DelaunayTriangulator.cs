using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulator : MonoBehaviour{

    public MonoTetrahedron prefab;
    private IList<Tetrahedron> tetrahedrons;
    private IDelaunayTriangulation triangulation;
    private IDBConnector database;
    private List<Measurement3D> measurements;

    void Start()
    {
        database = new DBConnectorMock();
        database.ConnectDatabase("/Database/database.db");
        CreateMeasurements();
        Initialize();
    }

    public void Initialize()
    {
        triangulation = new DelaunayTriangulationMock();
        triangulation.Generate(measurements);
        GameObject spinner = new GameObject("Spinner");
        //spinner.transform.position = new Vector3(triangulation.Centroid.X, triangulation.Centroid.Y, triangulation.Centroid.Z);
        //this.transform.parent = spinner.transform;
        //spinner.AddComponent<Spin>();
        CreateMonoTetrahedrons();
    }

    private void CreateMeasurements()
    {
        List<Location> locations = database.Select<Location>();
        List<Signal> signals = database.Select<Signal>();
        locations.Sort();
        signals.Sort();
        Queue<Location> locationQueue = new Queue<Location>(locations);
        Queue<Signal> signalQueue = new Queue<Signal>(signals);

        measurements = new List<Measurement3D>();

        while (locationQueue.Count > 0)
        {
            Location location = locationQueue.Dequeue();
            while (signalQueue.Count > 0)
            {
                Signal signal = signalQueue.Dequeue();
                if (location.Timestamp == signal.Timestamp)
                {
                    measurements.Add(new Measurement3D(location, signal));
                    break;
                }
            }
        }
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
