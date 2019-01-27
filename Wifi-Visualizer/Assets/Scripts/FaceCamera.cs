using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {


    // Update is called once per frame
    void FixedUpdate()
    {
        float t = 2 * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, t);
    }
}
