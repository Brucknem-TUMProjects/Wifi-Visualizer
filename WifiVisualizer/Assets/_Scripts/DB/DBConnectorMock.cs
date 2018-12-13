using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBConnectorMock : IDBConnector
{    
    public override List<T> Select<T>(long timestamp = -1)
    {
        T value = new T();
        if (value.GetType() == typeof(Location))
        {
            return locations as List<T>;
        }
        else if (value.GetType() == typeof(Signal))
        {
            return signals as List<T>;
        }
        return new List<T>();
    }
}
