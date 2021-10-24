//using Packages.Rider.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Character
{


    public StatBlock stats;
    public Deck deckBattle = null;
    Team team;
    BattleLogic game;

    [System.NonSerialized]
    List<float> abilityCooldowns;

    [System.NonSerialized]
    List<float> abilityCooldownsMax;

    [System.NonSerialized]
    List<bool> autoUse;
    [System.NonSerialized]
    List<Debuff> buffs = new List<Debuff>();

    [System.NonSerialized]
    List<Animation> currentAnimation;

    [System.NonSerialized]
    List<CardSound> currentSound;

    [System.NonSerialized]
    List<Character> accCheck;
    [System.NonSerialized]
    List<Character> critCheck;
    [System.NonSerialized]
    List<int> critCheckAmt;

    int speedMod = 0;

    bool hasDied;

    int locationX;
    int locationY;

    public Character()
    {

    }

    public void make(string name)
    {
        speedMod = 0;
        hasDied = false;
        accCheck = new List<Character>();
        critCheck = new List<Character>();
        removeAllBuffs();
        buffs = new List<Debuff>();
        currentAnimation = new List<Animation>();
        currentSound = new List<CardSound>();

        //stats = new StatBlock(this,name);

        stats.setupStats(this, name);

        setupDeck();
        setupPassive();
        setupTimers();
    }

    public Vector2Int getPos()
    {
        Vector2Int n = new Vector2Int(locationX, locationY);
        return n;
    }

    public float getScaleHealth()
    {
        return stats.getScaleHealth();
    }

    void updateBuffs(float time)
    {
        for (int i = buffs.Count-1; i >= 0; i--)
        {
            buffs[i].tickTime(time);
        }
    }

    void updateAnimations(float time)
    {
        for (int i = currentAnimation.Count - 1; i >= 0; i--)
        {
            currentAnimation[i].fight(time);
        }
    }

    public void addCurrentSound(CardSound c)
    {
        currentSound.Add(c);
    }

    void updateSounds(float time)
    {
        for (int i = currentSound.Count - 1; i >= 0; i--)
        {
            currentSound[i].fight(time);
        }
    }

    public Transform getDoll()
    {
        return game.getDoll(this);
    }

    public List<Debuff> getBuffs()
    {
        return buffs;
    }

    public void addBuff(Debuff b)
    {
        buffs.Add(b);

        triggerEvents(this, "addbuff");
    }


    public void toggleAutoUse(int i)
    {
        if (autoUse.Count > i)
        {
            autoUse[i] = !autoUse[i];
        }

    }

    public bool getAutoUse(int i)
    {
        if (autoUse.Count > i)
        {
            return autoUse[i];
        }

        return false;
    }

    public void removeBuff(Debuff b)
    {
        buffs.Remove(b);
    }

    public void removeAnimation(Animation a)
    {
        currentAnimation.Remove(a);
    }

    public void removeSound(CardSound a)
    {
        currentSound.Remove(a);
    }


    public bool isClass(string s)
    {
        return stats.isClass(s);
    }

    public bool hasDebuff(string s)
    {
        foreach (Debuff b in buffs)
        {
            if (b.getName().ToLower() == s.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    public float getCooldown(int i)
    {
        if (abilityCooldowns.Count > i)
        {
            return abilityCooldowns[i];
        }       
        return 0.0f;
    }

    public float getCooldownPercent(int i)
    {
        if (abilityCooldowns.Count > i && abilityCooldownsMax.Count > i && abilityCooldownsMax[i] != 0)
        {
            return abilityCooldowns[i] / abilityCooldownsMax[i];
        }
        return 0.0f;
    }


    public List<Character> getAccCheck()
    {
        return accCheck;
    }

    public List<Character> getCritCheck()
    {
        return critCheck;
    }

    void setupPassive()
    {

    }


    public void addHistory(string text)
    {
        game.addHistory(this, text);
    }

    void addTimer(Card c)
    {
        abilityCooldownsMax.Add(c.getCooldown());
        abilityCooldowns.Add(c.getCooldown());
        autoUse.Add(false);

    }

    void setupTimers()
    {
        abilityCooldowns = new List<float>();
        abilityCooldownsMax = new List<float>();


        autoUse = new List<bool>();
        foreach ( Card c in deckBattle.getCards())
        {
            addTimer(c);
        }
    }

    bool checkCooldown(int i)
    {
        if (abilityCooldowns[i] > 0.0)
        {
            return false;
        }
        return true;
    }

    public void setSpeedMod(int i)
    {
        speedMod = i;
    }

    public void takeTrigger(string trigger)
    {
        for (int i = buffs.Count-1; i >= 0; i--)
        {
            buffs[i].takeTrigger(trigger);
        }
    }


    public void playSound(DictionaryOfStringAndString actionStats)
    {
        if (actionStats.ContainsKey("value"))
        {
            string soundName = actionStats["value"];
            CardSound c = deckBattle.findSound(soundName);
            if (c != null)
            {
                if (actionStats.ContainsKey("delay"))
                {
                    float number;
                    if (float.TryParse(actionStats["delay"], out number))
                    {
                        c.setDelay(number);
                    }                   
                }

                if (actionStats.ContainsKey("volume"))
                {
                    float number;
                    if (float.TryParse(actionStats["volume"], out number))
                    {
                        c.setVolume(number);
                    }
                }
                c.play(this);
            }          
        }
    }

    public void triggerEvents(Character target, string eventString)
    {
        game.triggerEvents(target, this, eventString);
    }

    public void decreaseCooldownChunkAll(float amt)
    {
        for (int i = 0; i < abilityCooldowns.Count; i++)
        {
            decreaseCooldownChunk(amt, i);
        }
    }

    public void decreaseCooldownChunk(float amt, int i)
    {
            float coolAmt = (amt/100) * abilityCooldownsMax[i];
            abilityCooldowns[i] -= coolAmt;
    }

    public void decreaseCooldown(int i, float time, int speed)
    {

        if (abilityCooldowns[i] > 0.0f)
        {
            float modTime = time;

            modTime *= (1 + ((float)speed /100.0f));

            float sub = abilityCooldowns[i] - modTime;
            abilityCooldowns[i] = sub;

            if (abilityCooldowns[i] < 0.0f)
            {
                abilityCooldowns[i] = 0.0f;

            }
        }

        if (autoUse.Count > i)
        {
            if (autoUse[i])
            {
                skillClick(i);
            }           
        }
    }

    public string getImageID()
    {
        return stats.imageID;
    }

    public void decreaseCooldownAll(float time)
    {
        updateBuffs(time);
        updateAnimations(time);
        updateSounds(time);
        //triggerEvents(this, "frame");

        //int speedMod = (int)(parseNum(this, this, getBaseStat("speed")));
        if (speedMod < -100)
        {
            speedMod = -100;
        }

        if (canFight())
        {
            for (int i = 0; i < abilityCooldowns.Count; i++)
            {
                decreaseCooldown(i, time, speedMod);
            }
        }
    }


    Character getAdjTarget()
    {
        Character adj = game.getAdjacent(this);
        if (adj.isAlive())
        {
            return adj;
        }
        else
        {
            return game.randomAlive(adj);
        } 
    }

    public void skillClick(int i)
    {
        if (canFight())
        {
            if (abilityCooldowns.Count > i)
            {
                if (checkCooldown(i))
                {
                    Card c = getCard(i);
                    abilityCooldowns[i] = c.getCooldown();
                    addHistory(c.getName());
                    useCard(c, getAdjTarget());
                }
            }
        }
    }



    public BattleLogic getBattleLogic()
    {
        return game;
    }

    public void setGame(BattleLogic g,string name)
    {
        game = g;
        make(name);
        
    }

    public void addTeam(Team t)
    {
        team = t;
    }

    public Team getTeam()
    {
        return team;
    }

    public List<Character> getEnemyTeam()
    {
        return game.getEnemyTeam(this);
    }

    public List<Character> getAllyTeam()
    {
        return game.getAllyTeam(this);
    }

    public List<Card> getDeck()
    {
        return deckBattle.getCards();
    }

    public List<Card> getHand()
    {
        return deckBattle.getHand();
    }

    public string getName()
    {
        return stats.getName();
    }

    public StatBlock getStats()
    {
        return stats;
    }

    public Transform getAnimationParent()
    {
        return game.getAnimationParent();
    }

    public void removeAllBuffs()
    {
        for(int i = buffs.Count -1; i >= 0; i--)
        {
            buffs[i].die();
        }

    }



    public void playAnimation(Animation a, Character target)
    {
        deckBattle.addAnimationFrames(a);
        a.play(target);

        target.addAnimation(a);
    }

    public void addAnimation(Animation a)
    {
        currentAnimation.Add(a);

    }

    public void checkListCalcAcc(Character target, Character source, DictionaryOfStringAndString actionStats)
    {
        if (source.accCheckListCalc(source, target, actionStats))
        {
            accCheck.Add(target);
        }
        else
        {
            triggerEvents(this, "miss");
            triggerEvents(target, "dodge");
            target.addHistory("dodge");
        }
    }

    public void useCard(Card c, Character target)
    {        
        if (c != null)
        {
            if (accCheck.Count > 0)
            {
                accCheck = new List<Character>();
            }
            if (critCheck.Count > 0)
            {
                critCheck = new List<Character>();
            }
            c.use(this, target);
        }
        
    }

    public void healthDie()
    {
        if (!hasDied)
        {
            hasDied = true;
            game.healthDie(this);

            triggerEvents(this, "die");
        }
    }

    public bool isAlive()
    {
        if (hasDied)
        {
            return false;
        }

        if (getScaleHealth() > 0.0f)
        {
            return true;
        }
        healthDie();
        return false;
    }



    public bool isStunned()
    {
        foreach(Debuff d in buffs)
        {
            if (d.getTriggerDo() == "stun")
            {
                return true;
            }
        }
        return false;
    }

    public bool canFight()
    {
        if (!(isAlive()) || isStunned())
        {
            return false;
        }
        return true;
    }


    //not working
    public void useCardFromDeckMisc(Character target, string name)
    {
        Card c = deckBattle.getCardMisc(name);

        useCard(c, target);
    }


    public string getCardDesc(int i)
    {
        Card c = getCard(i);


        return c.getDesc();
    }

    Card getCard(int spot)
    {
        List<Card> cds = getDeck();
        if (cds.Count > spot)
        {
            Card cd = cds[spot];
            return cd;
        }
        return null;
    }

    public void saveCard(Card c)
    {
        game.saveCard(c, "TestDebuff");
    }

    public void updateInterface()
    {
        game.updateAllCharacterGraphics();
    }
    /*
    public void addCard(Card c)
    {
        deckBattle.addCard(c);
    }
    */

    public Card loadCard(string name)
    {
        return game.loadCard(name);
    }


    void setupDeck()
    {
        DictionaryOfStringAndString deckdict = stats.getStartDeck();
        DictionaryOfStringAndString passivedict = stats.getStartPassives();
        deckBattle = game.createDeck(deckdict, passivedict,this);
    }

    public void setupStats(StatBlock stat)
    {
        stats = stat;
    }


    public void activatePassives()
    {
        deckBattle.activatePassives();
    }

    public float parseNum(Character target, Character own, string num)
    {
        return game.parseNum(target,own,num);
    }

    public string parseType(Character target, Character own, string type)
    {
        return type;
    }

    public void addStatResist(string type, float amt)
    {
        stats.addStat(1,type, amt);
    }


    public void addStat(Character source, DictionaryOfStringAndString action, int statType = 0)
    {
        string st = "";
        if (action.ContainsKey("stat"))
        {
            st = action["stat"];
        }

        string val = "";
        if (action.ContainsKey("value"))
        {
            val = action["value"];
        }

        float f = source.parseNum(this, source, val);

        if (statType == 1)
        {
            addStatResist(st, f);
        }
        else
        {
            addStatBase(st, f);
        }

    }

    public void addStatBase(string type, float amt)
    {
        stats.addStat(0,type, amt);
    }

    public void takeDamage(Character source, string type, string amt, float critMult)
    {
        float trueAmt = parseNum(this,source,amt);

        float baseDamage = parseNum(source, source, source.getBaseStat("damage"));
        trueAmt *= (baseDamage / 100);

        string trueType = parseType(this, source, type);
        float damAmt = stats.calcTakeDamage(trueAmt,trueType);

        if (damAmt < 0)
        {
            DictionaryOfStringAndString sts = getStatsDict();
            if (sts.ContainsKey("healself"))
            {
                damAmt = damAmt * ((100+parseNum(this,this,sts["healself"]))/100);
            }

        }

        string crittxt = "";
        if  (critMult > 0)
        {
            damAmt += (damAmt * (critMult / 100));
            crittxt = "crit ";
        }

        stats.takeDamage(damAmt);

        if (critMult > 0)
        {
            triggerEvents(this, "takecrit");
        }

        if (damAmt >= 0)
        {
            triggerEvents(this, "takedamage");
            triggerEvents(this, "takedamagetype" + trueType.ToLower());
        }
        else
        {
            triggerEvents(this, "takeheal");
        }       
        addHistory("" + (-damAmt).ToString("F2") + " "+ crittxt + trueType);
        updateInterface();
    }

    public float luckRoll()
    {
        float l = UnityEngine.Random.Range(0.0f, 1.0f);

        float basL = parseNum(this, this, getBaseStat("critchance"));

        l = l * (1 + (basL / 100));
        if (l > 1.0f)
        {
            l = 1.0f;
        }
        return l;
    }


    public int critRoll(int baseChance)
    {
        int maxcritroll = 10;
        int c = 0;

        while (baseChance >= 100 && maxcritroll > 0)
        {
            maxcritroll -= 1;
            baseChance -= 100;
            c += 1;
        }

        int roll = UnityEngine.Random.Range(0, 100);
        if(baseChance > roll)
        {
            c += 1;
        }

        return c;
    }


    public int baseAccuracy(Character target, Character source)
    {       
        return (int)(parseNum(source, source, getBaseStat("accuracy")) - parseNum(target, target, target.getBaseStat("dodge")));
    }

    public int baseCritChance(Character target, Character source)
    {
      return (int)(parseNum(source, source, getBaseStat("critchance")) - parseNum(target, target, target.getBaseStat("critchanceresist")));
    }


    public int critCheckOneChar(Character target, Character source, int accMod)
    {
        int baseAmt = source.baseCritChance(target, source);
        int hitAmt = source.critRoll(baseAmt);
        return hitAmt;

    }

    public int accCheckOneChar(Character target, Character source, int accMod)
    {
        int baseAmt = source.baseAccuracy(target, source);
        int hitAmt = source.critRoll(100 + baseAmt);
        return hitAmt;

    }


    public bool accCheckListCalc(Character source, Character target, DictionaryOfStringAndString actionStats)
    {
        int accMod = 0;
        if (actionStats.ContainsKey("accuracy"))
        {
            accMod += (int)(parseNum(source, source, actionStats["accuracy"]));
        }

        int hitAmt = accCheckOneChar(target, source, accMod);
        if (hitAmt > 0)
        {
            return true;
        }

        return false;
    }



   
    public void healFirst(Character target, DictionaryOfStringAndString actionStats)
    {
        string amt = "0";
        //int baseCritChance = (int)(parseNum(this, this, getBaseStat("critchance")) - parseNum(target, target, getBaseStat("critchanceresist")));
        int baseCritDamage = (int)(parseNum(this, this, getBaseStat("critdamage")));

        int baseCritChanceI = (int)(parseNum(this, this, getBaseStat("critchance")));

        if (actionStats.ContainsKey("value"))
        {
            amt = actionStats["value"];
        }
        if (actionStats.ContainsKey("critchance"))
        {
            baseCritChanceI += (int)(parseNum(this, this, actionStats["critchance"]));
        }

        int critBonus = 0;
        int critAmt = critRoll(baseCritChanceI);
        if (critAmt > 0)
        {
            baseCritDamage = (baseCritDamage * critAmt);
            critBonus = baseCritDamage;
        }

        target.takeDamage(this, "heal", "-"+amt, critBonus);
    }


    //public void modStat


    public void dealDamage(Character target, DictionaryOfStringAndString actionStats)
    {
        //actionStats["elementType"], actionStats["value"]
        string type = "slash";
        string amt = "0";

        int baseCritDamage = (int)(parseNum(this, this, getBaseStat("critdamage")) - parseNum(target, target, target.getBaseStat("critdamageresist")));
        int baseCritChanceI = baseCritChance(target,this);


        //int baseAccuracyI = (int)(parseNum(this, this, getBaseStat("accuracy")) - parseNum(target, target, target.getBaseStat("dodge")));



        if (actionStats.ContainsKey("elementType"))
        {
            type = actionStats["elementType"];
        }
        if (actionStats.ContainsKey("value"))
        {
            amt = actionStats["value"];
        }
        if (actionStats.ContainsKey("critchance"))
        {
            baseCritChanceI += (int)(parseNum(this, this, actionStats["critchance"]));
        }
        /*if (actionStats.ContainsKey("accuracy"))
        {
            baseAccuracyI += (int)(parseNum(this, this, actionStats["accuracy"]));
        }*/

        if (actionStats.ContainsKey("critdamage"))
        {
            baseCritDamage += (int)(parseNum(this, this, actionStats["critdamage"]));
        }


        int critBonus = 0;
        int critAmt = critRoll(baseCritChanceI);
        if (critAmt > 0)
        {
            baseCritDamage = (baseCritDamage * critAmt);
            critBonus = baseCritDamage; 
        }

        //amt = amt * (baseDamage / 100);

        //int accAmt = critRoll(100+baseAccuracyI);

        //if (accAmt > 0)
       // {
            target.takeDamage(this, type, amt, critBonus);
        //}
        //else
        //{
         //   triggerEvents(this, "miss");
        //    triggerEvents(target, "dodge");
        //}   
    }


    public DictionaryOfStringAndString getStatsDict()
    {
        return stats.getStatsDict();
    }

    public string getBaseStat(string st)
    {
        return stats.getBaseStat(st);
    }

    public string getResistStat(string st)
    {
        return stats.getResistStat(st);
    }

    public string getDamageStat(string st)
    {
        return stats.getDamageStat(st);
    }



    void doChange(Character target, string change)
    {

    }

    void handleChange(Change change)
    {

    }

}
