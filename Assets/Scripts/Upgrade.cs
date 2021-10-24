using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;


using System;
[Serializable]
public class Upgrade
{
    string name;
    BigInteger baseCost;
    int id;
    int bought;
    string desc;

    public Upgrade(int nid, string nname, string ndesc, string nbaseCost)
    {
        name = nname;
        desc = ndesc;
        baseCost = BigInteger.Parse(nbaseCost);
        id = nid;
        bought = 0;
    }

    public int getId()
    {
        return id;
    }
    public string getName()
    {
        return name;
    }

    public string getDesc()
    {
        return desc;
    }



    public BigInteger addUpgrade(BigInteger amt, Building bd, BuildingHold bh, UpgradeHold uh)
    {
        BigInteger btemp = amt;
        if (bought == 1)
        {

            switch (id)
            {
                case 0:
                    if (bd.getId() == 0)
                    {
                        btemp = btemp * 2;
                    }
                    break;
                case 1:
                    if (bd.getId() == 0)
                    {
                        btemp = btemp * 2;
                    }
                    break;

                default:
                    btemp = amt;
                    break;
            }
        }



        return btemp;
    }

    public BigInteger getCost()
    {
        return baseCost;
    }

    public void buy()
    {
        bought = 1;
    }

    public void reset()
    {
        bought = 0;
    }

    public int getBought()
    {
        return bought;
    }

    public bool isBought()
    {
        return (bought == 1);
    }

}
