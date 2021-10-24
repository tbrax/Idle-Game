using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using CustomHelpNameSpace;
using UnityEngine.UI;
public class UIMenuBuilding
{
    int selectedBuilding = -1;
    Manip m;

    string secondAdd;
    int globalMult;

    List<GameObject> buildButnList = new List<GameObject>();



    public UIMenuBuilding(Manip mm, string s, int g)
    {
        m = mm;
        secondAdd = s;
        globalMult = g;
    }

    public void addButn(GameObject t)
    {
        buildButnList.Add(t);
    }


    public GameObject getButn(int i)
    {
        return buildButnList[i];
    }

    public void updateButtonText(SaveData data, Manip tm)
    {
        for (int i = 0; i < buildButnList.Count; i++)
        {
            Transform ntx = buildButnList[i].transform;

            // GameObject ntx0 = ntx.Find("Name").gameObject;
            Transform ntx1 = ntx.Find("Cost");
            Transform ntx2 = ntx.Find("Amt");

            BigInteger buildCost = data.buildingCost(i);
            ulong buildAmt = data.getBHold().getBuilding(i).getAmt();

            if (ntx1 != null)
            {
                ntx1.GetComponent<TMPro.TextMeshProUGUI>().text = tm.numToText(data, buildCost);
            }

            if (ntx2 != null)
            {
                ntx2.GetComponent<TMPro.TextMeshProUGUI>().text = buildAmt.ToString();
            }
        }

    }


    public int getSelected()
    {
        return selectedBuilding;
    }

    public void setSelected(int select)
    {
        selectedBuilding = select;
    }

    public void reset()
    {
        selectedBuilding = -1;
    }

    public void updateStats(SaveData data, Transform t, Building b)
    {
        if (selectedBuilding != -1)
        {
            t.Find("Amt").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = b.getAmt().ToString();
            t.Find("Title").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = b.getName();
            t.Find("GainPerSecond").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = m.numToText(data, b.getPassive()) + secondAdd;
            t.Find("Buy").Find("Cost").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + m.numToText(data, b.getNextCost() * globalMult);
            t.Find("Desc").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = b.getDesc();
            t.Find("Image").gameObject.GetComponent<Image>().sprite = b.getIcon();
        }

    }


}
