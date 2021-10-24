using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CardCost
{
    // Start is called before the first frame update

    //List<Effect> effects = new List<Effect>();
    //public List<Dictionary<string, string>> cost = new List<Dictionary<string, string>>();

    public List<DictionaryOfStringAndString> cost = new List<DictionaryOfStringAndString>();


    
    public CardCost()
    {
        testCost();
    }

    public void testCost()
    {
        DictionaryOfStringAndString d = new DictionaryOfStringAndString();
        //d.Add("red", "1");
        //d.Add("green", "1");
        d.Add("blue", "1");
        cost.Add(d);
    }


    public void addCostDict(DictionaryOfStringAndString d)
    {
        cost.Add(d);
    }

    public void addCostSingle(string type, string amt)
    {
        DictionaryOfStringAndString d = new DictionaryOfStringAndString();
        d.Add(type, amt);
        cost.Add(d);
    }

    public CardCost deepCopy()
    {
        CardCost c = new CardCost();

        foreach (DictionaryOfStringAndString cos in cost)
        {
            DictionaryOfStringAndString newD = new DictionaryOfStringAndString();

            foreach (KeyValuePair<string, string> attach in cos)
            {
                newD.Add(attach.Key, attach.Value);
            }
            c.addCostDict(newD);
        }
            return c;
    }

    public List<DictionaryOfStringAndString> getCost()
    {
        return cost;
    }

    public void takeCost(Character c, int choice)
    {

    }

    public bool canAfford(Character c, Card ch, int choice)
    {
        return true;
    }

    // Update is called once per frame

}
