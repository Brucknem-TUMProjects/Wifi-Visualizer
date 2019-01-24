using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddRandom : MonoBehaviour {

    public MeasurementPlacer placer;
    [Range(-80,-20)]
    public int decibel;

    private int i = 0;

	public void AddRandom()
    {
        placer.Add(new Measurement3D(Vector3.one * i++, "","", decibel));
    }
}
