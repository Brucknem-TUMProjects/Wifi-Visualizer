using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWifiInfo<T> where T : new()
{
    private static T instance;
    private static readonly object padlock = new object();
    
    public static T Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }

    public abstract int GetDecibel();
    public abstract string GetSSID();
    public abstract string GetMAC();
    public abstract string GetIP();

    public int GetPort()
    {
        return 12345;
    }
}
