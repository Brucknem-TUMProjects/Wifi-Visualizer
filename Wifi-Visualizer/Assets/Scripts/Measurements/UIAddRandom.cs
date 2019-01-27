using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddRandom : MonoBehaviour {

    public MeasurementPlacer placer;
    [Range(-80,-20)]
    public int decibel;

    public Vector3 position;
    
    private void Start()
    {
        for(int i = -80; i <= -20; i++)
        {
            placer.Add(new Measurement3D((Vector3.right * i) / 4, "", "", i));
        }
    }

    public void AddRandom()
    {
        placer.Add(new Measurement3D(position, "","", decibel));
    }
}
