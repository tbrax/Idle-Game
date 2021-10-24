using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridSquare
{
    string name;
    int collide;
    Transform loc;


    public Vector3 getPos()
    {
        return loc.position;
    }

    public GameGridSquare(string n, int c, Transform t)
    {
        name = n;
        collide = c;
        loc = t;
    }

    public string getName()
    {
        return name;
    }

    public void setTexture()
    {
        
    }


}
