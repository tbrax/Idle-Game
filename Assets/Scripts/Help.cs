using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;


namespace CustomHelpNameSpace
{
    public class MiscHelp
    {



    }





    public static class CustomHelp
    {
        
        public static BigInteger bigIntegerMult(BigInteger b, double mult)
        {
            
            int dig = 5;
            int val = (int)Mathf.Pow(10,dig);

            long intPart = (long)mult;
            double fractionalPart = mult - intPart;


            BigInteger c = b * intPart;

            double tempFrac = fractionalPart;
            BigInteger tempDec = b;

            for (int i = 0; i < dig; i++)
            {
                tempDec = tempDec / 10;
                tempFrac *= 10;
                long multBy = (int)tempFrac;

                tempFrac = tempFrac - multBy;
                c += multBy * tempDec;
            }

            return c;
        }
    }

}




