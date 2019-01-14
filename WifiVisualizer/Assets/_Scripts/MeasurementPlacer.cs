using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {
    public Shader shader;
    private IDBConnector database;

    public IList<Vertex3> measurements;
    public CubeHull cubeHull;
    private DelaunayTriangulation3 triangulation;

    // Use this for initialization
    void Start () {
        database = new DBConnector();
        database.ConnectDatabase("/Database/database.db");

        CreateMeasurements();
        cubeHull = new CubeHull(measurements);
        foreach(Measurement3D measurement in cubeHull.Hull)
        {
            measurements.Add(measurement);
        }
        triangulation = new DelaunayTriangulation3();
        triangulation.Generate(measurements);

        int i = 0;
        foreach(DelaunayCell<Vertex3> cell in triangulation.Cells)
        {
            GameObject renderCell = new GameObject("Cell " + i++);
            Vector3[] vertices = new Vector3[4];
            for(int j = 0; j < cell.Simplex.Vertices.Length; j++)
            {
                vertices[j] = new Vector3(cell.Simplex.Vertices[j].X, cell.Simplex.Vertices[j].Y, cell.Simplex.Vertices[j].Z);
            }

            Mesh mesh = new Mesh()
            {
                vertices = vertices,
                triangles = new int[]{0,1,2,0,1,3,1,2,3,0,2,3}
            };

            renderCell.AddComponent<MeshFilter>().mesh = mesh;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            renderCell.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }

        //foreach(Measurement3D measurement in hull)
        //{
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.transform.position = measurement.Position;
        //    cube.transform.localScale = Vector3.one * 0.01f;
        //}


        //for(int i = 0; i < locations.Count; i++)
        //{
        //    Location current = locations[i];
        //    measurements[i].gameObject.SetActive(true);
        //    measurements[i].transform.position = new Vector3(current.PosX, current.PosY, current.PosZ);
        //    measurements[i].transform.rotation = Quaternion.Euler(current.RotX, current.RotY, current.RotZ);
        //    measurements[i].SetShader(shader);
        //    measurements[i].SetColor(signals[i].Decibel);
        //}
    }

    private void CreateMeasurements()
    {
        List<Location> locations = database.Select<Location>();
        List<Signal> signals = database.Select<Signal>();
        locations.Sort();
        signals.Sort();
        Queue<Location> locationQueue = new Queue<Location>(locations);
        Queue<Signal> signalQueue = new Queue<Signal>(signals);

        measurements = new List<Vertex3>();

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

    // Update is called once per frame
    void Update () {
		
	}
}
