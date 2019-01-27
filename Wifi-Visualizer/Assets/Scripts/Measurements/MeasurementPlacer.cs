using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeasurementPlacer : MonoBehaviour {

	//public List<Vector3> Positions { get; private set; }
    public List<MonoMeasurement3D> Measurements { get; private set; }

    private readonly float MIN_DISTANCE = 0.25f;

    private void Start()
    {
        //Positions = new List<Vector3>();
        Measurements = new List<MonoMeasurement3D>();
    }

    public bool Add(Measurement3D measurement)
    {
        RemoveOveridden(measurement);
        AddMono(measurement);
        return true;
    }

    private void RemoveOveridden(Measurement3D measurement)
    {
        List<int> indices = new List<int>();
        for(int i = Measurements.Count - 1; i >= 0; i--)
        {
            if(Vector3.Distance(Measurements[i].Measurement.Position, measurement) < MIN_DISTANCE)
            {
                indices.Add(i);
            }
        }

        foreach(int i in indices) { 
            Destroy(Measurements[i].gameObject);
            Measurements.Remove(Measurements[i]);
        }
    }

    //public bool HasNear(Measurement3D measurement)
    //{
    //    foreach(Vector3 position in Positions)
    //    {
    //        if(Vector3.Distance(position, measurement) < MIN_DISTANCE)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private void AddMono(Measurement3D measurement)
    {
        GameObject obj = new GameObject("Measurement " + Measurements.Count);
        MonoMeasurement3D mono = (obj.AddComponent<MonoMeasurement3D>());
        mono.SetMeasurement(measurement);
        mono.SetSize(MIN_DISTANCE * 2);
        obj.transform.parent = transform;
        Measurements.Add(mono);
    }
}
