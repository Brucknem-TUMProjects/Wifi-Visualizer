using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehaviour : MonoBehaviour {

    public IMarkerBehaviour marker;

	// Update is called once per frame
	void Update () {
        if (marker.IsTracked)
        {
            Destroy(gameObject);
        }
	}
}
