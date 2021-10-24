using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Data;
using System;
public class BattleLogic
{

    public List<Team> teams;
    List<Character> characterOrder;
    public List<Character> characters;

    Character selectedCharacter;

    SaveData save;
    CharacterDoll doll;
    //string roundS = "F2";

    bool isPaused = false;
    DataTable datat = new DataTable();
    ConstantData constData;

    float totalTime = 0.0f;
    float intervalTime = 0.0f;

    List<string> playerTeamLoadIDAlly;
    List<string> playerTeamLoadIDEnemy;

    bool battleLoaded = false;

    List<List<string>> history;

    GameGrid grid;

    public BattleLogic(SaveData s,ConstantData c, GameObject battle)
    {

        save = s;
        constData = c;

        setupGrid(battle);
        setupDoll(battle);
        //setupBattle();
        
    }

    void setupBattle()
    {
        totalTime = 0.0f;
        intervalTime = 0.0f;
        makeTwoTeams();
        setupMisc();
        setupCharacters();
        battleLoaded = false;
        selectedCharacter = null;
    }

    public Character getSelectedCharacter()
    {

        return selectedCharacter;
    }

    void setupMisc()
    {
        characterOrder = new List<Character>();
        
    }

    public Vector2 getSquarePos(int x, int y)
    {
        return grid.getSquarePos(x, y);

    }

    public bool loadAndSetup(List<string> namesAlly, List<int> posAlly, List<string> namesEnemy, List<int> posEnemy)
    {

        List<int> orderListAlly = new List<int>();
        List<int> orderListEnemy = new List<int>();

        playerTeamLoadIDAlly = new List<string>();
        playerTeamLoadIDEnemy = new List<string>();

        if (namesEnemy.Count < namesAlly.Count)
        {
            namesEnemy = new List<string>();
            posEnemy = new List<int>();
            for (int i = 0; i < namesAlly.Count; i++)
            {
                namesEnemy.Add(namesAlly[i]);
                posEnemy.Add(posAlly[i]);
            }
        }





        for (int i = 0; i < posAlly.Count; i++)
        {
            orderListAlly.Add(posAlly[i]);
        }
        orderListAlly.Sort();

        for (int i = 0; i < orderListAlly.Count; i++)
        {
            int inde = posAlly.IndexOf(orderListAlly[i]);
            playerTeamLoadIDAlly.Add(namesAlly[inde]);
        }
        
        for (int i = 0; i < posEnemy.Count; i++)
        {
            orderListEnemy.Add(posEnemy[i]);
        }
        orderListEnemy.Sort();

        for (int i = 0; i < orderListEnemy.Count; i++)
        {
            int inde = posEnemy.IndexOf(orderListEnemy[i]);
            playerTeamLoadIDEnemy.Add(namesEnemy[inde]);
        }


        /*
        string ss = "Loaded ";

        for (int i = 0; i < playerTeamLoadID.Count; i++)
        {
            ss += "" + playerTeamLoadID[i] + "/ ";
        }*/



        return true;
    }

    void setupDoll(GameObject o)
    {
        doll = new CharacterDoll(o, this);
        
        

    }

    void setupCharacters()
    {
        characters = new List<Character>();
        addPlayerCharacters();
        addEnemyCharacters();
        updateAllCharacterGraphics();

    }

    public void healthDie(Character c)
    {
        doll.healthDieChange(getIndexFromCharacter(c));
    }

    void showCharacterData(Character c)
    {
        doll.showCharacterData(c);
    }

    void setupHistory()
    {
        history = new List<List<string>>();
        for (int i = 0; i < characters.Count; i++)
        {
            List<string> tmp = new List<string>();
            tmp.Add("");
            tmp.Add("");
            tmp.Add("");
            history.Add(tmp);
        }
    }


    public List<List<string>> getHistory()
    {
        return history;
    }

    public void addHistory(Character c, string text)
    {
        int idx = getIndexFromCharacter(c);
        history[idx].Insert(0, text);
    }

    void setupGrid(GameObject o)
    {
        grid = new GameGrid(o.transform.Find("arena").Find("terrain"));
    }

    void restartGrid()
    {
        grid.setupGrid();
    }

    public void restartBattle()
    {
        if (playerTeamLoadIDAlly != null && playerTeamLoadIDEnemy != null)
        {
            restartGrid();
            setupBattle();
            setupHistory();
            battleLoaded = true;
            doll.startBattle();
            startBattle();
        }
    }


