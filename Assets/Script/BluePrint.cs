using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    public string ItemName;

    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;

    public BluePrint(string name, int reqNum, string r1, int r1Num, string r2, int r2Num)
    {
        ItemName = name;

        numOfRequirements = reqNum;

        Req1 = r1;
        Req2 = r2;

        Req1Amount = r1Num; 
        Req2Amount = r2Num;
    }
}
