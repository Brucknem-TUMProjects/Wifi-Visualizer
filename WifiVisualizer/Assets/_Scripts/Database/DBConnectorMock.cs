using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBConnectorMock : IDBConnector
{
    public override void ConnectDatabase(string file)
    {
        base.ConnectDatabase(file);
        Random.InitState(0);
        RandomCloud();
    }

    private void CubeCloud()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Add(new Measurement3D(
                        new Location(i + 10 * j + 100 * k, i * 10, j * 10, k * 10),
                        new Signal(i + 10 * j + 100 * k, "", "", -Random.Range(30,80)
                        )));
                }
            }
        }
    }

    private void RandomCloud()
    {
        for (int i = 0; i < 100; i++)
        {

            Add(new Measurement3D(
                new Location(i, Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)),
                new Signal(i, "", "", -Random.Range(30, 80))
                ));
        }
    }

    public override List<T> Select<T>(long timestamp = -1)
    {
        T value = new T();
        if (value.GetType() == typeof(Location))
        {
            return Locations as List<T>;
        }
        else if (value.GetType() == typeof(Signal))
        {
            return Signals as List<T>;
        }
        return new List<T>();
    }
}
