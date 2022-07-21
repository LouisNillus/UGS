using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorExtensions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(UGS_Observer))]
public class ObserverDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.serializedObject.Update();


        //Initialize(property);

        //UGS_Observer observer = property.serializedObject.targetObject as UGS_Observer;


        UGS_Observer observer = EditorMethods.GetValue(property) as UGS_Observer;


        observer.Start();
        
        //float propertyHeight = GetPropertyHeight(property, label);

        SerializedProperty outputs = property.FindPropertyRelative("outputs");
        SerializedProperty conditions = property.FindPropertyRelative("conditions");

        int outputsCount = outputs.arraySize;

        if (outputsCount <= 0) outputs.InsertArrayElementAtIndex(0);



        if (GUI.Button(position.RatioPositionX(0, 2).ClampHeight(20), "+"))
        {
            observer.conditions.Add(new UGS_Condition(observer.conditions.Count, Color.white));
        }

        Color defaultColor = Color.white;

        UGS_Condition condition;

        for (int i = 0; i < observer.conditions.Count; i++) // 0 1 2
        {
            condition = observer.conditions[i];

            float widthProg = 0f;

            if (GUI.Button(position.RatioPositionXLayout(2.5f, 1.5f, ref widthProg).SkipLine(i, 20), "-")) observer.conditions.RemoveAt(i);


            condition.color = EditorGUI.ColorField(position.RatioPositionXLayout(0.5f, 2, ref widthProg).SkipLine(i, 20), condition.color);
            GUI.backgroundColor = condition.color;

            condition.typeIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 5, ref widthProg).SkipLine(i, 20), condition.typeIndex, System.Enum.GetNames(typeof(ConditionType)));

            if ((ConditionType)condition.typeIndex == ConditionType.Method)
            {
                condition.target = EditorGUI.ObjectField(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.target, typeof(GameObject), true) as GameObject;

                if (condition.target != null)
                {
                    Component[] components = condition.target.GetComponents<Component>();
                    string[] compNames = new string[components.Length];
                    int compIndex = 0;

                    foreach(Component comp in components)
                    {
                        compNames[compIndex] = comp.GetType().Name;
                        compIndex++;
                    }

                    condition.componentIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.componentIndex, compNames);

                    MethodInfo[] methodInfos = components[condition.componentIndex].GetType().GetMethods();
                    string[] methodNames = new string[methodInfos.Length];
                    int methIndex = 0;

                    foreach (MethodInfo mi in methodInfos)
                    {                        
                        if(mi.ReturnType == typeof(bool))
                            methodNames[methIndex] = mi.Name;

                        methIndex++;
                    }

                    methodNames = methodNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    condition.methodIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.methodIndex, methodNames);
                    condition.methodName = methodNames[condition.methodIndex];

                }        
            }
            else if((ConditionType)condition.typeIndex == ConditionType.Logic)
            {
                condition.target = EditorGUI.ObjectField(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.target, typeof(GameObject), true) as GameObject;

                if(condition.target != null)
                {

                    Component[] components = condition.target.GetComponents<Component>();
                    string[] compNames = new string[components.Length];
                    int compIndex = 0;

                    foreach (Component comp in components)
                    {
                        compNames[compIndex] = comp.GetType().Name;
                        compIndex++;
                    }

                    condition.componentIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.componentIndex, compNames);

                    FieldInfo[] fieldInfos = components[condition.componentIndex].GetType().GetFields();
                    string[] fieldNames = new string[fieldInfos.Length];
                    int fieldIndex = 0;

                    foreach (FieldInfo fi in fieldInfos)
                    {
                        fieldNames[fieldIndex] = fi.Name + " (" + fi.FieldType.Name + ")";
                        fieldIndex++;
                    }

                    condition.fieldIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.fieldIndex, fieldNames);

                    if(fieldInfos.Length > 0 && condition.fieldIndex < fieldInfos.Length)
                    {
                        FieldInfo field = fieldInfos[condition.fieldIndex];

                        if(field.FieldType.GetInterfaces().Contains(typeof(IComparable)))
                        {
                            condition.operatorIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.operatorIndex, System.Enum.GetNames(typeof(Operator)));
                        }

                        
                        
                        switch(field.FieldType.Name)
                        {
                            case string s when s == (typeof(bool)).Name:
                            {
                                    if (condition.atom.GetType().Name != typeof(UGS_Bool).Name) condition.atom = new UGS_Bool();

                                    (condition.atom as UGS_Bool).value = EditorGUI.Toggle(position.RatioPositionXLayout(1, 4, ref widthProg).SkipLine(i, 20), (condition.atom as UGS_Bool).value);
                                    break;
                            }
                            case string s when s == (typeof(float)).Name:
                            {
                                if (condition.atom.GetType().Name != typeof(UGS_Float).Name) condition.atom = new UGS_Float();

                                (condition.atom as UGS_Float).value = EditorGUI.FloatField(position.RatioPositionXLayout(1, 1 + (0.5f * (condition.atom as UGS_Float).value.ToString().Length), ref widthProg).SkipLine(i, 20), (condition.atom as UGS_Float).value);
                                break;
                            }
                            case string s when s == (typeof(int)).Name:
                                {
                                    if (condition.atom.GetType().Name != typeof(UGS_Int).Name) condition.atom = new UGS_Int();

                                    (condition.atom as UGS_Int).value = EditorGUI.IntField(position.RatioPositionXLayout(1, 1 + (0.5f *(condition.atom as UGS_Int).value.ToString().Length), ref widthProg).SkipLine(i, 20), (condition.atom as UGS_Int).value);
                                    break;
                                }
                        }

                    }






                }
            }

            GUI.backgroundColor = defaultColor;
        }



        if (GUI.Button(position.RatioPositionX(85, 2).ClampHeight(20), "+"))
            outputs.InsertArrayElementAtIndex(outputsCount);

        for (int i = 0; i < outputsCount; i++)
        {

            float outputsWidth = 0f;
            outputs.GetArrayElementAtIndex(i).objectReferenceValue = EditorGUI.ObjectField(position.RatioPositionXLayout(88, 10, ref outputsWidth).SkipLine(i, 20), outputs.GetArrayElementAtIndex(i).objectReferenceValue, typeof(UGS_Action), false);

            if (GUI.Button(position.RatioPositionXLayout(0.5f, 1.5f, ref outputsWidth).SkipLine(i, 20), "-")) observer.outputs.RemoveAt(i);
        }



        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int outputsCount = property.FindPropertyRelative("outputs").arraySize;

        UGS_Observer observer = EditorMethods.GetValue(property) as UGS_Observer;

        int higher = observer.outputs.Count >= observer.conditions.Count ? observer.outputs.Count : observer.conditions.Count;

        return 20 * (1 + higher);
    }
}
public enum ConditionType {Logic, Method, None}

public enum Operator {EQUALS, DIFFERENT, SUPERIOR, INFERIOR}


