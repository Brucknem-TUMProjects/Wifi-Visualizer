using System.Collections.Generic;
using UnityEngine;

public class MonoMeasurement3D : MonoBehaviour {

    public Measurement3D Measurement { get; private set; }
    private bool initialized = false;
    private MeshRenderer rend;

    public double decibel;
    public float transparency;

    public void SetMeasurement(Measurement3D measurement)
    {
        if (!initialized)
        {
            Initialize();
        }
        Measurement = measurement;        
        transform.position = measurement;
        rend.material.color = measurement.Color;
        decibel = measurement.Decibel;
        transparency = measurement.Transparency;
        //rend.material.SetFloat("_Falloff", measurement.Falloff);
        rend.material.SetFloat("_Transparency", measurement.Transparency);
    }

    private void Initialize()
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.AddComponent<MeshFilter>().mesh = quad.GetComponent<MeshFilter>().mesh;
        Destroy(quad);

        rend = gameObject.AddComponent<MeshRenderer>();
        rend.material = new Material(Shader.Find("Custom/SphereSurf"));
        initialized = true;
    }
    
    public void SetSize(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }
}
