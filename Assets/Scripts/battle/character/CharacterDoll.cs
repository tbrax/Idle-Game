using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterDoll
{
    public List<GameObject> cDolls = new List<GameObject>();


    public List<Transform> history = new List<Transform>();

    public GameObject dataWindow;
    public GameObject charWindow;
    public GameObject helpWindow;

    public GameObject selectWindow;

    public GameObject bat;
    public BattleLogic logic;

    public Transform timerShow;
    public Transform fpsShow;

    List<GameObject> statDataCells = new List<GameObject>();

    List<List<string>> statDataValues = new List<List<string>>();
    //List<List<string>> resistDataValues = new List<List<string>>();
    int statViewing = 0;


    List<Transform> charSpotsAlly = new List<Transform>();
    List<Transform> charSpotsEnemy = new List<Transform>();
    List<string> loadedImages;


    int selectedCharacterSpot = 0;
    List<string> statBaseKeyOrder;

    List<string> loadNamesAlly;
    List<string> loadIDAlly;
    List<int> loadNumAlly;

    List<string> loadNamesEnemy;
    List<string> loadIDEnemy;
    List<int> loadNumEnemy;


    int loadTeam = 0;


    DictionaryOfStringAndString dicBox;

    int avgFrameRate;


    List<Transform> spinList;

    public CharacterDoll(GameObject battle, BattleLogic l)
    {
        setupObjects(battle, l);
    }

    void makeCharacterList(List<string> names)
    {

    }




    public Transform getDoll(int i)
    {
        return cDolls[i].transform;
    }

    List<string> getConstData(string name)
    {
        return logic.getConstData(name);
    }


    Character getCharFromIndex(int i)
    {
        return logic.getCharacter(i);
    }

    public void updateDollPos()
    {
        List<Character> cs = logic.getCharacters();

        for(int i = 0; i < cs.Count; i++)
        {
            if (cDolls.Count > i)
            {
                Vector2Int v0 = cs[i].getPos();


                Vector2 v1 = logic.getSquarePos(v0.x, v0.y);

                cDolls[i].GetComponent<RectTransform>().position = v1;
            }
        }
    }

    void selectCharacterLocation(int i)
    {
        selectedCharacterSpot = i;

        if (loadTeam == 0)
        {
            selectWindow.transform.Find("selectIcon").position = charSpotsAlly[i].position;
        }
        else if (loadTeam == 1)
        {
            selectWindow.transform.Find("selectIcon").position = charSpotsEnemy[i].position;
        }

        

    }

    void clickSelectChar(string id,string name)
    {
        if (loadTeam == 0)
        {
            for (int i = 0; i < loadNumAlly.Count; i++)
            {
                if (loadNumAlly[i] == selectedCharacterSpot)
                {
                    loadNumAlly.RemoveAt(i);
                    loadNamesAlly.RemoveAt(i);
                    loadIDAlly.RemoveAt(i);
                }
            }
            loadNamesAlly.Add(name);
            loadIDAlly.Add(id);
            loadNumAlly.Add(selectedCharacterSpot);
            charSpotsAlly[selectedCharacterSpot].Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = name;
        }
        else if (loadTeam == 1)
        {
            for (int i = 0; i < loadNumEnemy.Count; i++)
            {
                if (loadNumEnemy[i] == selectedCharacterSpot)
                {
                    loadNumEnemy.RemoveAt(i);
                    loadNamesEnemy.RemoveAt(i);
                    loadIDEnemy.RemoveAt(i);
                }
            }
            loadNamesEnemy.Add(name);
            loadIDEnemy.Add(id);
            loadNumEnemy.Add(selectedCharacterSpot);
            charSpotsEnemy[selectedCharacterSpot].Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = name;
        }


    }

    void confirmSelectCharacter()
    {

        bool s = logic.loadAndSetup(loadIDAlly, loadNumAlly, loadIDEnemy, loadNumEnemy);
        closeCharacterSelectBox();
    }

    void openCharacterSelectBox()
    {
        CanvasGroup g0 = selectWindow.GetComponent<CanvasGroup>();
        g0.alpha = 1;
        g0.interactable = true;
        g0.blocksRaycasts = true;
        //logic.setPause(true);
    }

    void closeCharacterSelectBox()
    {
        CanvasGroup g0 = selectWindow.GetComponent<CanvasGroup>();
        g0.alpha = 0;
        g0.interactable = false;
        g0.blocksRaycasts = false;
        //logic.setPause(false);
    }


    void makeCharacterSelectWindow()
    {
        selectedCharacterSpot = 0;
        List<string> chaListID = getConstData("battlecharacterid");
        List<string> chaList = getConstData("battlecharacternames");


        Transform statContent = selectWindow.transform.Find("nameWindow").Find("GridMenu").Find("Viewport").Find("Content");
        removeChildren(statContent);

        //statDataCells = new List<GameObject>();

        GameObject buildBtn = Resources.Load<GameObject>("UI/CharBtn") as GameObject;
        for (int i = 0; i < chaList.Count; i++)
        {
            GameObject buildTemp = GameObject.Instantiate(buildBtn);
            buildTemp.transform.SetParent(statContent);
            RectTransform rt = buildTemp.GetComponent<RectTransform>();
            rt.transform.localScale = new Vector2(1.0f, 1.0f);
            statDataCells.Add(buildTemp);
            Button bc = buildTemp.transform.Find("Button").GetComponent<Button>();

            string nameID = chaList[i];
            buildTemp.transform.Find("Name").Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = nameID;

            int tempInt = i;


            string charID = chaListID[i];
            bc.onClick.AddListener(delegate { clickSelectChar(charID, nameID); });

            //GameObject bdi = buildTemp.transform.Find("Image").gameObject;
            //bdi.GetComponent<Image>().sprite = data.getUIcon(i);

            //bdi.GetComponent<Image>().SetAsLastSibling();

        }
        ScrollRect scroll = selectWindow.transform.Find("nameWindow").Find("GridMenu").GetComponent<ScrollRect>();

        Canvas.ForceUpdateCanvases();
        scroll.verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
        toggleSelectTeamHide();
    }

    void toggleSelectTeam()
    {
        if (loadTeam == 1)
        {
            loadTeam = 0;
        }
        else if (loadTeam == 0)
        {
            loadTeam = 1;
        }

        //selectWindow.transform.Find("teamSwitch").Find("button").GetComponent<Button>().onClick.AddListener(delegate { toggleSelectTeam(); });
        toggleSelectTeamHide();
        //Debug.Log(loadTeam);
    }

    void toggleSelectTeamHide()
    {
        CanvasGroup g0 = selectWindow.transform.Find("chars").Find("ally").GetComponent<CanvasGroup>();
        CanvasGroup g1 = selectWindow.transform.Find("chars").Find("enemy").GetComponent<CanvasGroup>();

        CanvasGroup s0 = selectWindow.transform.Find("teamSwitch").Find("ally").GetComponent<CanvasGroup>();
        CanvasGroup s1 = selectWindow.transform.Find("teamSwitch").Find("enemy").GetComponent<CanvasGroup>();

        if (loadTeam == 0)
        {
            g0.alpha = 1;
            g0.interactable = true;
            g0.blocksRaycasts = true;
            g1.alpha = 0;
            g1.interactable = false;
            g1.blocksRaycasts = false;

            s0.alpha = 1;
            s0.interactable = true;
            s0.blocksRaycasts = true;
            s1.alpha = 0;
            s1.interactable = false;
            s1.blocksRaycasts = false;

        }
        else if (loadTeam == 1)
        {
            g1.alpha = 1;
            g1.interactable = true;
            g1.blocksRaycasts = true;
            g0.alpha = 0;
            g0.interactable = false;
            g0.blocksRaycasts = false;
            s1.alpha = 1;
            s1.interactable = true;
            s1.blocksRaycasts = true;
            s0.alpha = 0;
            s0.interactable = false;
            s0.blocksRaycasts = false;
        }
    }

    void setupSelectWindow(GameObject battle, BattleLogic l)
    {

        loadNamesAlly = new List<string>();
        loadIDAlly = new List<string>();
        loadNumAlly = new List<int>();

        loadNamesEnemy = new List<string>();
        loadIDEnemy = new List<string>();
        loadNumEnemy = new List<int>();


        selectWindow = battle.transform.Find("windows").Find("selectCharacter").gameObject;

        selectWindow.transform.Find("confirm").Find("button").GetComponent<Button>().onClick.AddListener(delegate { confirmSelectCharacter(); });
        selectWindow.transform.Find("teamSwitch").Find("button").GetComponent<Button>().onClick.AddListener(delegate { toggleSelectTeam(); });

        //selectCharacterLocation();
        charSpotsAlly = new List<Transform>();
        charSpotsEnemy = new List<Transform>();
        foreach (Transform child in selectWindow.transform.Find("chars").Find("ally"))
        {
            charSpotsAlly.Add(child);

        }
        foreach (Transform child in selectWindow.transform.Find("chars").Find("enemy"))
        {
            charSpotsEnemy.Add(child);

        }


        for (int i = 0; i < charSpotsAlly.Count; i++)
        {
            int tm = i;
            charSpotsAlly[i].Find("button").GetComponent<Button>().onClick.AddListener(delegate { selectCharacterLocation(tm); });
        }
        for (int i = 0; i < charSpotsEnemy.Count; i++)
        {
            int tm = i;
            charSpotsEnemy[i].Find("button").GetComponent<Button>().onClick.AddListener(delegate { selectCharacterLocation(tm); });
        }

        makeCharacterSelectWindow();
    }

    void openCharacterSelect()
    {
        openCharacterSelectBox();
    }


    void setupAutoSpinList()
    {
        spinList = new List<Transform>();

        spinList.Add(dataWindow.transform.Find("skills").Find("0").Find("spin"));
        spinList.Add(dataWindow.transform.Find("skills").Find("1").Find("spin"));
        spinList.Add(dataWindow.transform.Find("skills").Find("2").Find("spin"));
    }

    void setupObjects(GameObject battle, BattleLogic l)
    {
        loadTeam = 0;
        bat = battle;
        logic = l;
        dataWindow = battle.transform.Find("windows").Find("data").gameObject;
        charWindow = battle.transform.Find("windows").Find("infoChar").gameObject;
        helpWindow = battle.transform.Find("windows").Find("help").gameObject;
        setupSelectWindow(battle, l);
        helpWindow.transform.Find("button").GetComponent<Button>().onClick.AddListener(delegate { closeHelpBox(); });
        battle.transform.Find("globalButtons").Find("pause").GetComponent<Button>().onClick.AddListener(delegate { logic.togglePause(); });
        battle.transform.Find("globalButtons").Find("team").Find("button").GetComponent<Button>().onClick.AddListener(delegate { openCharacterSelect(); });
        timerShow = battle.transform.Find("globalButtons").Find("time");
        fpsShow = battle.transform.Find("globalButtons").Find("fps");
        closeSubWindows();
        dataWindow.transform.Find("charInfoButton").GetComponent<Button>().onClick.AddListener(delegate { charInfoWindowOpen(); });
        charWindow.transform.Find("otherButtons").Find("return").Find("button").GetComponent<Button>().onClick.AddListener(delegate { charDataWindowOpen(); });
        charWindow.transform.Find("otherButtons").Find("viewStats").GetComponent<Button>().onClick.AddListener(delegate { openWindowStat(); });
        charWindow.transform.Find("otherButtons").Find("viewResist").GetComponent<Button>().onClick.AddListener(delegate { openWindowResist(); });
        charWindow.transform.Find("otherButtons").Find("viewBuffs").GetComponent<Button>().onClick.AddListener(delegate { openWindowBuff(); });
        makeStatBaseKeyOrder();
        List<GameObject> c0s = getChildren(battle.transform.Find("arena").Find("teams").Find("ally").gameObject);
        List<GameObject> c1s = getChildren(battle.transform.Find("arena").Find("teams").Find("enemy").gameObject);

        List<GameObject> h0s = getChildren(battle.transform.Find("arena").Find("history").Find("ally").gameObject);
        List<GameObject> h1s = getChildren(battle.transform.Find("arena").Find("history").Find("enemy").gameObject);

        setupAutoSpinList();
        loadedImages = new List<string>();
        foreach ( GameObject g in c0s)
        {
            cDolls.Add(g);
            loadedImages.Add("");
        }
        foreach (GameObject g in c1s)
        {
            cDolls.Add(g);
            loadedImages.Add("");
        }

        foreach (GameObject g in h0s)
        {
            history.Add(g.transform);
        }
        foreach (GameObject g in h1s)
        {
            history.Add(g.transform);
        }


        hideCharacters();
    }


    public void restartDoll()
    {
        closeCharacterSelectBox();
        removeSoundsAnimations();
        healthDieReset();
    }

    public void healthDieChange(int i)
    {
        if (cDolls.Count > i)
        {
            CanvasGroup g0 = cDolls[i].transform.Find("death").GetComponent<CanvasGroup>();
            g0.alpha = 1;
        }      
    }

    public void healthDieReset()
    {
        for (int i = 0; i < cDolls.Count;i++)
        {
            CanvasGroup g0 = cDolls[i].transform.Find("death").GetComponent<CanvasGroup>();
            g0.alpha = 0;
        }
    }

    void loadImage(Character c, int spot)
    {
        Sprite img = Resources.Load<Sprite>("Battle/images/" + c.getImageID());
        img.texture.filterMode = FilterMode.Point;
        if (img != null)
        {
            cDolls[spot].transform.Find("Image").GetComponent<Image>().sprite = img;
        }
        

    }

    public void timerS(float f)
    {
        timerShow.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = f.ToString("F2"); ;


    }

    void makeStatBaseKeyOrder()
    {
        statBaseKeyOrder = new List<string>();

        statBaseKeyOrder.Add("health");
        statBaseKeyOrder.Add("maxhealth");
        statBaseKeyOrder.Add("speed");
        statBaseKeyOrder.Add("damage");
        statBaseKeyOrder.Add("accuracy");
        statBaseKeyOrder.Add("dodge");
    }

    public void openHelpBox(string s)
    {
        CanvasGroup g0 = helpWindow.GetComponent<CanvasGroup>();
        g0.alpha = 1;
        g0.interactable = true;
        g0.blocksRaycasts = true;
        helpWindow.transform.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = s;
    }

    void closeHelpBox()
    {
        CanvasGroup g0 = helpWindow.GetComponent<CanvasGroup>();
        g0.alpha = 0;
        g0.interactable = false;
        g0.blocksRaycasts = false;
    }


    public void fpsUpdate()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        string s = avgFrameRate.ToString() + " FPS";
        fpsShow.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = s;
    }


    void clickStat(int i)
    {
        string s = "";
        string s0 = statDataValues[i][0];
        string s1 = statDataValues[i][1];
        if (statViewing == 0)
        {
            switch (s0)
            {
                case "health":
                    s = "When health reaches 0, the character can no longer fight";
                    break;
                case "maxhealth":
                    s = "The maximum health of the character";
                    break;
                case "accuracy":
                    s = "Bonus to hitting skills against enemies";
                    break;
                case "speed":
                    s = "Increases how fast skills refresh";
                    break;
                case "dodge":
                    s = "Base chance of avoiding skills from enemies";
                    break;
                case "critchance":
                    s = "Base chance of how likely damage will critically strike";
                    break;
                case "critdamage":
                    s = "How much extra damage a critical hit will inflict";
                    break;
                case "critdamageresist":
                    s = "Reduces how much damage critical hits will inflict";
                    break;
                case "critchanceresist":
                    s = "Reduces how likely a critical hit will be inflicted against the user";
                    break;
                case "damage":
                    s = "How much extra damage the user will deal";
                    break;
                default:
                    s = s0;
                    break;

            }
        }
        else if (statViewing == 1)
        {
            float r = logic.parseNum(logic.getSelectedCharacter(), logic.getSelectedCharacter(), s1);
            s += "The character ";
            if (r < 0)
            {
                s += "takes " + (-r).ToString() + "% more damage"; 
            }
            else if(r == 0)
            {
                s += "takes full damage";
            }
            else if (r > 0 && r < 100)
            {
                s += "takes " + (r).ToString() + "% less damage";
            }
            else if (r == 100)
            {
                s += "takes no damage";
            }
            else
            {
                s += "heals " + (r-100).ToString() + "% of damage";
            }
            s += " from " + getNiceText(s0) + " damage sources";
        }
        else if (statViewing == 2)
        {
            if (dicBox.ContainsKey(s0))
            {
                s = dicBox[s0];
            }
            
        }
        openHelpBox(s);
    }

    void openSubWindowData()
    {
        CanvasGroup g0 = dataWindow.GetComponent<CanvasGroup>();
        g0.alpha = 1;
        g0.interactable = true;
        g0.blocksRaycasts = true;
    }

    void openSubWindowChar()
    {
        CanvasGroup g0 = charWindow.GetComponent<CanvasGroup>();
        g0.alpha = 1;
        g0.interactable = true;
        g0.blocksRaycasts = true;
    }


    public void closeSubWindows()
    {
        CanvasGroup g0 = dataWindow.GetComponent<CanvasGroup>();
        CanvasGroup g1 = charWindow.GetComponent<CanvasGroup>();
        g0.alpha = 0;
        g0.interactable = false;
        g0.blocksRaycasts = false;
        g1.alpha = 0;
        g1.interactable = false;
        g1.blocksRaycasts = false;
        closeCharacterSelectBox();
    }

    void removeChildren(Transform o)
    {
        foreach (Transform child in o)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public void amimationUpdate(float time)
    {

        fpsUpdate();
        spinAuto(time);
    }

    void spinAuto(float time)
    {
        float sp = 180 * time;
        foreach (Transform t in spinList)
        {
            t.Rotate(0,0,sp);
        }
    }

    public void updateSpinAuto(Character c)
    {
        for (int i = 0; i < spinList.Count; i++)
        {
            CanvasGroup g1 = spinList[i].GetComponent<CanvasGroup>();

            if (c.getAutoUse(i))
            {
                g1.alpha = 1;
                g1.interactable = true;
                g1.blocksRaycasts = true;
            }
            else
            {
                g1.alpha = 0;
                g1.interactable = false;
                g1.blocksRaycasts = false;
            }
        }
        
    }

    void updateStatBtns(Character c)
    {

        StatBlock st = c.getStats();
        DictionaryOfStringAndString dic;

        if (statViewing == 1)
        {
            dic = st.getResistDict();
        }
        else if (statViewing == 2)
        {
            dicBox = st.getBuffsDict();
            dic = st.getBuffsTimeDict();
            //openWindowBuff();
        }
        else
        {
            dic = st.getStatsDict();
        }

        bool wantReset = false;
        foreach (KeyValuePair<string, string> s0 in dic)
        {
            bool found = false;
            foreach (List<string> s1 in statDataValues)
            {
                if (s1[0] == s0.Key)
                {
                    s1[1] = s0.Value;
                    found = true;
                }
            }
            if (!found)
            {
                wantReset = true;
                break;
            }
        }
        int ct = dic.Count;
        if (ct != statDataCells.Count)
        {
            wantReset = true;
        } 
        if (wantReset)
        {
            List<List<string>> ss = new List<List<string>>();
            foreach (KeyValuePair<string, string> s0 in dic)
            {
                List<string> s1 = new List<string>();
                s1.Add(s0.Key);
                s1.Add(s0.Value);
                ss.Add(s1);
            }
            statDataValues = ss;
            buildStatBtns(c, ct);

            foreach (KeyValuePair<string, string> s0 in dic)
            {
                foreach (List<string> s1 in statDataValues)
                {
                    if (s1[0] == s0.Key)
                    {
                        s1[1] = s0.Value;
                    }
                }
            }
            sortStatDataValues();
        }
        updateStatBtnsText();
    }

    void sortStatDataValues()
    {
        List<List<string>> newS = new List<List<string>>();

        for (int j = 0; j < statBaseKeyOrder.Count; j++)
        {
            for ( int i = 0; i < statDataValues.Count; i++)
            {
            
                if (statDataValues[i][0] == statBaseKeyOrder[j])
                {
                    List<string> tmp = new List<string>();
                    tmp.Add(statDataValues[i][0]);
                    tmp.Add(statDataValues[i][1]);
                    newS.Add(tmp);
                }
            }
        }

        for (int i = 0; i < statDataValues.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < newS.Count; j++)
            {

                if (statDataValues[i][0] == newS[j][0])
                {
                    found = true;
                }
            }

            if (!(found))
            {
                List<string> tmp = new List<string>();
                tmp.Add(statDataValues[i][0]);
                tmp.Add(statDataValues[i][1]);
                newS.Add(tmp);

            }
        }

        statDataValues = newS;

    }


    void openWindowStat()
    {
        statViewing = 0;
        updateStatBtns(logic.getSelectedCharacter());
    }
    void openWindowBuff()
    {
        statViewing = 2;
        updateStatBtns(logic.getSelectedCharacter());
    }
    void openWindowResist()
    {
        statViewing = 1;
        updateStatBtns(logic.getSelectedCharacter());
    }


    string getNiceText(string s)
    {
        string s0 = s;

        if (s0.Contains("["))
        {
            s0 = s0.Substring(0,s0.IndexOf("["));
        }

        if (statViewing == 0)
        {           
            if (s == "health")
            {
                s0 = "Current Health";
            }
            else if (s == "maxhealth")
            {
                s0 = "Max Health";
            }
            else if (s == "speed")
            {
                s0 = "Speed";
            }
            else if (s == "accuracy")
            {
                s0 = "Accuracy";
            }
            else if (s == "critchance")
            {
                s0 = "Critical Chance";
            }
            else if (s == "critdamage")
            {
                s0 = "Critical Damage";
            }
            else if (s == "critchanceresist")
            {
                s0 = "Critical Chance Resist";
            }
            else if (s == "critdamageresist")
            {
                s0 = "Critical Damage Resist";
            }
            else if (s == "damage")
            {
                s0 = "Damage";
            }
        }
        else if (statViewing == 1)
        {
            s0 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s0.ToLower());
        }


        return s0;
    }

    public Transform getAnimationParent()
    {
        return bat.transform.Find("arena").Find("animations");
    }

    void updateStatBtnsText()
    {

        for (int i = 0; i < statDataValues.Count;i++)
        {
            string s0 = getNiceText(statDataValues[i][0]);
            string s1 = statDataValues[i][1];
            statDataCells[i].transform.Find("Name").Find("text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s0;
            statDataCells[i].transform.Find("Value").Find("text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s1;

        }
    }

    void buildStatBtns(Character c, int num)
    {      
        Transform statContent = charWindow.transform.Find("skillWindow").Find("GridMenu").Find("Viewport").Find("Content");
        removeChildren(statContent);

        statDataCells = new List<GameObject>();

        GameObject buildBtn = Resources.Load<GameObject>("UI/StatBtn") as GameObject;
        for (int i = 0; i < num; i++)
        {
            GameObject buildTemp = GameObject.Instantiate(buildBtn);
            buildTemp.transform.SetParent(statContent);
            RectTransform rt = buildTemp.GetComponent<RectTransform>();
            //Vector2 pos = new Vector2(0.0f, -70.0f * i);
            rt.transform.localScale = new Vector2(1.4f, 1.4f);
            //infoMenuUpgrade.addButn(buildTemp);
            statDataCells.Add(buildTemp);
            Button bc = buildTemp.transform.Find("Button").GetComponent<Button>();
            int tempInt = i;
            //bc.onClick.AddListener(delegate { openMenuUpgrade(tempInt); });

            bc.onClick.AddListener(delegate { clickStat(tempInt); });

            //GameObject bdi = buildTemp.transform.Find("Image").gameObject;
            //bdi.GetComponent<Image>().sprite = data.getUIcon(i);

            //bdi.GetComponent<Image>().SetAsLastSibling();

        }
        ScrollRect scroll = charWindow.transform.Find("skillWindow").Find("GridMenu").GetComponent<ScrollRect>();

        Canvas.ForceUpdateCanvases();
        scroll.verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
    }


    public void charInfoWindowOpen()
    {
        closeSubWindows();
        Character c = logic.getSelectedCharacter();
        CanvasGroup g1 = charWindow.GetComponent<CanvasGroup>();
        g1.alpha = 1;
        g1.interactable = true;
        g1.blocksRaycasts = true;
        if (c != null)
        {
            updateStatBtns(logic.getSelectedCharacter());
        }
    }

    public void charDataWindowOpen()
    {
        closeSubWindows();
        Character c = logic.getSelectedCharacter();
        CanvasGroup g1 = dataWindow.GetComponent<CanvasGroup>();
        g1.alpha = 1;
        g1.interactable = true;
        g1.blocksRaycasts = true;
        if (c != null)
        {
            updateStatBtns(logic.getSelectedCharacter());
        }
    }


    public void characterStats(Character c)
    {
        GameObject win = charWindow;
    }

    Character getSelectedCharacter()
    {
        return logic.getSelectedCharacter();
    }

    void showCharacters()
    {
        foreach(GameObject o in cDolls)
        {
            CanvasGroup g1 = o.GetComponent<CanvasGroup>();
            g1.alpha = 1;
            g1.interactable = true;
            g1.blocksRaycasts = true;
        }

        foreach (Transform o in history)
        {
            CanvasGroup g1 = o.GetComponent<CanvasGroup>();
            g1.alpha = 1;
            g1.interactable = true;
            g1.blocksRaycasts = true;
        }

    }

    void hideCharacters()
    {
        foreach (GameObject o in cDolls)
        {
            CanvasGroup g1 = o.GetComponent<CanvasGroup>();
            g1.alpha = 0;
            g1.interactable = false;
            g1.blocksRaycasts = false;
        }

        foreach (Transform o in history)
        {
            CanvasGroup g1 = o.GetComponent<CanvasGroup>();
            g1.alpha = 0;
            g1.interactable = false;
            g1.blocksRaycasts = false;
        }
    }


    public void updateHistory()
    {
        List<List<string>> tx = logic.getHistory();

        if (tx != null)
        {
            for (int i = 0; i < tx.Count; i++)
            {
                if (tx[i].Count >= 3)
                {
                    string s;
                    s = tx[i][0];
                    history[i].Find("h0").Find("text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
                    s = tx[i][1];
                    history[i].Find("h1").Find("text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
                    s = tx[i][2];
                    history[i].Find("h2").Find("text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
                }
            }
        }
    }

    public void startBattle()
    {
        restartDoll();
        closeSubWindows();
        showCharacters();
    }


    public void updateCharacter(Character c, int place)
    {

        if (c ==getSelectedCharacter())
        {
            updateStatBtns(c);
        }

        if (cDolls.Count > place)
        {
            string s;
            s = c.getName();
            cDolls[place].transform.Find("name").GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;


            //float sc = c.parseNum(c,c,c.getBaseStat("health")) / c.parseNum(c, c, c.getBaseStat("maxhealth"));
            cDolls[place].transform.Find("hpbar").localScale = new Vector3(c.getScaleHealth(), 1, 1);
            //s = c.getBaseStat("health").ToString();
            //cDolls[place].transform.Find("health").GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;

            if (loadedImages[place] != c.getImageID())
            {
                loadImage(c, place);
            }

            updateHistory();

        } 
    }


    public void openCharacterData(Character c)
    {
        statViewing = 0;
        closeSubWindows();
        openSubWindowData();
        showCharacterData(c);
    }





    public void showCharacterData(Character c)
    {
        //closeSubWindows();
        //openSubWindowData();
        string s;
        s = c.getName();
        dataWindow.transform.Find("name").GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
        List<Card> cds = c.getDeck();
        for (int i = 0; i < 3; i++)
        {
            if (cds.Count > i)
            {
                GameObject skil = dataWindow.transform.Find("skills").GetChild(i).gameObject;
                s = cds[i].getName();
                skil.transform.Find("use").GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;


                float f = c.getCooldownPercent(i);
                skil.transform.Find("cool").GetComponent<Slider>().value = f;
                //s = c.getCooldownPercent(i).ToString("F2");
                //skil.transform.Find("time").GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;

            }
        }
    }

    public void removeSoundsAnimations()
    {
        List<GameObject> c = getChildren(getAnimationParent().gameObject);

        for (int i = c.Count-1; i >=0; i--)
        {
            GameObject.Destroy(c[i]);
        }
    }

    public List<GameObject> getChildren(GameObject g)
    {
        List<GameObject> childrens = new List<GameObject>();

        foreach (Transform child in g.transform)
        {
            if (!childrens.Contains(child.gameObject))
            {
                childrens.Add(child.gameObject);
            }
        }
        return childrens;
    }
}
