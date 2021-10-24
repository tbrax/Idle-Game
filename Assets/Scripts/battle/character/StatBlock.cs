using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class StatBlock
{
    public DictionaryOfStringAndString baseStats;
    public DictionaryOfStringAndString damageType;
    public DictionaryOfStringAndString resistType;

    public DictionaryOfStringAndString startDeck;

    public DictionaryOfStringAndString startPassives;

    public List<string> races;

    DictionaryOfStringAndString baseStatsTemp;
    DictionaryOfStringAndString damageTypeTemp;
    DictionaryOfStringAndString resistTypeTemp;

    Character character;
    public string charName;
    public string imageID;

    float scaleHealth = 1.0f;


    public StatBlock()
    {

    }

    public float getScaleHealth()
    {
        return scaleHealth;
    }

    void updateScaleHealth()
    {
        scaleHealth = character.parseNum(character, character, getBaseStat("health")) / character.parseNum(character, character, getBaseStat("maxhealth"));


    }

    public void setupStats(Character c, string name)
    {
        charName = name;
        character = c;
        basicStats();
        startBattle();
    }


    public DictionaryOfStringAndString getBuffsTimeDict()
    {
        DictionaryOfStringAndString ret = new DictionaryOfStringAndString();

        for (int i = 0; i < character.getBuffs().Count; i++)
        {
            ret.Add(character.getBuffs()[i].getName() + "[" + i.ToString(), character.getBuffs()[i].getDescDur());
        }

        return ret;
    }

    public DictionaryOfStringAndString getBuffsDict()
    {
        DictionaryOfStringAndString ret = new DictionaryOfStringAndString();

        for (int i = 0; i < character.getBuffs().Count; i++)
        {
            ret.Add(character.getBuffs()[i].getName() + "["+ i.ToString(), character.getBuffs()[i].getDesc());
        }

        return ret;
    }

    public DictionaryOfStringAndString getStatsDict()
    {
        return baseStats;
    }

    public DictionaryOfStringAndString getResistDict()
    {
        return resistType;
    }

    public DictionaryOfStringAndString getStartDeck()
    {
        return startDeck;
    }

    public DictionaryOfStringAndString getStartPassives()
    {
        return startPassives;
    }

    public string getName()
    {
        return charName;
    }

    public string getImageID()
    {
        return imageID;
    }



    public bool isClass(string s)
    {
        foreach (string r in races)
        {
            if (r.ToLower() == s.ToLower())
            {
                return true;
            }
        }

        return false;
    }



    void setImageID(string s)
    {
        imageID = s;
    }
    void basicDeck()
    {
        startDeck.Add("Fire Blast", "5");
        startDeck.Add("Ice Storm", "5");
        startDeck.Add("Lightning Strike", "5");
    }

    void basicPassive()
    {
        startPassives.Add("Side Swipe", "1");

    }


    void addIfNotExist(string key, string val)
    {
        if (!(baseStats.ContainsKey(key)))
        {
            baseStats.Add(key, val);
        }
    }


    void makeIfNotExist(DictionaryOfStringAndString c)
    {
        if (c == null)
        {
            c = new DictionaryOfStringAndString();
        }
    }


    void basicStats()
    {

        if (baseStats == null)
            baseStats = new DictionaryOfStringAndString();
        if (damageType == null)
            damageType = new DictionaryOfStringAndString();
        if (resistType == null)
            resistType = new DictionaryOfStringAndString();

        baseStatsTemp = new DictionaryOfStringAndString();
        damageTypeTemp = new DictionaryOfStringAndString();

        resistTypeTemp = new DictionaryOfStringAndString();
        if (startDeck == null)
            startDeck = new DictionaryOfStringAndString();

        if (startPassives == null)
        {
            startPassives = new DictionaryOfStringAndString();
        }
        addIfNotExist("maxhealth", "100");
        addIfNotExist("damage", "100");
        addIfNotExist("resist", "100");
        addIfNotExist("resistTypeStart", "0"); 
        setStat(baseStats,"health",getStat(baseStats,"maxhealth"));

        updateScaleHealth();

        if (imageID == null)
        {
            imageID = "lion0";
        }
        //basicResist();
        //basicDeck();
    }

    void basicResist()
    {
        resistType.Add("fire", "100");
        resistType.Add("cold", "150");
        resistType.Add("energy", "50");

    }

    void startBattle()
    {
        baseStats["health"] = baseStats["maxhealth"];
    }
    
    public void addResistType(string type)
    {
        if (!resistType.ContainsKey(type))
        {
            resistType.Add(type, "0");
        }
        if (!resistTypeTemp.ContainsKey(type))
        {
            string newAdd = "0";
            if (baseStats.ContainsKey("resistTypeStart"))
            {
                newAdd = baseStats["resistTypeStart"];
            }
            resistTypeTemp.Add(type, newAdd);
        }

    }

    public void addDamageType(string type)
    {

    }

    float parseNum(Character target,string num)
    {
        return character.parseNum(target,character,num);
    }

    public float calcTakeDamage(float amt, string type)
    {
        addResistType(type);
        float multt = float.Parse(resistType[type]);
        float multt1 = float.Parse(resistTypeTemp[type]);

        float res = multt + multt1;
        float ret;

        if (type == "heal")
        {
            ret = amt * ((100 + res) / 100);
        }
        else

        {
            ret = amt * ((100 - res) / 100);
        }

        return ret;
    }

    DictionaryOfStringAndString getTemp(DictionaryOfStringAndString loc)
    {
        DictionaryOfStringAndString loc2;

        if (loc == baseStats)
        {
            loc2 = baseStatsTemp;
        }
        else if (loc == damageType)
        {
            loc2 = damageTypeTemp;
        }
        else if (loc == resistType)
        {
            loc2 = resistTypeTemp;
        }
        else
        {
            return null;
        }
        return loc2;
    }

    void setStat(DictionaryOfStringAndString loc, string st, string val)
    {
        DictionaryOfStringAndString loc2 = getTemp(loc);

        if (!loc2.ContainsKey(st))
        {
            addTempStat(loc2, st);
        }

        //ini = parseNum(character, loc2[st]);
        //float nw = ini + val;
        //loc[st] = nw.ToString(roundS);
        loc[st] = val;
    }


    void addTempStat(DictionaryOfStringAndString loc, string st)
    {
        loc.Add(st, "0");
    }


    public string getBaseStat(string st)
    {
        return getStat(baseStats, st);
    }

    public string getResistStat(string st)
    {
        return getStat(resistType, st);
    }

    public string getDamageStat(string st)
    {
        return getStat(damageType, st);
    }



    string getStat(DictionaryOfStringAndString loc, string st)
    {
        DictionaryOfStringAndString loc2 = getTemp(loc);

        if (loc.ContainsKey(st))
        {
            return loc[st];
        }
        if (!loc2.ContainsKey(st))
        {
            addTempStat(loc2, st);
        }

        return "";
    }
    
    public void addStat(int placeNum, string type, float amt)
    {

        DictionaryOfStringAndString place;
        if (placeNum == 1)
        {
            place = resistType;
        }
        else
        {
            place = baseStats;
        }

        if (getStat(place, type) != "")
        {
            float hpVal = character.parseNum(character, character, getStat(place, type));
            string newVal = (hpVal + amt).ToString();

            setStat(place, type, newVal);

            if (type == "speed")
            {
                character.setSpeedMod((int)(character.parseNum(character, character, getBaseStat("speed"))));
            }
        }
        else
        {
            if (placeNum == 1)
            {
                addResistType(type);
            }
            else
            {
                addIfNotExist(type, "0");
            }

            
        }
    }



    public void takeDamage(float damage)
    {
        float hpVal = character.parseNum(character,character,getStat(baseStats, "health"));
        string newVal = (hpVal - damage).ToString();
        setStat(baseStats,"health",newVal);

        float hh = character.parseNum(character,character,getBaseStat("health"));
        float mh = character.parseNum(character, character, getBaseStat("maxhealth"));


        
        if (hh < 0)
        {
            setStat(baseStats, "health", "0");
        }
        else if (hh > mh)
        {
            setStat(baseStats, "health", mh.ToString());
        }

        updateScaleHealth();

    }

}
