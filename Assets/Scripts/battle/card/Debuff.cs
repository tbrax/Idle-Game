using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Debuff
{
    public float duration;
    public int tickNum = 1;
    public string tickTriggerWhen;
    public string tickTriggerDo;
    public string desc;
    public string name;

    [System.NonSerialized]
    int tickCharge;
    [System.NonSerialized]
    bool triggerTick;
    [System.NonSerialized]
    bool canTick;
    [System.NonSerialized]
    float timer;

    [System.NonSerialized]
    List<float> tickTimes;

    [System.NonSerialized]
    int tickNext;
    [System.NonSerialized]
    Character target;
    [System.NonSerialized]
    Character owner;
    [System.NonSerialized]
    float statTaken = 0;

    string statTakenStr = "";

    List<string> doList;

    public void apply(Character ta, Character ow)
    {
        timer = 0;
        canTick = true;
        target = ta;
        owner = ow;
        doList = seperateStr(tickTriggerDo);
        ta.addBuff(this);
        calcTickTimes();
        checkTick();
    }

    public string getName()
    {
        return name;
    }

    public string getDesc()
    {
        return desc;
    }

    public string getDescDur()
    {
        if (!triggerTick)
        {
            return (duration-timer).ToString("f2");
        }
        else
        {
            if (tickNum != -1)
            {
                return tickNum.ToString();
            }
            else
            {
                if (duration > 0)
                {
                    if (timer < duration)
                    {
                        return (duration - timer).ToString("F2");
                    }
                }
                return "-";
            }
        }      
    }

    public string getTriggerDo()
    {
        return tickTriggerDo;
    }

    public string getDo()
    {
        return tickTriggerDo;
    }

    public Debuff()
    {
        basicDebuff();
    }

    public void takeTrigger (string action)
    {
        refreshStat();
        if (triggerTick)
        {           
            if (action == tickTriggerWhen)
            {             
                if (timer >= duration)
                {
                    timer = 0;
                    tick();
                    if (tickCharge != -1)
                    {
                        tickCharge -= 1;
                        if (tickCharge <= 0)
                        {
                            die();
                        }
                    }
                }
            }
        }
    }

    void basicDebuff()
    {
        duration = 5;
        tickNum = 5;
        tickTriggerWhen = "tick";
        tickTriggerDo = "useskill[LionBleedTick";
        //desc = "does something";
        //name = "debuff name";
    }

    public string getDo(int i)
    {
        List<string> s = seperateStr(tickTriggerDo);

        if (s.Count > i)
        {
            return s[i];
        }
        return "";
    }

    void useCard(Character target, Character owner, string name)
    {
        owner.useCardFromDeckMisc(target,name);
    }

    void triggerTickSetup(string s)
    {
        tickCharge = tickNum;
        triggerTick = true;
    }

    void calcTickTimes()
    {
        tickCharge = -1;
        triggerTick = false;
        tickTimes = new List<float>();
        if (tickTriggerWhen == "start")
        {
            tickTimes.Add(0.0f);
        }
        else if (tickTriggerWhen == "end")
        {
            tickTimes.Add(duration);
        }
        else if (tickTriggerWhen == "tick")
        {
            float div = duration / tickNum;
            float t = 0.0f;
            for (int i = 0; i < tickNum; i++)
            {
                t += div;
                tickTimes.Add(t);
            }
        }
        else
        {
            triggerTickSetup(tickTriggerWhen);
        }

        tickNext = 0;
    }

    public void die()
    {
        returnStat();
        target.removeBuff(this);
        target.triggerEvents(target, "removedbuff");
    }

    public void lightDie()
    {
        returnStat();
        target.removeBuff(this);
        target.triggerEvents(target, "removedbuff");
    }


    public void tickTime(float time)
    {
        timer += time;
        checkTick();
    }

    void tick()
    {
        if (canTick)
        {
            canTick = false;
            performAction();
            target.triggerEvents(target, "tickbuff");
        }
    }

    List<string> seperateStr(string s)
    {
        List<string> spt = new List<string>();
        string tmp = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '[')
            {
                spt.Add(tmp);
                tmp = "";
            }
            else
            {
                tmp += s[i];
            }
        }
        spt.Add(tmp);
        return spt;
    }

    void returnStat()
    {
        //List<string> s = seperateStr(tickTriggerDo);
        if (doList[0] == "stat")
        {
            modStat(doList[1],statTaken * -1);
        }
    }

    void modStat(string stat, float f)
    {
        statTaken = f;
        target.addStatBase(stat, f);
    }


    void modStat(string stat, string amt)
    {
        float f = owner.parseNum(target, owner, amt);
        statTaken = f;

        statTakenStr = stat;
        target.addStatBase(stat,f);
    }



    void refreshStat()
    {
        //List<string> s = seperateStr(tickTriggerDo);
        if (doList[0] == "stat")
        {
            if (statTaken != 0)
            {
                float f = target.parseNum(target, target, doList[2]);
                if (f != statTaken)
                {
                    returnStat();
                    modStat(doList[1], doList[2]);
                }
            }    
        }
    }


    void performAction()
    {
        List<string> s = seperateStr(tickTriggerDo);

        if (s[0] == "useskill")
        {
            
            useCard(target, owner, s[1]);
        }
        if (s[0] == "stat")
        {
            returnStat();
            modStat(s[1], s[2]);
        }
    }

    void calcTickNext()
    {
        
        tickNext += 1;
        if (tickNext >= tickTimes.Count)
        {
            tickNext = -1;
        }

    }

    void checkTick()
    {
        canTick = true;
        if (!(triggerTick))
        {
            //target.triggerEvents(target, "tickbufftimer");

            while (tickNext != -1 && timer >= tickTimes[tickNext])
            {
                tick();
                calcTickNext();
            }
            if (timer >= duration)
            {
                die();
            }
        }
        else
        {
            if (timer >= duration)
            {
                timer = duration;
            }
        }
    }

    public Debuff deepCopy()
    {
        Debuff copy = new Debuff();

        copy.duration = duration;
        copy.tickNum = tickNum;
        copy.tickTriggerWhen = tickTriggerWhen;
        copy.tickTriggerDo = tickTriggerDo;
        copy.name = name;
        copy.desc = desc;

        return copy;
    }


}
