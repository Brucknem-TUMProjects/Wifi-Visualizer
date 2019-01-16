using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonoMeasurement3D : MonoBehaviour {

    public Measurement3D Measurement { get; private set; }

    public void Initialize(Measurement3D measurement, float size)
    {
        Measurement = measurement;
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material = new Material(Shader.Find("Custom/Measurement"));
        transform.position = measurement;
        transform.localScale = Vector3.one * size;
        Measurement = measurement;
        rend.material.color = measurement.Color;
        rend.material.SetFloat("_Falloff", measurement.Fallout);
        Texture2D texture = (Texture2D)Resources.Load("Images/FadeOutBillboard");
        Debug.Log(texture);
        rend.material.SetTexture("_MainTex", texture);
    }

    public void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
        //transform.Rotate(transform.up, 180);
    }
}
