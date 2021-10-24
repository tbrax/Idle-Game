using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Criteria
{
    public string check;
    public string compareType;
    public string against;
    [System.NonSerialized]
    Effect effect;
    public Criteria(Effect e)
    {
        effect = e;
    }


    public void setupCriteria(Effect e)
    {
        effect = e;

        if (check == null)
        {
            check = "1";
        }
        if (compareType == null)
        {
            compareType = "==";
        }
        if (against == null)
        {
            against = "1";
        }
    }

    public Criteria(string ch, string co, string ag)
    {
        check = ch;
        compareType = co;
        against = ag;
    }

    public Criteria deepCopy()
    {
        Criteria cr = new Criteria(check, compareType, against);
        return cr;
    }

    public string getDesc()
    {
        return check + compareType + against;
    }

    public float parseNum(Character tar, Character own, string num)
    {
        return effect.parseNum(tar, own, num);
    }

    public bool isValid(Character tar, Character own)
    {
        float checkf = parseNum(tar,own,check);
        float checka = parseNum(tar, own, against);

        if (compareType == "==")
        {
            
            return (checkf == checka);
        }

        else if (compareType == "!=")
        {
            return (checkf != checka);
        }

        else if (compareType == "<")
        {
            return (checkf < checka);
        }

        else if (compareType == "<=")
        {
            return (checkf <= checka);
        }

        else if (compareType == ">")
        {
            return (checkf > checka);
        }

        else if (compareType == ">=")
        {
            return (checkf >= checka);
        }


        return true;
    }
}
