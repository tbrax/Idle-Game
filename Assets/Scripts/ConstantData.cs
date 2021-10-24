using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstantData
{
    List<List<string>> descs = new List<List<string>>();


    public ConstantData()
    {
        makeData();
    }


    string[] buildingNames = new string[] { "Building A",
                                            "Building B",
                                            "Building C",
                                            "Building D" };

    string[] buildingDescs = new string[] { "Building Tier 1",
                                            "Building Tier 2",
                                            "Building Tier 3",
                                            "Building Tier 4" };

    string[] buildingBaseCost = new string[] {  "1",
                                                "10",
                                                "100",
                                                "1000" };

    string[] buildingBasePassive = new string[] {   "100",
                                                    "1000",
                                                    "10000",
                                                    "10000" };




    string[] upgradeNames = new string[] {  "upgrade 1",
                                            "upgrade 2",
                                            "upgrade 3",
                                            "upgrade 4" };

    string[] upgradeDescs = new string[] {  "Doubles rate of tier 1 buildings",
                                            "Doubles rate of tier 1 buildings",
                                            "Does nothing",
                                            "Does nothing" };

    string[] upgradeCosts = new string[] {  "1",
                                            "2",
                                            "3",
                                            "4" };


    string[] minigameNames = new string[] {  "Super Sudoku",
                                            "Battle",
                                            "3",
                                            "4" };

    string[] battleID = new string[] {  "lion0",
                                        "lion1",
                                        "oilooze0"
                                    };
    string[] battleName = new string[] {  "Hunter Lion",
                                        "Alpha Lion",
                                        "Oil Ooze",
                                        };


    string[] buildingDataNames = new string[] { "building name",
                                                "building desc",
                                                "building base cost",
                                                "building base passive",
                                                "upgrade name",
                                                "upgrade desc",
                                                "upgrade cost",
                                                "minigame name",
                                                "battlecharacternames",
                                                "battlecharacterid"

    };


    void makeData()
    {
        descs.Add(makeList(buildingNames));
        descs.Add(makeList(buildingDescs));
        descs.Add(makeList(buildingBaseCost));
        descs.Add(makeList(buildingBasePassive));
        descs.Add(makeList(upgradeNames));
        descs.Add(makeList(upgradeDescs));
        descs.Add(makeList(upgradeCosts));
        descs.Add(makeList(minigameNames));
        descs.Add(makeList(battleName));
        descs.Add(makeList(battleID));
    }


    List<string> makeList(string[] ls)
    {
        List<string> temp = new List<string>();

        foreach (string s in ls)
        {
            temp.Add(s);
        }
        return temp;
    }





    public string getData(int x, int y)
    {
        return descs[x][y];
    }


    public List<string> getData(int x)
    {
        return descs[x];
    }

    public List<string> getData(string x)
    {

        return descs[getDataName(x)];
    }


    public int getDataName(string xs)
    {
        int x = -1;

        for(int i = 0; i< buildingDataNames.Length; i++)
        {
            if (xs == buildingDataNames[i])
            {
                x = i;
            }
        }
        return x;
    }


    public string getData(string xs, int y)
    {
        int x = getDataName(xs);

        if (x == -1)
        {
            return "Error";
        }
        return getData(x, y);
        
    }

}

