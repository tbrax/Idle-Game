using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using CustomHelpNameSpace;
using System.IO;

[Serializable]
public class SaveData
{
    public BigInteger currentCash;
    public BigInteger totalCash;
    public BigInteger runCash;
    public int ascendNum;
    UpgradeHold uHold;
    BuildingHold bHold;
    MiniHold mHold;

    [System.NonSerialized]
    int globalPlace = 3;
    [System.NonSerialized]
    int globalMult;

    [System.NonSerialized]
    BigInteger secondCash;
    [System.NonSerialized]
    double ascendMult;


    public SaveData()
    {

        calcGlobalMult();
    }

    public BigInteger getRunCash()
    {
        return runCash;
    }

    public BigInteger getTotalCash()
    {
        return totalCash;
    }

    void calcGlobalMult()
    {
        globalPlace = 3;
        globalMult = (int)Mathf.Pow(10, globalPlace);
        ascendMult = 1;
    }

    public int getGlobalMult()
    {
        return globalMult;
    }


    public BigInteger buildingCost(int idx)
    {
        return getBHold().getBuilding(idx).getNextCost() * getGlobalMult();
    }

    BigInteger buildingCost(Building b)
    {
        return b.getNextCost() * getGlobalMult();
    }

    public BigInteger upgradeCost(int idx)
    {
        return getUHold().getUpgrade(idx).getCost() * getGlobalMult();
    }

    BigInteger upgradeCost(Upgrade u)
    {
        return u.getCost() * getGlobalMult();
    }


    public void setIcons(List<Sprite> b, List<Sprite> u)
    {
        bHold.setIconList(b);
        uHold.setIconList(u);
    }

    public void setLoadAttributes()
    {
        calcGlobalMult();
        bHold.setUpgradeHold(uHold);

    }

    public Sprite getBIcon(int i)
    {
        return bHold.getIcon(i);
    }

    public Sprite getUIcon(int i)
    {
        return uHold.getIcon(i);
    }

    public Sprite getMIcon(int i)
    {
        return mHold.getIcon(i);
    }

    public void setUHold(UpgradeHold u)
    {
        uHold = u;
    }

    public UpgradeHold getUHold()
    {
        return uHold;
    }

    public MiniHold getMHold()
    {
        return mHold;
    }

    public void setBHold(BuildingHold b)
    {
        bHold = b;
    }

    public BuildingHold getBHold()
    {
        return bHold;
    }

    public void setMHold(MiniHold m)
    {
        mHold = m;
    }

    public void setCash(BigInteger i)
    {
        currentCash = i;
    }

    public BigInteger getCash()
    {
        return currentCash;
    }

    public BigInteger addBonus(BigInteger i)
    {
        BigInteger gain = CustomHelp.bigIntegerMult(i, ascendMult);
        return gain;
    }

    public void addCash(BigInteger i)
    {
        i = addBonus(i);
        currentCash += i;
        runCash += i;
        totalCash += i;
    }

    public void subCash(BigInteger i)
    {
        currentCash -= i;
    }

    public BigInteger getSecond()
    {
        return secondCash;
    }

    public void setSecond(BigInteger i)
    {
        secondCash = i;
    }

    public void saveCharacterJSON(Character data, string saveName)
    {
        string path = Application.persistentDataPath + "/character";
        var folder = Directory.CreateDirectory(path);

        string card = JsonUtility.ToJson(data.getStats());
        File.WriteAllText(path + "/" + saveName + ".json", card);
        //Card save = JsonUtility.FromJson<Card>(json);
        //Debug.Log("Saved character: " + saveName);
    }


    public Character loadCharacter(string saveName, int custom)
    {
        string json = "";

        if (custom == 1)
        {
            json = loadCharacterJSON(saveName);
        }
        else
        {
            json = loadCharacterDefault(saveName);
        }

        if (json != "")
        {
            Character c = new Character();
            StatBlock stats;
            try
            {
                stats = JsonUtility.FromJson<StatBlock>(json);
            }
            catch
            {
                stats = null;
            }
            

            c.setupStats(stats);

            saveCharacterJSON(c,"test");

            return c;
        }
        Debug.LogError("Could not find character " + saveName);
        return null;
    }

    public string loadCharacterDefault(string saveName)
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("Battle/Characters/" + saveName);

        if (jsonTextFile != null)
        {
            return jsonTextFile.text;
        }
        return "";
    }


    public string loadCharacterJSON(string saveName)
    {
        string path = Application.persistentDataPath + "/character/" + saveName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return json;
            //File.WriteAllText(Application.persistentDataPath + "/" + saveName + ".json", potion);
        }
        return "";
    }

    public void saveCardJSON(Card data, string saveName)
    {
        string path = Application.persistentDataPath + "/cards";
        var folder = Directory.CreateDirectory(path);

        string card = JsonUtility.ToJson(data);
        File.WriteAllText(path + "/" + saveName + ".json", card);
        //Card save = JsonUtility.FromJson<Card>(json);
        //Debug.Log("Saved card: " + saveName);
    }

    public Card loadCard(string saveName, int custom = 0)
    {
        string json = "";
        if (custom == 0)
        {
            json = loadCardDefault(saveName);
        }
        else
        {

        }

        if (json != "")
        {
            Card card;
            //Debug.Log("Load " + saveName);
            try
            {
                card = JsonUtility.FromJson<Card>(json);
            }
            catch
            {
                Debug.Log("Error loading " + saveName);
                card = null;
            }

            /*if (saveName == "Pin Down")
            {
                saveCardJSON(card, saveName);
            }*/
            
            //card.setupCard(saveName, null);
            return card;
        }
        Debug.Log("Cant find " + saveName);
        return null;
    }

    string loadCardDefault(string saveName)
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("Battle/Skills/" + saveName);

        if (jsonTextFile != null)
        {
            return jsonTextFile.text;
        }
        return "";
    }


    string loadCardJSON(string saveName)
    {
        string path = Application.persistentDataPath + "/cards/" + saveName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return json;
            //File.WriteAllText(Application.persistentDataPath + "/" + saveName + ".json", potion);
            //Card card = JsonUtility.FromJson<Card>(json);
            //card.setupCard(saveName, null);
            //return card;
        }
        //Card c2 = new Card("Test", null);
        //return c2;
        return "";
    }



}
