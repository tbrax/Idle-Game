using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using CustomHelpNameSpace;
using System.Numerics;

using Vector2 = UnityEngine.Vector3;


public class ButtonController : MonoBehaviour
{

    float runCards;
    float totalCards;

    float currentTimer = 0.0f;

    double addTimer = 0.0;


    public List<ScrollRect> scrollbars = new List<ScrollRect>();
    //BigInteger cardsSecond = 0;
    //BigInteger cardsCurrent = 0;
    //BigInteger cardsRunTotal = 0;
    // BigInteger cardsGrandTotal = 0;

    string secondAdd = " /s";

    public GameObject txtCurrent;
    public GameObject txtSecond;
    public GameObject sudokuUI;
    //public GameObject txtRunTotal;
    //public GameObject txtGrandTotal;

    public List<GameObject> txt = new List<GameObject>();

    public List<GameObject> gameWindows = new List<GameObject>();

    public List<GameObject> uiWindows = new List<GameObject>();

    public GameObject canvasBuildBtn;
    public GameObject canvasUpgradeBtn;
    public GameObject canvasMiniBtn;
    public GameObject UIbuildings;
    public GameObject UIupgrades;
    public GameObject UIsave;



    public GameObject UIBuildingBuyMenu;
    public GameObject UIUpgradeBuyMenu;

    public List<Sprite> buildingIconList = new List<Sprite>();
    public List<Sprite> upgradeIconList = new List<Sprite>();
    public List<Sprite> minigameIconList = new List<Sprite>();

    List<float> buildAmtList = new List<float>();

    Dictionary<string, float> numNameList = new Dictionary<string, float>();

    //int selectedUpgrade;

    Manip tm = new Manip();
    UIMenuBuilding infoMenuBuilding;
    UIMenuUpgrade infoMenuUpgrade;
    UIMenuMini infoMenuMini;

    ConstantData constData = new ConstantData();


    SaveUnit save = new SaveUnit();
    SaveData data;


    Sudoku sudok;
    BattleLogic battleArena;


    int lastWindow = -1;

    


    void Start()
    {
        data = new SaveData();
        //selectedUpgrade = -1;
        //cardsCurrent = new BigInteger(0);
        initialSetup();
        updateAmt();

    }

    public void clickCharacter(int i)
    {
       // scrollbars[2].normalizedPosition = new Vector2(0, 1);

        battleArena.clickCharacter(i);
    }


    void initialSetup()
    {

        int frameW = Screen.currentResolution.refreshRate;

        if (frameW > 30)
        {
            frameW = 30;
        }

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = frameW;

        UpgradeHold ut = new UpgradeHold(upgradeIconList);
        BuildingHold bt = new BuildingHold(buildingIconList);
        MiniHold mt = new MiniHold(minigameIconList);
        sudok = new Sudoku(sudokuUI);
        battleArena = new BattleLogic(data,constData, gameWindows[1]);

        infoMenuBuilding = new UIMenuBuilding(tm, secondAdd,data.getGlobalMult());
        infoMenuUpgrade = new UIMenuUpgrade(tm, secondAdd, data.getGlobalMult());

        infoMenuMini = new UIMenuMini(this, tm, secondAdd, data.getGlobalMult(), uiWindows, gameWindows);

        data.setUHold(ut);
        data.setBHold(bt);
        data.setMHold(mt);
        data.setLoadAttributes();

        makeBuildings();
        makeUpgrades();
        makeMinigames();
        setupUI();
        
    }


    public void datSave()
    {
        save.saveData(data);
    }

    public void sudokuMake()
    {
        sudok.make();
    }

    public void sudokuNum(int num)
    {
        sudok.pickNum(num);
    }

    public void restartBattle()
    {
        battleArena.restartBattle();
    }

    public void datLoad()
    {
        SaveData d = save.loadData();

        data = d;

        d.setIcons(buildingIconList, upgradeIconList);

        updateAmt();
        updateWindowMenu();

    }




    void makeUpgrades()
    {
        data.getUHold().makeUpgrades(constData);
        data.getUHold().setIconList(upgradeIconList);
    }

    void makeBuildings()
    {
        data.getBHold().makeBuildings(constData);
        data.getBHold().setIconList(buildingIconList);
    }

    void makeMinigames()
    {
        data.getMHold().makeMinigames(constData);
        data.getMHold().setIconList(minigameIconList);
    }



    void setupUI()
    {
        buildBuildBtns();
        buildUpgradeBtns();
        infoMenuMini.buildMiniBtns(data, canvasMiniBtn, scrollbars);
        updateUpgradeButnText();
        updateBuildingButnText();

    }

    public bool canBuy(BigInteger amt)
    {
        return data.getCash() >= amt;
    }

    void takeCost(BigInteger amt)
    {
        data.subCash(amt);
    }

