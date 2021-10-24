using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using CustomHelpNameSpace;

public class Manip
{

    List<string> smallWordList;
    List<BigInteger> smallLimitList;


    public Manip()
    {
        setup();
    }

    void setup()
    {
        smallWordList = new List<string>();
        smallLimitList = new List<BigInteger>();


        makeNumTextList();
    }



    public string numToText(SaveData data,BigInteger num)
    {
        string s = smallerNum(num);
        return s;
    }


    string smallerNum(BigInteger num)
    {

        int place = 0;
        while ((place < smallLimitList.Count - 1) && (num >= smallLimitList[place + 1]))
        {
            place += 1;
        }

        int acc = 3;
        string s = num.ToString();
        if (place == 0)
        {
            s = "000" + s;
            place += 1;
        }
        int sub = s.Length - (place * 3);

        string s1 = s.Substring(0, sub);
        int msub = (int)Mathf.Min(acc, s.Length - sub);
        string s2 = s.Substring(sub, msub);

        if (s1 == "000")
        {
            s1 = "0";
        }

        s1 += ".";

        string sfinal = s1 + s2 + smallWordList[place];

        return sfinal;
    }

    void makeNumTextList()
    {
        smallWordList.Add("");
        smallWordList.Add("");
        smallWordList.Add(" Thousand");
        smallWordList.Add(" Million");
        smallWordList.Add(" Billion");
        smallWordList.Add(" Trillion");
        smallWordList.Add(" Quadrillion");
        smallWordList.Add(" Quintillion");
        smallWordList.Add(" Hextillion");
        smallWordList.Add(" Septillion");
        smallWordList.Add(" Octillion");
        smallWordList.Add(" Nonillion");
        smallWordList.Add(" Decillion");
        smallWordList.Add(" Undecillion");
        smallWordList.Add(" Duodecillion");
        smallWordList.Add(" Tredecillion");
        smallWordList.Add(" Quattuordecillion");
        smallWordList.Add(" Quindecillion");
        smallWordList.Add(" Hexdecillion");
        smallWordList.Add(" Septendecillion");
        smallWordList.Add(" Octodecillion");

        for (int i = 0; i < smallWordList.Count; i++)
        {
            string bs = "1";

            for (int j = 0; j < i; j++)
            {
                bs += "000";
            }

            BigInteger t = BigInteger.Parse(bs);
            smallLimitList.Add(t);
        }
    }
}
