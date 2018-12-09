using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class DatabaseConnector
{
    private static DatabaseConnector instance;
    private static IDbConnection dbconn;

    public bool isConnected = false;
    public string file;

    private DatabaseConnector()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public static DatabaseConnector Instance
    {
        get
        {
            if (instance == null)
                instance = new DatabaseConnector();
            return instance;
        }
    }

    public void ConnectDatabase(String file)
    {
        string conn = "URI=file:" + Application.dataPath + file; //Path to database.
        Debug.Log(conn);
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        Debug.Log("Connected to database");
    }

    public void InsertInto(string table, params object[] values)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO " + table + " VALUES (";
        foreach (object value in values)
        {
            sqlQuery += value + ", ";
        }
        sqlQuery = sqlQuery.Substring(0, sqlQuery.Length - 2);
        sqlQuery += ");";
        Debug.Log(sqlQuery);
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
    }

    public T SelectFrom<T>(string select, string table, string where, T value)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT " + select + " FROM " + table + " WHERE " + where;
        Debug.Log(sqlQuery);
        FieldInfo[] fields = value.GetType().GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance);
        foreach (FieldInfo info in fields)
        {
            Debug.Log(info.ToString() + " - " + info.GetType().ToString() + " - " + info.Attributes);
        }
        try
        {
            dbcmd.CommandText = sqlQuery;
            IDataReader dbr = dbcmd.ExecuteReader();

            Debug.Log("Executed");

            while (dbr.Read())
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    Debug.Log(dbr.GetData(i));
                }
            }
            EditorApplication.isPaused = true;
        }
        catch (Exception e)
        {
            Debug.Log("Error while executing");
            return value;
        }
        return value;
    }

    public void CloseConnection()
    {
        dbconn.Close();
        dbconn = null;
        Debug.Log("Closed database connection");
    }
}
