using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {
    public Shader shader;
    private IDBConnector database;

    public IList<Measurement3D> measurements;
    public CubeHull cubeHull;
    // Use this for initialization
    void Start () {
        database = new DBConnector();
        database.ConnectDatabase("/Database/database.db");

        CreateMeasurements();
        new DelauneyTriangulation(measurements);

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

    // Update is called once per frame
    void Update () {
		
	}
}
