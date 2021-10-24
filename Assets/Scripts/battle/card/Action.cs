using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Action
{
    public DictionaryOfStringAndString actionStats;

    public Debuff buf;
    public Animation ani;

    //string json = JsonUtility.ToJson(this);

    public Action(Effect e)
    {
        defaultAction();
        
    }


    void checkBuff(Effect e)
    {
        if (buf != null)
        {
            if (buf.getDo(0) == "useskill")
            {
                e.getCard().getDeck().loadMiscCard(buf.getDo(1));
            }
        }
    }

    void checkAnimation(Effect e)
    {
        if (ani != null)
        {
            e.getCard().getDeck().loadAnimation(ani.getName());
        }
    }

    void checkSound(Effect e)
    {
        if (actionStats.ContainsKey("actionType"))
        {
            if (actionStats["actionType"].ToLower() == "playsound")
            {
                e.getCard().getDeck().loadSound(actionStats["value"]);
            }
        }
    }

    public void setupAction(Effect e)
    {
        defaultAction();
        checkBuff(e);
        checkAnimation(e);
        checkSound(e);
    }

    public string getDesc()
    {
        string temp = "";

        foreach (KeyValuePair<string, string> attach in actionStats)
        {
            temp += attach.Key + "-" + attach.Value + " ";
        }

        return temp;
    }
    public void defaultAction()
    {

        if (actionStats == null)
        {
            actionStats = new DictionaryOfStringAndString();
        }

        if (!actionStats.ContainsKey("actionType"))
        {
            addActionStat("actionType", "dealdamage");
            addActionStat("elementType", "fire");
            addActionStat("value", "10.0");
            addActionStat("targets", "select");
        }
    }

    public Action()
    {

    }


    public void addActionStat(string type, string val)
    {
        actionStats.Add(type,val);
    }

    public Action deepCopy()
    {
        Action a = new Action();

        foreach (KeyValuePair<string, string> attach in actionStats)
        {
            a.addActionStat(attach.Key, attach.Value);
        }

        return a;
    }

    public void use(Character target, Character source, Effect e)
    {
        switch (actionStats["actionType"])
        {
            case "dealdamage":     
                source.dealDamage(target, actionStats);
                break;
            case "debuff":
                Debuff de = buf.deepCopy();
                de.apply(target, source);
                break;
            case "animation":
                Animation a = ani.deepCopy();
                a.apply(target,source);
                break;

            case "heal":
                source.healFirst(target, actionStats);
                break;
            case "playsound":
                source.playSound(actionStats);
                break;
            case "instantcooldown":
                target.decreaseCooldownChunkAll(source.parseNum(source, source, actionStats["value"]));
                break;
            case "addstat":
                target.addStat(source, actionStats,0);
                break;
            case "addresist":
                target.addStat(source, actionStats, 1);
                break;
            case "settrigger":
                switch(actionStats["set"])
                {
                    case "accuracy":
                        source.checkListCalcAcc(target, source, actionStats);
                        break;
                    case "crit":
                        break;
                    default:
                        break;
                }
                break;
            default:
                return;
        }


    }


}
