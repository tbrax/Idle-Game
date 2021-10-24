using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameLogic : MonoBehaviour
{

    public Camera ccamera;

    Character current;
    Character target;
    Card currentCard;

    SaveData save = new SaveData();
    int turnSpeedPos = 0;

    public Transform mainText;

    public List<Character> characters = new List<Character>();

    List<Character> characterOrder = new List<Character>();

    public List<Team> teams = new List<Team>();
    DictionaryOfStringAndString gameStats = new DictionaryOfStringAndString();

    List<Transform> buttonList =  new List<Transform>();

    public Transform showCards;


    public Transform targetSelect;
    public Transform turnSelect;

    float gameTimer;
    int mode = 0;
    public Transform cardDisplay;

    //bool gameLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        //setPlayerGame();
        makeTwoTeams();
        makeTestCharacters();

        loadGame();
        //makeButtons();

        Debug.Log(Application.persistentDataPath);
    }

    public void loadGame()
    {
        //gameLoaded = true;
        startGame();
    }

    public void updateMainInterface()
    {
        string tx = current.getName() + " Turn"+"\n"+"Round "+getGameStat("round");

        mainText.GetComponent<TMPro.TextMeshProUGUI>().text = tx;
    }

    public void setGameStat(string stat, string val)
    {
        gameStats[stat] = val;
    }

    public string getGameStat(string stat)
    {
        if (!gameStats.ContainsKey(stat))
        {
            return null;
        }

        return gameStats[stat];
    }

    public void setMode(int i)
    {
        mode = i;
    }

    public void getCharacterTurn()
    {
        current = characterOrder[turnSpeedPos];

        int ct = 0;
        while(!current.canFight())
        {
            ct += 1;
            if(ct > characterOrder.Count)
            {

                Debug.Log("No characters can fight");
                return;
            }
            increasePos();
            current = characterOrder[turnSpeedPos];
        }

    }

    public void startGame()
    {
        setGameStat("round", "0");

        startRound();
        startTurn();
    }

    public void endTurn()
    {
        increasePos();
        startTurn();
    }

    public void startTurn()
    {
        getCharacterTurn();


        characterTurnSelect(current);
        characterSelect(current);

        makeButtons(current, current.getHand());


        updateMainInterface();  
    }

    public void increasePos()
    {
        turnSpeedPos += 1;
        if (turnSpeedPos >= characterOrder.Count)
        {
            turnSpeedPos = 0;
            endRound();
        }
    }

    public void endRound()
    {
        startRound();
    }

    public void startRound()
    {
        string s = getGameStat("round");
        string n = (Convert.ToInt32(s)+1).ToString();
        setGameStat("round", n);

        calcOrder();
    }

    public void calcOrder()
    {
        turnSpeedPos = 0;
        characterOrder = new List<Character>(characters);
        characterOrder.Sort(sortBySpeed);

    }


    static int sortBySpeed(Character p0, Character p1)
    {
        int c = p0.getBaseStat("speed").CompareTo(p1.getBaseStat("speed"));
        if (c == 0)
        {
            c = p0.luckRoll().CompareTo(p1.luckRoll());
        }
        if (c == 0)
        {
            float f = UnityEngine.Random.Range(0.0f, 1.0f);
            if (f > 0.5)
            {
                c = -1;
            }
            else
            {
                c = 1;
            }
        }
        return c;
    }


    public void characterTurnSelect(Character c)
    {
        
        //Vector2 v = c.getDisplay().getCharacterPosition();
        //turnSelect.position = v;
    }


    public void characterSelect(Character c)
    {

        target = c;
        //Vector2 v = c.getDisplay().getCharacterPosition();
        //targetSelect.position = v;
    }

    void displayCards(List<Card> cards)
    {

    }

    void makeTwoTeams()
    {
        Team t0 = new Team("player");
        teams.Add(t0);

        Team t1 = new Team("enemy");
        teams.Add(t1);

    }

    void makeTestCharacters()
    {
        createCharacter("Jackson","ch0");
        createCharacter("Jackson", "ch1");
        createCharacter("Jackson", "ch2");
        createCharacter("Jackson", "ch3");
        createCharacter("Jackson", "ch4");
        createCharacter("Jackson", "ch5");
        teams[0].addCharacter(characters[0]);
        teams[0].addCharacter(characters[1]);
        teams[0].addCharacter(characters[2]);
        teams[1].addCharacter(characters[3]);
        teams[1].addCharacter(characters[4]);
        teams[1].addCharacter(characters[5]);

    }

    public Vector2 getCharacterPos(Character ch)
    {
        if (ch.getTeam() != null)
        {
            int it = teams.IndexOf(ch.getTeam());
            int ic = ch.getTeam().getIndex(ch);
            float xsep = 0.10f;
            float ysep = 0.15f;
            Vector2 pos = new Vector2(0.0f, 0.7f);
            Vector2 addPos = new Vector2(0.0f, 0.0f);

            int rowSize = 2;
            int row = (int)(ic / rowSize);
            int column = ic % rowSize;


            addPos.y += ysep * row;
            if (it == 0)
            {
                pos.x = 0.1f;
                addPos.x += xsep * column;
            }
            else if (it == 1)
            {
                pos.x = 0.9f;
                addPos.x -= xsep * column;
            }
            return getCamCoords(pos+addPos);
        }
        return new Vector2(0.0f, 0.0f);
        //return pos;
    }

    public Vector2 getCamCoords(Vector2 v)
    {

        Vector3 p = ccamera.ViewportToWorldPoint(new Vector3(v.x, v.y, ccamera.nearClipPlane));
        Vector2 ret = new Vector2(p.x, p.y);
        return ret;
    }



    public void createCharacter(string load,string name)
    {
        Character c = loadCharacter(load);

        //c.setGame(this, name);
        characters.Add(c);

    }

    public Character getCharacter(int index)
    {
        return characters[index];
    }

    public Vector2 getChaPos(Character c)
    {
        return new Vector2(0.0f, 0.0f);
    }

    public Deck createDeck(DictionaryOfStringAndString de,Character c)
    {
        Deck d = new Deck(c);
        foreach (var key in de.Keys)
        {
            int num = (int)Mathf.Round(parseNum(c,c, de[key]));
            for (int i = 0; i < num; i++) //Error messages on this line
            {
                
                Card card = save.loadCard(key);
                if (card != null)
                {
                    d.addCard(card,num);
                }
                else
                {
                    //Debug.Log("Card "+ key + " not found");
                }
            }

        }
        return d;
    }

    public void saveCharacter(Character c, string name)
    {
        save.saveCharacterJSON(c, name);
    }

    Character loadCharacter(string name)
    {
        return save.loadCharacter(name,0);
    }




    public float parseNum(Character target, Character own, string num)
    {
        return float.Parse(num);
    }


    void makeTestPlayers()
    {
        Character c0 = new Character();
        Character c1 = new Character();
    }

    public void saveTempCharacter()
    {
        saveCharacter(current, "Jack1");
    }

    public void saveTempCard()
    {
        saveCardButton();
    }





    public void saveCardButton()
    {
        if (currentCard != null)
        {
            saveCard(currentCard, "Fire Blast");
        }
    }

    public void saveCard(Card c,string name)
    {
       save.saveCardJSON(c, name);
    }

    public Card loadCard(string cardName)
    {
        return save.loadCard(cardName);
    }

    public Transform makeButtonTransform()
    {
        Transform doll = Resources.Load<Transform>("Prefabs/Button0");

        return doll;
    }


    public void clickButton(int i)
    {
        Debug.Log(i);
    }



    public void clickChaButton(Character c, Card ch)
    {
        viewCard(c, ch);

    }

    public void viewCard(Character c, Card ch)
    {
        currentCard = ch;
        cardDisplay.gameObject.SetActive(true);

        Transform titleText = cardDisplay.Find("cardObj/nameText");
        titleText.GetComponent<TMPro.TextMeshProUGUI>().text = ch.getName();

        Transform descText = cardDisplay.Find("cardObj/descText");
        descText.GetComponent<TMPro.TextMeshProUGUI>().text = ch.getDesc();

    }




    public void useWithCost(Character c, Character tar, Card ch, int choice)
    {
        if (ch.canAfford(c,choice))
        {
            ch.takeCost(c, choice);
            cardUseHand(c, tar, ch);
            ch = null;
            cardDisplay.gameObject.SetActive(false);
        }
    }

    public void costWindow(Character c, Character tar, Card ch)
    {
        //List<DictionaryOfStringAndString> costList = ch.getCost();
    }

    public void costCheck(Character c, Character tar, Card ch)
    {

            useWithCost(c, tar, ch, 0);
        
    }




    public void useButton()
    {
        if (currentCard != null)
        {
            costCheck(current, target,currentCard);
        }
    } 

    public void cardUseHand(Character ca, Character tar, Card ch)
    {
        cardUse(ca, tar, ch);

    }



    public void cardUse(Character ca, Character tar,Card ch)
    {


        ca.useCard(ch, tar);

    }

    public void removeButtons()
    {
        foreach (Transform t in buttonList)
        {
            if (t != null)
            {
                Destroy(t.gameObject);
            }
            
        }

    }
    public void makeButtons(Character c,List<Card> cds)
    {
        removeButtons();

        for (var i =0;i< cds.Count; i++)
        {
            Transform but = GameObject.Instantiate(makeButtonTransform(), new Vector2(0, 0), Quaternion.identity);
            
            for (int i1 = 0; i1 < but.transform.childCount; i1++)
            {
                if (but.transform.GetChild(i1).transform.name == "Text")
                {
                    but.transform.GetChild(i1).transform.GetComponent<TMPro.TextMeshProUGUI>().text = cds[i].getName();
                }
            }


            buttonList.Add(but);
            but.SetParent(showCards);
            int tempInt = i;
            Button myselfButton = but.GetComponent<Button>();
            Character c2 = c;
            Card car2 = cds[i];
            //myselfButton.onClick.AddListener(() => { clickButton(tempInt); });
            myselfButton.onClick.AddListener(() => { clickChaButton(c2,car2); });
        }


    }

        

    void updateTest()
    {


        foreach (Character c in characters)
        {
            c.updateInterface();
        }

            
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;
        updateTest();
    }
}
