using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoMeasurement3D : MonoBehaviour {

    public Measurement3D Measurement { get; private set; }
    public Shader shader;
    public Texture texture;
    private MeshRenderer rend;

	// Use this for initialization
	void Awake () {
        rend = GetComponent<MeshRenderer>();
        rend.material = new Material(shader);
	}
	
	public void Initialize(Measurement3D measurement)
    {
        Measurement = measurement;
        rend.material.color = measurement.Color;
        rend.material.SetFloat("_Falloff", measurement.Fallout);
        rend.material.SetTexture("_MainTex", texture);
    }

    public void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(transform.up, 180);
    }
}
