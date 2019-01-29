using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeasurementPlacer : MonoBehaviour {

	//public List<Vector3> Positions { get; private set; }
    private List<MonoMeasurement3D> Measurements { get; set; }
    public MonoMeasurement3D prefab;

    private readonly float MIN_DISTANCE = 0.25f;

    private void Start()
    {
        //Positions = new List<Vector3>();
        Measurements = new List<MonoMeasurement3D>();
    }

    public bool Add(Measurement3D measurement, bool auto=false)
    {
        if (!auto)
        {
            RemoveOveridden(measurement);
        }
        else
        {
            if (HasNear(measurement))
            {
                return false;
            }
        }
        AddMono(measurement);
        return true;
    }

    public bool HasNear(Measurement3D measurement)
    {
        for (int i = Measurements.Count - 1; i >= 0; i--)
        {
            if(Vector3.Distance(measurement, Measurements[i].Measurement) < MIN_DISTANCE)
            {
                return true;
            }
        }
        return false;
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
    
    private void AddMono(Measurement3D measurement)
    {
        MonoMeasurement3D mono = Instantiate(prefab);
        mono.name = "Measurement " + Measurements.Count;
        mono.SetMeasurement(measurement);
        mono.SetSize(MIN_DISTANCE * 2);
        mono.gameObject.transform.parent = transform;
        Measurements.Add(mono);
    }
}
