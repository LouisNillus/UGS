using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(ColorFieldAttribute))]
public class ColorFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ColorFieldAttribute target = attribute as ColorFieldAttribute;

        

        if (property.propertyType == SerializedPropertyType.Boolean)
        {
            bool value = property.boolValue;

            GUI.backgroundColor = value ? (target.inverse ? target.colorOff : target.colorOn) : (target.inverse ? target.colorOn : target.colorOff);
            property.boolValue = EditorGUI.Toggle(position, label, property.boolValue);
            GUI.backgroundColor = Color.white;
        }
        else if(property.propertyType == SerializedPropertyType.Integer)
        {
            GUI.backgroundColor = target.colorOn;
            property.intValue = EditorGUI.IntField(position, label, property.intValue);
            GUI.backgroundColor = Color.white;

        }
        else if (property.propertyType == SerializedPropertyType.Float)
        {
            GUI.backgroundColor = target.colorOn;
            property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
            GUI.backgroundColor = Color.white;
        }
        else if (property.propertyType == SerializedPropertyType.String)
        {
            GUI.backgroundColor = target.colorOn;
            property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
            GUI.backgroundColor = Color.white;
        }
        else if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            bool value = property.objectReferenceValue != null;


            GUI.backgroundColor = value ? (target.inverse ? target.colorOff : target.colorOn) : (target.inverse ? target.colorOn : target.colorOff);
            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, fieldInfo.FieldType, true);
            GUI.backgroundColor = Color.white;
        }


    }
}