    public void startBattle()
    {
        activatePassives();

        foreach (Character c in characters)
        {
            triggerEvents(c,c, "startbattle");
        }
        
    }

    void addPlayerCharacters()
    {

        foreach(string s in playerTeamLoadIDAlly)
        {
            setupCharacter(s, "", 0);
        }

        //setupCharacter("lion", "lion0", 0);
        //setupCharacter("lion", "lion1", 0);
        //setupCharacter("lion", "lion2", 0);
    }

    void addEnemyCharacters()
    {
        foreach (string s in playerTeamLoadIDEnemy)
        {
            setupCharacter(s, "", 1);
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused;
    }

    public void setPause(bool s)
    {
        isPaused = s;
    }

    public Character getCharacter(int i)
    {

        if (characters.Count > i)
        {
            return characters[i];
        }

        return null;
    }

    public void fight(float time)
    {
        

        doll.amimationUpdate(time);


        if (battleLoaded && !(isPaused))
        {
            intervalTime += time;
            totalTime += time;
            doll.timerS(totalTime);
            if (intervalTime > 0.01)
            {
                foreach (Character c in characters)
                {
                    c.decreaseCooldownAll(intervalTime);
                }
                characterBoxDataUpdate();

                intervalTime = 0;
            }

            updateAllCharacterGraphics();
        }
    }


    public Character randomAlive(Character c)
    {
        List<Character> rd = new List<Character>();

        List<Character> t = c.getTeam().getChas();

        foreach(Character ca in t)
        {
            if (ca.isAlive())
            {
                rd.Add(ca);
            }
        }

        if (rd.Count == 0)
        {
            return c;
        }

        return rd[UnityEngine.Random.Range(0, rd.Count)];

    }

    public int getIndexFromCharacter(Character c)
    {
        for (int i = 0; i < characters.Count; i++)
        {

            if (characters[i] == c)
            {
                return i;
            }

        }
        return -1;
    }

    public void triggerEvents(Character target, Character source, string eventString)
    {


        List<Character> allies = getAllyTeam(source);
        List<Character> enemies = getEnemyTeam(source);
        source.takeTrigger("self" + eventString);
        foreach (Character c in allies)
        {
            c.takeTrigger("ally"+eventString);
        }
        foreach (Character c in enemies)
        {
            c.takeTrigger("enemy" + eventString);
        }

        updateAllCharacterGraphics();
    }


    public void updateAllCharacterGraphics()
    {
        foreach (Character c in characters)
        {
            updateCharacterGraphic(c);
        }
    }

    public void updateCharacterGraphic(Character c)
    {
        doll.updateCharacter(c, getIndexFromCharacter(c));
    }



    void characterBoxDataUpdate()
    {
        if (selectedCharacter != null)
        {
            showCharacterData(selectedCharacter);
        }
    }


    public void clickCharacter(int i)
    {
        if (characters.Count > i)
        {
            selectedCharacter = characters[i];
            doll.openCharacterData(selectedCharacter);
        }
        doll.updateSpinAuto(selectedCharacter);
    }


    Team oppositeTeam(Team t)
    {
        if (teams.Count == 2)
        {
            if (t == teams[1])
            {
                return teams[0];
            }
            else if (t == teams[0])
            {
                return teams[1];
            }
        }
        return null;
    }

    public Character getAdjacent(Character c)
    {
        Team t = oppositeTeam(c.getTeam());
        int i = c.getTeam().getIndex(c);
        List<Character> cs = t.getChas();
        
        if (cs.Count > i)
        {

            return cs[i];
        }

        return c;
    }

    void useSkill(Character c, int spot)
    {
        if (!(isPaused))
        {
            c.skillClick(spot);
        }
        updateAllCharacterGraphics();
    }

    public void clickSkill(int i)
    {
        if (selectedCharacter != null)
        {
            useSkill(selectedCharacter, i);
        }
    }

    public void clickSkillAuto(int i)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.toggleAutoUse(i);
        }

