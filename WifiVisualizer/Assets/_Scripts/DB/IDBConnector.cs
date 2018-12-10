using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDBConnector {

    public abstract void ConnectDatabase(string file);
    public abstract void AddLocation(Location location);
    public abstract List<Location> QueryLocations(long timestamp = -1);
    public abstract void CloseConnection();
    public abstract void AddSignal(Signal location);
    public abstract void AddSignals(List<Signal> signals);
    public abstract List<Signal> QuerySignals(long timestamp = -1);
    public abstract void ClearTables();
}