    public bool tryBuy(BigInteger amt)
    {
        if (canBuy(amt))
        {
            takeCost(amt);
            return true;
        }

        return false;
    }





    Building getBuildFromId(int idx)
    {
        return data.getBHold().getBuilding(idx);
    }


    Upgrade getUpgradeFromId(int idx)
    {
        return data.getUHold().getUpgrade(idx);
    }

    // Start is called before the first frame update


    

    




    void ascention()
    {

    }




    void openMenuBuild(int idx)
    {
        infoMenuBuilding.setSelected(idx);
        UIBuildingBuyMenu.SetActive(true);
    }

    void openMenuUpgrade(int idx)
    {
        infoMenuUpgrade.setSelected(idx);
        UIUpgradeBuyMenu.SetActive(true);
    }

    public void buyBuildingButton()
    {
        buyBuilding(infoMenuBuilding.getSelected());
    }

    public void buyUpgradeButton()
    {
        buyUpgrade(infoMenuUpgrade.getSelected());
    }

    public void clickSkill(int i)
    {
        battleArena.clickSkill(i);
    }

    public void clickSkillInfo(int i)
    {
        battleArena.clickSkillInfo(i);
    }

    public void clickSkillAuto(int i)
    {
        battleArena.clickSkillAuto(i);
    }


    public void clickDataButton(int i)
    {
        battleArena.clickDataButton(i);
    }


    void buildBuildBtns()
    {
        GameObject buildBtn = Resources.Load<GameObject>("UI/BuildBtn") as GameObject;
        for (int i = 0; i < data.getBHold().getCount(); i++)
        {
            buildAmtList.Add(0);
            ////////////
            GameObject buildTemp = Instantiate(buildBtn);
            buildTemp.transform.SetParent(canvasBuildBtn.transform);
            RectTransform rt = buildTemp.GetComponent<RectTransform>();
            //Vector2 pos = new Vector2(0.0f, -70.0f*i);
            rt.transform.localScale = new Vector2(1.0f, 1.0f);


            infoMenuBuilding.addButn(buildTemp);
            Button bc = buildTemp.GetComponent<Button>();
            int tempInt = i;
            bc.onClick.AddListener(delegate { openMenuBuild(tempInt); });
            GameObject ntx = buildTemp.transform.Find("Name").gameObject;
            if (ntx != null)
            {
                ntx.transform.GetComponent<TMPro.TextMeshProUGUI>().text = data.getBHold().getBuilding(i).getName();
            }

            GameObject bdi = buildTemp.transform.Find("Image").gameObject;
            bdi.GetComponent<Image>().sprite = data.getBIcon(i);
        }

        scrollbars[0].normalizedPosition = new Vector2(0, 1);

    }




    void buildUpgradeBtns()
    {
        GameObject buildBtn = Resources.Load<GameObject>("UI/UpgradeBtn") as GameObject;
        for (int i = 0; i < data.getUHold().getCount(); i++)
        {
            GameObject buildTemp = Instantiate(buildBtn);
            buildTemp.transform.SetParent(canvasUpgradeBtn.transform);
            RectTransform rt = buildTemp.GetComponent<RectTransform>();
            //Vector2 pos = new Vector2(0.0f, -70.0f * i);
            rt.transform.localScale = new Vector2(1.0f, 1.0f);

            infoMenuUpgrade.addButn(buildTemp);

            Button bc = buildTemp.GetComponent<Button>();
            int tempInt = i;
            bc.onClick.AddListener(delegate { openMenuUpgrade(tempInt); });


            GameObject bdi = buildTemp.transform.Find("Image").gameObject;
            bdi.GetComponent<Image>().sprite = data.getUIcon(i);

            //bdi.GetComponent<Image>().SetAsLastSibling();

        }
        scrollbars[1].normalizedPosition = new Vector2(0, 1);
    }


    void openSave()
    {
        UIsave.SetActive(true);
    }

    void closeSave()
    {
        UIsave.SetActive(false);
    }


    void openBuildings()
    {
        closeAll();
        UIbuildings.SetActive(true);
        infoMenuBuilding.reset();
        scrollbars[0].normalizedPosition = new Vector2(0, 1);
    }
    void closeBuildings()
    {
        UIBuildingBuyMenu.SetActive(false);
        UIbuildings.SetActive(false);
        infoMenuBuilding.reset();
    }

