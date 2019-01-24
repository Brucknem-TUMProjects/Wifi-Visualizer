using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {

	public List<Vector3> Positions { get; private set; }
    public List<MonoMeasurement3D> Monos { get; private set; }

    private readonly float MIN_DISTANCE = 0.25f;

    private void Start()
    {
        Positions = new List<Vector3>();
        Monos = new List<MonoMeasurement3D>();
    }

    public bool Add(Measurement3D measurement)
    {
        if (HasNear(measurement) )
        {
            return false;
        }
        Positions.Add(measurement);
        AddMono(measurement);
        return true;
    }

    public bool HasNear(Measurement3D measurement)
    {
        foreach(Vector3 position in Positions)
        {
            if(Vector3.Distance(position, measurement) < MIN_DISTANCE)
            {
                return true;
            }
        }
        return false;
    }

    private void AddMono(Measurement3D measurement)
    {
        GameObject obj = new GameObject("Measurement " + Monos.Count);
        MonoMeasurement3D mono = (obj.AddComponent<MonoMeasurement3D>());
        mono.SetMeasurement(measurement);
        mono.SetSize(MIN_DISTANCE * 2);
        obj.transform.parent = transform;
        Monos.Add(mono);
    }
}
