using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class SQLable
{
    [PrimaryKey]
    public long Timestamp { get; set; }

    public SQLable()
    {
        Timestamp = 0;
    }

    public string ToSqlValueList()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance |
                         BindingFlags.DeclaredOnly);
        string valueList = "(" + Timestamp;

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(System.String))
            {
                valueList += ", '" + field.GetValue(this) + "'";
            }
            else
            {
                valueList += ", " + field.GetValue(this);
            }
        }

        return valueList + ")";
    }
}
