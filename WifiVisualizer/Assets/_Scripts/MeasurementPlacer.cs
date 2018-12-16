using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {
    private readonly IDBConnector<DBConnector> database = DBConnector.Instance;
    public HaloScript[] measurements;

	// Use this for initialization
	void Start () {
        database.ConnectDatabase("/Database/database.db");

        List<Location> locations = database.Select<Location>();
        List<Signal> signals = database.Select<Signal>();

        Debug.Log(locations + "" +signals);

        for(int i = 0; i < locations.Count; i++)
        {
            Location current = locations[i];
            measurements[i].gameObject.SetActive(true);
            measurements[i].transform.position = new Vector3(current.PosX, current.PosY, current.PosZ);
            measurements[i].transform.rotation = Quaternion.Euler(current.RotX, current.RotY, current.RotZ);
            measurements[i].SetColor(signals[i].Decibel);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
