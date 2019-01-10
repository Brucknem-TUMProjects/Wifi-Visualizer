using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScaleSphere : MonoBehaviour {

    [Range(0, 100)]
    public float scale;

    private void OnValidate()
    {
        transform.localScale = Vector3.one * scale;
    }
}