        doll.updateSpinAuto(selectedCharacter);
    }




    public void clickSkillInfo(int i)
    {
        if (selectedCharacter != null)
        {
            string s = selectedCharacter.getCardDesc(i);
            doll.openHelpBox(s);
        }
    }

    public void clickDataButton(int i)
    {
        doll.charInfoWindowOpen();

    }

    void setupCharacter(string load, string name, int team)
    {
        
        Character c = createCharacter(load, name);

        if (c != null)
        {
            characters.Add(c);
            teams[team].addCharacter(c);
        }

    }

    Character loadCharacter(string name, int custom = 0)
    {
        return save.loadCharacter(name, custom);
    }

    void saveCharacter(Character c, string name)
    {
        save.saveCharacterJSON(c,name);
    }


    public List<string> getConstData(string name)
    {
        return constData.getData(name);
    }

    public void saveCard(Card c, string name)
    {
        save.saveCardJSON(c, name);
    }

    public List<Character> getAllyTeam(Character c)
    {
        List<Character> e = new List<Character>();
        foreach (Character ct in characters)
        {
            if (ct.getTeam() == c.getTeam())
            {
                e.Add(ct);
            }
        }
        return e;
    }

    public List<Character> getAllyTeamOther(Character c)
    {
        List<Character> e = getAllyTeam(c);
        if (e.Contains(c))
        {
            e.Remove(c);
        }
        return e;
    }


    public List<Character> getEnemyTeam(Character c)
    {
        List<Character> e = new List<Character>();
        foreach (Character ct in characters)
        {
            if (ct.getTeam() != c.getTeam())
            {
                e.Add(ct);
            }
        }
        return e;
    }

    List<Character> getAllyAlive(Character c)
    {
        List<Character> e = new List<Character>();
        foreach(Character ct in getAllyTeam(c))
        {
            if (ct.isAlive())
            {
                e.Add(ct);
            }
        }
        return e;
    }

    List<Character> getEnemyAlive(Character c)
    {
        List<Character> e = new List<Character>();
        foreach (Character ct in getEnemyTeam(c))
        {
            if (ct.isAlive())
            {
                e.Add(ct);
            }
        }
        return e;
    }

    List<Character> getAllAlive()
    {
        List<Character> e = new List<Character>();
        foreach (Character ct in characters)
        {
            if (ct.isAlive())
            {
                e.Add(ct);
            }
        }
        return e;
    }

    public List<Character> getCharacters()
    {

        return characters;
    }


    Character createCharacter(string load, string name)
    {
        Character c = loadCharacter(load);
        if (c != null)
        {
            if (name == "")
            {
                name = c.getName();
            }
            c.setGame(this, name);
        }
        return c;
    }


    string replaceChecks(Character target, Character source, string text)
    {
        string txx = "";
        int loop = 100;

        txx = "accuracytriggercheck";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            int boolInt = (source.getAccCheck().Contains(target)) ? 1 : 0;
            text = text.Replace(txx, boolInt.ToString());
        }

        txx = "enemyalive";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            int numnt = getEnemyAlive(source).Count;
            text = text.Replace(txx, numnt.ToString());
        }

        txx = "enemydead";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            int numnt = getEnemyTeam(source).Count - getEnemyAlive(source).Count;
            text = text.Replace(txx, numnt.ToString());
        }

        txx = "allyalive";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            int numnt = getAllyAlive(source).Count;
            text = text.Replace(txx, numnt.ToString());
        }


        txx = "allydead";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            int numnt = getAllyTeam(source).Count - getAllyAlive(source).Count;
            text = text.Replace(txx, numnt.ToString());
        }

        return text;
    }



    public bool isClass(Character c, string s)
    {
        return c.isClass(s);
    }

    string singleCheckVar(string txx, string text,Character c, string result)
    {
        txx += "[";
        int iii = text.IndexOf(txx);
        int iii2 = iii + txx.Length;
        string s0 = text.Substring(iii2);
        int iii4 = s0.IndexOf("]");
        string s1 = text.Substring(iii2, iii4);
        int totalLen = txx.Length + s1.Length + 1;
        text = text.Substring(0, iii) + result + text.Substring(iii + totalLen);
        return text;
    }

    string getSingleCheckVar(string txx, string text, Character c)
    {
        txx += "[";
        int iii = text.IndexOf(txx);
        int iii2 = iii + txx.Length;
        string s0 = text.Substring(iii2);
        int iii4 = s0.IndexOf("]");
        return text.Substring(iii2, iii4);
    }

    string replaceCheckVars(Character target, Character source, string text)
    {
        int loop = 100;

        string txx;

        txx = "targetisclass";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            string check = getSingleCheckVar(txx, text, target);
            int boolInt = (target.isClass(check) ? 1 : 0);
            text = singleCheckVar(txx, text, target, boolInt.ToString());
        }

        txx = "sourceisclass";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            string check = getSingleCheckVar(txx, text, target);
            int boolInt = (source.isClass(check) ? 1 : 0);
            text = singleCheckVar(txx, text, target, boolInt.ToString());
        }


        txx = "targethasdebuff";
        while (loop > 0 && text.Contains(txx))
        {
            loop--;
            string check = getSingleCheckVar(txx, text, target);
            int boolInt = (target.hasDebuff(check) ? 1 : 0);
            text = singleCheckVar(txx, text, target, boolInt.ToString());

        }


        return text;
    }

    public string replaceString(Character target, Character own, string text)
    {
        //string sss = own.getBaseStat("health").ToString();
        //text = text.Replace("ownbasestat[health", "y");

        text = replaceCheckVars(target, own, text);
        text = replaceChecks(target, own, text);
        text = text.Replace("ownbasestat[maxhealth]", own.getBaseStat("maxhealth").ToString());
        return text;
    }

    string expressionSolver2(string s)
    {
        string temp = "";
        string groupTemp = "";
        bool capturing = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(')
            {
                capturing = true;
            }
            else if (s[i] == ')')
            {
                string add = "";
                temp += add;
                capturing = false;
            }
            else
            {
                if (capturing)
                {
                    groupTemp += s[i];
                }
                else
                {
                    temp += s[i];
                }
            }
        }
        //1+ (3+2)

        /*Regex rx = new Regex(@"^\((.*)\)$");
        string text = "(hi there !)(wdwdwdwdwd)";
        MatchCollection matches = rx.Matches(text);

        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;

        }*/

        return temp;
    }

    string expressionSolver(string s)
    {
        var v = datat.Compute(s, "");


        return v.ToString();
    }

    public float parseNum(Character target, Character own, string text)
    {

        float number;

        if (float.TryParse(text, out number))
        {
            return number;
        }

       

        if (text == null || text == "")
        {
            return 0.0f;
        }

        string rep = replaceString(target, own, text);
        if (rep == "")
        {
            return 0.0f;
        }

        if (float.TryParse(text, out number))
        {
            return number;
        }

        if (Regex.IsMatch(rep, @"^([0-9]|\(|\)|=|<|>|\+|\-|\*|\/|\s|\.)*$"))
        {
            string solved = "0";
            float n;
            if (Regex.IsMatch(rep, @"^([0-9]|\.)*$"))
            {
                solved = rep;
                n = float.Parse(solved);
            }
            else
            {

                solved = expressionSolver(rep);
                n = float.Parse(solved);
            }

            
            float result = (Mathf.Round(n * 100.0f)) / 100.0f;
            return result;
        }

        return 0;
    }

    public string parseType(Character target, Character own, string text)
    {
        string rep = replaceString(target, own, text);
        return rep;
    }


    void activatePassives()
    {
        foreach (Character c in characters)
        {
            c.activatePassives();
        }
    }

    public Transform getAnimationParent()
    {
        return doll.getAnimationParent();
    }


    public Transform getDoll(Character c)
    {
        return doll.getDoll(getIndexFromCharacter(c));
    }

    public Card loadCard(string name)
    {
        Card card = save.loadCard(name);
        return card;
    }

    public Deck createDeck(DictionaryOfStringAndString de, DictionaryOfStringAndString pa, Character c)
    {
        Deck d = new Deck(c);
        foreach (var key in de.Keys)
        {
            int num = (int)Mathf.Round(parseNum(c, c, de[key]));
                Card card = loadCard(key);
                if (card != null)
                {
                    d.addCard(card,num);

                    //Debug.Log("Card " + key + " added");
                }
                else
                {
                    //Debug.Log("Card "+ key + " not found");
                }
        }

        foreach (var key in pa.Keys)
        {
            int num = (int)Mathf.Round(parseNum(c, c, pa[key]));
            Card card = loadCard(key);
            if (card != null)
            {
                d.addPassive(card, num);
                //Debug.Log("Card " + key + " added");
            }
            else
            {
                //Debug.Log("Card "+ key + " not found");
            }
        }
        return d;
    }


    void makeTwoTeams()
    {
        teams = new List<Team>();
        Team t0 = new Team("player");
        teams.Add(t0);

        Team t1 = new Team("enemy");
        teams.Add(t1);
    }
}
