using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMock : IMarkerBehaviour
{
    public override bool IsTracked
    {
        get
        {
            return true;
        }
    }

    public override Vector3 Position
    {
        get
        {
            return Camera.main.transform.position + Camera.main.transform.forward.normalized;
        }
    }
}
