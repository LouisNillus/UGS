using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorHeaderAttribute : PropertyAttribute
{
    public string label;
    public int size;
    public bool bold;

    public Color color = Color.red;

    public ColorHeaderAttribute(string label, float r, float g, float b, int size = 12, bool bold = true)
    {
        this.label = label;
        this.size = size;
        this.bold = bold;

        this.color = new Color(r, g, b);
    }
}


