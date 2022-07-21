using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UGS_Checker
{
    public string idName;

    public List<UGS_Observer> observers = new List<UGS_Observer>();

    public List<UGS_Action> output = new List<UGS_Action>();
}
