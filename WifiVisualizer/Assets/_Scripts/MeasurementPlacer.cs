using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {
    private readonly DBConnector database = new DBConnector();
    public HaloScript[] measurements;

	// Use this for initialization
	void Start () {
        database.ConnectDatabase("/Database/database.db");

        List<Location> locations = database.QueryLocations();
        List<Signal> signals = database.QuerySignals();

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
