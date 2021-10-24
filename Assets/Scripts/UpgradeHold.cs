using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
[Serializable]
public class UpgradeHold
{
    public List<Upgrade> uList = new List<Upgrade>();


    [System.NonSerialized]
    BuildingHold bHold;
    [System.NonSerialized]
    public List<Sprite> upgradeIconList = new List<Sprite>();

    public UpgradeHold(List<Sprite> s)
    {
        setIconList(s);
    }

    public Sprite getIcon(int i)
    {
        try
        {
            return upgradeIconList[i];
        }
        catch
        {
            Debug.LogError("Could not find upgrade icon #" + i.ToString());
            return null;
        }

    }

    public void setIconList(List<Sprite> s)
    {
        upgradeIconList = s;
    }

    public Upgrade getUpgrade(int idx)
    {
        return uList[idx];
    }


    public int getCount()
    {
        return uList.Count;
    }

    public BigInteger addUpgrades(BigInteger amt, Building bd)
    {
        BigInteger temp = amt;
        for (int i = 0; i < uList.Count; i++)
        {
            temp = uList[i].addUpgrade(temp,bd,bHold,this);
        }

            return temp;
    }


    public void setBuildingHold(BuildingHold bh)
    {
        bHold = bh;
    }


    public void makeUpgrades(ConstantData c)
    {

        List<string> names = c.getData("upgrade name");
        List<string> desc = c.getData("upgrade desc");
        List<string> cost = c.getData("upgrade cost");

        for (int i = 0; i < names.Count; i++)
        {
            //BigInteger b0 = BigInteger.Parse(upgradePrices[i]);

            uList.Add(new Upgrade(i,names[i], desc[i], cost[i]));
        }

    }

}
