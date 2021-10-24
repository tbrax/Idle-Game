using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using CustomHelpNameSpace;
using System.Globalization;
using UnityEngine.UI;
public class UIMenuMini
{
    int selectedMini = -1;
    Manip m;

    string secondAdd;
    int globalMult;

    List<GameObject> buildButnList = new List<GameObject>();
    public List<GameObject> uiWindows;
    public List<GameObject> gameWindows;

    ButtonController control;

    public UIMenuMini(ButtonController c, Manip mm, string s, int g, List<GameObject> uw, List<GameObject> mw)
    {
        m = mm;
        secondAdd = s;
        globalMult = g;

        uiWindows = uw;
        gameWindows = mw;
        control = c;
    }

    public int getSelected()
    {
        return selectedMini;
    }


    void openBattle()
    {
        closeMinis();
        closeSelectList();
        uiWindows[1].SetActive(true);
        gameWindows[1].SetActive(true);
    }

    void openSudoku()
    {
        closeMinis();
        closeSelectList();
        uiWindows[1].SetActive(true);
        gameWindows[0].SetActive(true);
    }

    void closeMinis()
    {
        foreach (GameObject o in gameWindows)
        {
            o.SetActive(false);
        }
    }

    void closeSelectList()
    {
        uiWindows[0].SetActive(false);
    }

    void openMiniBuild(int idx)
    {
        control.resetLastWindow();
        setSelected(idx);
        closeMinis();

        switch (idx)
        {
            case 0:
                openSudoku();
                break;
            case 1:
                openBattle();
                break;
            default:
                closeMinis();
                break;
        }
    }

    public void buildMiniBtns(SaveData data, GameObject canvasMiniBtn, List<ScrollRect> scrollbars)
    {
        GameObject buildBtn = Resources.Load<GameObject>("UI/MiniBtn") as GameObject;
        for (int i = 0; i < data.getMHold().getCount(); i++)
        {
            ////////////
            GameObject buildTemp = GameObject.Instantiate(buildBtn);
            buildTemp.transform.SetParent(canvasMiniBtn.transform);
            RectTransform rt = buildTemp.GetComponent<RectTransform>();
            rt.transform.localScale = new UnityEngine.Vector2(1.0f, 1.0f);
            Button bc = buildTemp.GetComponent<Button>();
            int tempInt = i;
            bc.onClick.AddListener(delegate { openMiniBuild(tempInt); });
            GameObject ntx = buildTemp.transform.Find("Name").gameObject;
            if (ntx != null)
            {
                ntx.transform.GetComponent<TMPro.TextMeshProUGUI>().text = data.getMHold().getMinigameName(i);
            }

            GameObject bdi = buildTemp.transform.Find("Image").gameObject;
            bdi.GetComponent<Image>().sprite = data.getMIcon(i);
        }

        scrollbars[0].normalizedPosition = new UnityEngine.Vector2(0, 1);

    }



    public void setSelected(int select)
    {
        selectedMini = select;
    }




}
