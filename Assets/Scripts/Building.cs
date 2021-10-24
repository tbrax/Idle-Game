using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using CustomHelpNameSpace;

using System;
[Serializable]
public class Building
{
    string name;
    BigInteger baseCost;
    int id;
    ulong amt;
    ulong free;
    BigInteger passiveBase;
    float mult;
    string desc;


    [System.NonSerialized]
    BuildingHold bHold;
    [System.NonSerialized]
    UpgradeHold uHold;

    public Building(int nid,
                    string nname,
                    string ndesc,
                    string nbaseCost,
                    string npassiveBase,
                    BuildingHold bh,
                    UpgradeHold uh)
    {
        name = nname;
        desc = ndesc;
        baseCost = BigInteger.Parse(nbaseCost);
        id = nid;
        passiveBase = BigInteger.Parse(npassiveBase);
        mult = 1.0f;
        uHold = uh;
        bHold = bh;
        free = 0;
    }

    public void setBHold(BuildingHold b)
    {
        bHold = b;
    }
    public void setUHold(UpgradeHold u)
    {
        uHold = u;
    }

    public Sprite getIcon()
    {
        return bHold.getIcon(id);
    }

    public void addMult(float f)
    {
        mult *= f;
    }


    public int getId()
    {
        return id;
    }

    public string getDesc()
    {
        return desc;
    }

    public string getName()
    {
        return name;
    }

    public BigInteger getBaseCost()
    {

        return baseCost;
    }

    public BigInteger getNextCostAmt(int x)
    {
        BigInteger g = 0;
        for (ulong i = 0; i < (ulong)x; i++)
        {
            g += powerAdd(amt+i);
        }
        return g;
    }


    BigInteger powerAdd(ulong add)
    {
        double l = (Mathf.Pow(1.15f, add));
        BigInteger g = CustomHelp.bigIntegerMult(getBaseCost(), l);
        return g;
    }

    public BigInteger getNextCost()
    {
        return powerAdd(amt);
    }

    public ulong getAmt()
    {
        return amt + free;
    }

    void addAmt(ulong a)
    {
        amt += a;
    }

    public void addOne()
    {
        addAmt(1);
    }

    public BigInteger addUpgrades(BigInteger p)
    {
        return uHold.addUpgrades(p, this);
    }

    public BigInteger gainUnit()
    {
        return addUpgrades(passiveBase);
    }

    public BigInteger getPassive()
    {
        BigInteger p = (amt + free) * gainUnit();
        return p;
    }

}
