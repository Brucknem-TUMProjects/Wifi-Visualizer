using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform toFollow;
    	
	// Update is called once per frame
	void FixedUpdate () {
        float t = 2 * Time.fixedDeltaTime;
        transform.position = Vector3.Lerp(transform.position, toFollow.position, t);
    }
}
