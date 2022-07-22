using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class UGS_Observer
{
    public List<UGS_Action> outputs = new List<UGS_Action>();

    public List<UGS_Condition> conditions = new List<UGS_Condition>();

    public int actionIndex;
    public UGS_Observer()
    {
        actionIndex = 0;
    }

    public void Start()
    {
        if (conditions.Count == 0) conditions.Add(new UGS_Condition(0, Color.white));
    }


    public bool True()
    {
        return true;
    }

    public static bool go()
    {
        return true;
    }

    public bool Pass()
    {
        return false;
    }
}