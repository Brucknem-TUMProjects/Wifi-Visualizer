using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circumsphere
{
    public Vector3 Center { get; set; }
    public double Radius { get; set; }

    public void Render()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "Circumsphere";
        sphere.transform.position = Center;
        ScaleSphere scale = sphere.AddComponent<ScaleSphere>();
        scale.scale = (float)Radius * 2;
        sphere.transform.localScale = 2 * Vector3.one * (float)Radius;
        sphere.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/SphereSurf"));
    }
}