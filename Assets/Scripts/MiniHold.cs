using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class MiniHold
{
    [System.NonSerialized]
    public List<Sprite> miniIconList = new List<Sprite>();
    List<string> names;

    public MiniHold(List<Sprite> s)
    {
        setIconList(s);
    }

    public void setIconList(List<Sprite> s)
    {
        miniIconList = s;
    }

    public Sprite getIcon(int i)
    {
        try
        {
            return miniIconList[i];
        }
        catch
        {
            Debug.LogError("Could not find mini icon #" + i.ToString());
            return null;
        }

    }

    public string getMinigameName(int idx)
    {
        return names[idx];
    }


    public int getCount()
    {
        return 2;
    }

    public void makeMinigames(ConstantData c)
    {
        names = c.getData("minigame name");
    }

}
