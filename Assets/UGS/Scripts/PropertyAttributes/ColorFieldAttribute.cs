using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ColorFieldAttribute : PropertyAttribute
{
    public float r1;
    public float g1;
    public float b1;

    public float r2;
    public float g2;
    public float b2;

    public bool inverse = false;
    public ColorFieldAttribute()
    {
        r1 = g1 = b1 = 1f;
        r2 = g2 = b2 = 1f;
    }
    public ColorFieldAttribute(float aR, float aG, float aB, float bR, float bG, float bB, bool _inverse = false)
    {
        r1 = aR;
        g1 = aG;
        b1 = aB;

        r2 = bR;
        g2 = bG;
        b2 = bB;

        inverse = _inverse;
    }

    public ColorFieldAttribute(float aR, float aG, float aB)
    {
        r1 = aR;
        g1 = aG;
        b1 = aB;
    }

    public Color colorOn { get { return new Color(r1, g1, b1, 1); } }
    public Color colorOff { get { return new Color(r2, g2, b2, 1); } }
}

