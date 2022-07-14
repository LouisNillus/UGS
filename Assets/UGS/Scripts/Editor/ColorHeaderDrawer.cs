using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorHeaderAttribute))]
public class ColorHeaderDrawer : DecoratorDrawer
{
    ColorHeaderAttribute header { get { return attribute as ColorHeaderAttribute; } }

    public override void OnGUI(Rect position)
    {
        GUIStyle style = GUI.skin.label;

        style.normal.textColor = header.color;
        style.fontStyle = header.bold ? FontStyle.Bold : FontStyle.Normal;
        style.fontSize = header.size;

        EditorGUI.LabelField(position, header.label, style);
    }

}