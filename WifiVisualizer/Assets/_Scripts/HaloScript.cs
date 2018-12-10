using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HaloScript : MonoBehaviour
{
    Light halo;

    private void Start()
    {
        halo = GetComponent<Light>();
    }

    private void Awake()
    {
        Start();
    }

    public void SetColor(int decibel)
    {

        float value = Mathf.Clamp(decibel, -80f, -30f);
        value += 30;
        value *= -1;

        float r = 0f;
        float g = 255f;

        if (value <= 25)
        {
            r = value / 25f * 255f;
        }
        else
        {
            r = 255f;
            g = 255f - ((value / 25f) * 255f);
        }
        halo.color = new Color(r, g, 0);
    }
}