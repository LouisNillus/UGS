using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class UGS_Condition
{
    public int lineIndex;
    public int typeIndex;
    public Color color;

    public GameObject target;
    public int componentIndex;
    public int methodIndex;
    public string methodName;
    public int fieldIndex;

    public UGS_Atom atom;

    public int operatorIndex;

    public UGS_Condition(int lineIndex, Color color)
    {
        this.lineIndex = lineIndex;
        this.color = color;
    }
}
