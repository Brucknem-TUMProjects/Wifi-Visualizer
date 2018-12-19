using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DBConnector : IDBConnector<DBConnector>
{
    private SQLiteConnection dbconn;
            
    public override void ConnectDatabase(string file)
    {
        base.ConnectDatabase(file);
        dbconn = new SQLiteConnection(Application.dataPath + file);
    } 
    
    private void Write<T>(T value) where T : SQLable, new()
    {
        Write(new List<T>() { value });
    }

    private void Write<T>(List<T> values) where T : SQLable, new()
    {
        if (values.Count <= 0)
        {
            return;
        }

        dbconn.CreateTable<T>();
        string query = "INSERT INTO " + values[0].GetType().ToString() + " VALUES\n";

        foreach (T value in values)
        {
            query += value.ToSqlValueList() + ",\n";
        }

        query = query.Substring(0, query.Length - 2);
        Debug.Log(query);
        dbconn.Query<T>(query);
    }

    public override List<T> Select<T>(long timestamp = -1)
    {
        string name = typeof(T).ToString();
        
        string query = "SELECT * FROM " + name;

        if (timestamp != -1)
        {
            query += " WHERE timestamp = " + timestamp;
        }
        List<T> result = dbconn.Query<T>(query);
        Debug.Log(result.Count + " " + name.ToUpper() + "S returned");
        return result;
    }

    public override void ClearTables()
    {
        base.ClearTables();
        dbconn.DropTable<Location>();
        dbconn.DropTable<Signal>();
    }

    public override void CloseConnection()
    {
        base.CloseConnection();
        Write(locations);
        Write(signals);

        dbconn.Close();
    }
}