    void openMini()
    {
        closeAll();
        uiWindows[0].SetActive(true);


    }
    void closeMini()
    {
        uiWindows[0].SetActive(false);
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

    void closeGames()
    {

        List<GameObject> c = getChildren(uiWindows[1]);

        foreach (GameObject g in c)
        {
            g.SetActive(false);

        }

    }


    void openUpgrades()
    {
        UIUpgradeBuyMenu.SetActive(false);
        UIupgrades.SetActive(true);
        infoMenuUpgrade.reset();
        //selectedUpgrade = -1;
        scrollbars[1].normalizedPosition = new Vector2(0, 1);
    }
    void closeUpgrades()
    {
        UIUpgradeBuyMenu.SetActive(false);
        UIupgrades.SetActive(false);
        infoMenuUpgrade.reset();
        //selectedUpgrade = -1;
    }

    void closeAll()
    {
        closeGames();
        closeUpgrades();
        closeBuildings();
        closeSave();
        closeMini();
    }


    public void showBuildings()
    {
        closeAll();

        if (lastWindow != 0)
        {
            openBuildings();
            lastWindow = 0;
        }
        else
        {
            lastWindow = -1;
        }
        
        
    }

    public void showUpgrades()
    {
        closeAll();
        if (lastWindow != 1)
        {
            openUpgrades();
            lastWindow = 1;
        }
        else
        {
            lastWindow = -1;
        }

    }

    public void showSave()
    {
        closeAll();
        
        if (lastWindow != 2)
        {
            openSave();
            lastWindow = 2;
        }
        else
        {
            lastWindow = -1;
        }
    }

    public void resetLastWindow()
    {
        lastWindow = -1;
    }

    public void showMini()
    {
        closeAll();
        if (lastWindow != 3)
        {
            openMini();
            lastWindow = 3;
        }
        else
        {
            lastWindow = -1;
        }

    }










    void updateTotal()
    {
        
        txtCurrent.GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(data, data.getCash());
        txtSecond.GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(data, data.getSecond()) + secondAdd;
        txt[0].GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(data, data.getSecond());
        txt[1].GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(data, data.getSecond());
        //txtRunTotal.GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(cardsRunTotal);
    }


    BigInteger buildingPassive()
    {
        BigInteger bTotal = 0;

        for (int i = 0; i < data.getBHold().getCount(); i++)
        {
            bTotal += data.getBHold().getBuilding(i).getPassive();
        }

        return bTotal;
    }

    void calcPassive()
    {
        BigInteger b = buildingPassive();

        data.setSecond(b);
        
    }

    void updateBuildingButnText()
    {
        infoMenuBuilding.updateButtonText(data,tm);
    }

    void updateUpgradeButnText()
    {
        infoMenuUpgrade.updateButtonText(data, tm);
    }

    void updateAmt()
    {
        calcPassive();
        updateBuildingButnText();
        updateTotal();

    }

    void buyBuilding(int bid)
    {

        if ((bid != -1) && tryBuy(data.buildingCost(bid)))
        {
            getBuildFromId(bid).addOne();
        }

        updateAmt();
    }


    void buyUpgrade(int bid)
    {
        if ((bid != -1) && !data.getUHold().getUpgrade(bid).isBought())
        {
            if (tryBuy(data.upgradeCost(bid)))
            {
                infoMenuUpgrade.buyUpgradeConfirm(data,bid);
                updateAmt();
            }
        }
    }





    void gainCard(BigInteger amt)
    {

        data.addCash(amt * data.getGlobalMult());
        updateAmt();
    }

    void gainCardBuild(BigInteger amt)
    {
        data.addCash(amt);
        updateAmt();
    }

    void addPassive(float time)
    {
        addTimer += time;
        double t = 0.1;

        if (addTimer > t)
        {
            addTimer = 0;
            gainCardBuild(CustomHelp.bigIntegerMult(data.getSecond(), t));
        }
    }


    void updateBuildingMenu()
    {
        if (infoMenuBuilding.getSelected() != -1)
        {
            Building b = getBuildFromId(infoMenuBuilding.getSelected());
            Transform t = UIBuildingBuyMenu.transform;
            infoMenuBuilding.updateStats(data,t, b);

        }

    }

    void updateUpgradeMenu()
    {
        if (infoMenuUpgrade.getSelected() != -1)
        {
            Upgrade b = getUpgradeFromId(infoMenuUpgrade.getSelected());
            Transform t = UIUpgradeBuyMenu.transform;
            infoMenuUpgrade.updateStats(data,t, b);
        }
        infoMenuUpgrade.updateButnUI(data);
    }



    void updateWindowMenu()
    {
        updateBuildingMenu();
        updateUpgradeMenu();
    }

    void keyPresses()
    {

    }


    // Update is called once per frame
    void Update()
    {

        float change = Time.deltaTime;
        //keyPresses();
        //addPassive(change);
        currentTimer += change;

        battleArena.fight(change);

        //updateTotal();
        //updateWindowMenu();
    }

    BigInteger cardClickGain()
    {

        BigInteger f = new BigInteger(1);
        return (f);
    }


    void mainClick()
    {
       
        gainCard(cardClickGain());
    }


    public void mainClickButton()
    {
        mainClick();
    }


}
