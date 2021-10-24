using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
[Serializable]
public class BuildingHold
{
    public List<Building> bList = new List<Building>();

    [System.NonSerialized]
    UpgradeHold uHold;
    [System.NonSerialized]
    public List<Sprite> buildingIconList = new List<Sprite>();

    public BuildingHold(List<Sprite> s)
    {
        setIconList(s);
    }

    public Sprite getIcon(int i)
    {
        try
        {
            return buildingIconList[i];
        }
        catch
        {
            Debug.LogError("Could not find building icon #" + i.ToString());
            return null;
        }

    }


    public void setIconList(List<Sprite> s)
    {
        buildingIconList = s;
    }


    public void setUpgradeHold(UpgradeHold u)
    {
        uHold = u;
        uHold.setBuildingHold(this);


        for (int i = 0; i < bList.Count; i++)
        {
            bList[i].setBHold(this);
            bList[i].setUHold(u);
        }


    }


    public Building getBuilding(int idx)
    {
        return bList[idx];
    }

    public int getCount()
    {
        return bList.Count;
    }


    public void makeBuildings(  ConstantData c)
    {
        List<string> names = c.getData("building name");
        List<string> cost = c.getData("building base cost");
        List<string> passive = c.getData("building base passive");
        List<string> descs = c.getData("building desc");
        for (int i = 0; i < names.Count; i++)
        {
            bList.Add(new Building( i,
                                    names[i],
                                    descs[i],
                                    cost[i],
                                    passive[i], 
                                    this,
                                    uHold));
        }
    }


}
