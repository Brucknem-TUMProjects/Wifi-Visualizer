using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location {

    [PrimaryKey]
    private long timestamp;

    private double posX;
    private double posY;
    private double posZ;

    private double rotX;
    private double rotY;
    private double rotZ;

    public Location()
    {

    }

    public Location(long timestamp, double posX, double posY, double posZ, double rotX, double rotY, double rotZ)
    {

    }
}
