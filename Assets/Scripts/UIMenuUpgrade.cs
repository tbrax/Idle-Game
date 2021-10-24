using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using CustomHelpNameSpace;

public class UIMenuUpgrade
{
    int selectedUpgrade = -1;
    Manip m;

    string secondAdd;
    int globalMult;

    List<GameObject> upgradeButnList = new List<GameObject>();


    public UIMenuUpgrade(Manip mm, string s, int g)
    {
        m = mm;
        secondAdd = s;
        globalMult = g;
    }


    public void addButn(GameObject t)
    {
        upgradeButnList.Add(t);
    }


    public void buyUpgradeConfirm(SaveData data,int bid)
    {
        data.getUHold().getUpgrade(bid).buy();
        updateButnUI(data);
    }


    

    public void updateButnUI(SaveData data)
    {
        for (int i = 0; i < upgradeButnList.Count; i++)
        {
            updateButnUI(data, i);

        }
    }

    void updateButnUI(SaveData data,int bid)
    {
        GameObject bdi = getButnImage(bid);
        if (data.getUHold().getUpgrade(bid).isBought())
        {
            bdi.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            bdi.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1.0f);
        }
    }

    GameObject getButnImage(int bid)
    {
        return upgradeButnList[bid].transform.Find("Image").gameObject;
    }



    void removeUpgradeBtns()
    {
        foreach (GameObject btn in upgradeButnList)
        {
            GameObject.Destroy(btn);
        }
    }

    public void updateButtonText(SaveData data, Manip tm)
    {

    }

    public int getSelected()
    {
        return selectedUpgrade;
    }

    public void setSelected(int select)
    {
        selectedUpgrade = select;
    }

    public void reset()
    {
        selectedUpgrade = -1;
    }

    public void updateStats(SaveData data, Transform t, Upgrade u)
    {
        if (selectedUpgrade != -1)
        {
            //t.Find("Amt").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = b.getAmt().ToString();
            t.Find("Title").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = u.getName();
            //t.Find("GainPerSecond").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = m.numToText(b.getPassive()) + secondAdd;
            t.Find("Buy").Find("Cost").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + m.numToText(data, u.getCost() * globalMult);
            t.Find("Desc").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = u.getDesc();
            //t.Find("Image").gameObject.GetComponent<Image>().sprite = b.getIcon();
            t.Find("Image").gameObject.GetComponent<Image>().sprite = data.getUIcon(u.getId());
        }

    }


}
