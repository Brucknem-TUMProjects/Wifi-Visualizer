using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulatorTest : MonoBehaviour {

    float lastUpdate = 0;
	void Start () {
        
    }

    private void Update()
    {
        if(Time.time - lastUpdate > 1)
        {
            lastUpdate = Time.time;
            DelaunayTriangulator.Instance.Add(new Measurement3D(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), "", "", Random.Range(-80, -30), false));
        }
    }

    private void RandomTest()
    {
        List<Measurement3D> measurements = new List<Measurement3D>();
        for (int i = 0; i < 100; i++)
        {
            measurements.Add(new Measurement3D(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), "", "", Random.Range(-80, -30), false));
        }
        DelaunayTriangulator.Instance.Generate(measurements);
    }

    private void CubeTest()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    DelaunayTriangulator.Instance.Add(new Measurement3D(i * 10, j * 10, k * 10, "", "", Random.Range(-80, -30), false));
                }
            }
        }
    }
}
