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

        GUI.backgroundColor = (ObserverAction)observer.actionIndex == ObserverAction.Add ? Color.green
                            : (ObserverAction)observer.actionIndex == ObserverAction.Reset ? Color.red : Color.white;

        observer.actionIndex = EditorGUI.Popup(position.RatioPositionX(0, 4).ClampHeight(20), observer.actionIndex, System.Enum.GetNames(typeof(ObserverAction)));

        GUI.backgroundColor = Color.white;

        GUIContent buttonSkin = new GUIContent(Resources.Load("RightArrow") as Texture2D);

        if (GUI.Button(position.RatioPositionX(4.25f, 1.25f).ClampHeight(20), buttonSkin))
        {
            switch ((ObserverAction)observer.actionIndex)
            {
                case ObserverAction.Add:
                    observer.conditions.Add(new UGS_Condition(observer.conditions.Count, Color.white));
                    break;
                case ObserverAction.Reset:
                    observer.conditions.Clear();
                    observer.outputs.Clear();
                    break;
            }
        }

        Color defaultColor = Color.white;

        UGS_Condition condition;

        for (int i = 0; i < observer.conditions.Count; i++) // 0 1 2
        {
            condition = observer.conditions[i];

            float widthProg = 0f;

            if (GUI.Button(position.RatioPositionXLayout(6f, 1.5f, ref widthProg).SkipLine(i, 20), "-")) observer.conditions.RemoveAt(i);


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

                    Array.Sort(compNames, StringComparer.OrdinalIgnoreCase);

                    foreach (Component comp in components)
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
                        if (mi.ReturnType == typeof(bool))
                            methodNames[methIndex] = mi.Name;

                        methIndex++;
                    }

                    methodNames = methodNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    Array.Sort(methodNames, StringComparer.OrdinalIgnoreCase);

                    condition.methodIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.methodIndex, methodNames);
                    condition.methodName = methodNames[condition.methodIndex];

                }
            }
            else if ((ConditionType)condition.typeIndex == ConditionType.Logic)
            {
                condition.target = EditorGUI.ObjectField(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.target, typeof(GameObject), true) as GameObject;

                if (condition.target != null)
                {

                    Component[] components = condition.target.GetComponents<Component>();
                    string[] compNames = new string[components.Length];
                    int compIndex = 0;

                    foreach (Component comp in components)
                    {
                        compNames[compIndex] = comp.GetType().Name;
                        compIndex++;
                    }

                    Array.Sort(compNames, StringComparer.OrdinalIgnoreCase);

                    condition.componentIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.componentIndex, compNames);

                    FieldInfo[] fieldInfos = components[condition.componentIndex].GetType().GetFields();
                    PropertyInfo[] propertyInfos = components[condition.componentIndex].GetType().GetProperties();
                    string[] fieldNames = new string[fieldInfos.Length + propertyInfos.Length];
                    int fieldIndex = 0;

                    foreach (FieldInfo fi in fieldInfos)
                    {
                        fieldNames[fieldIndex] = fi.Name + " (" + fi.FieldType.Name + ")";
                        fieldIndex++;
                    }

                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        fieldNames[fieldIndex] = pi.Name + " (" + pi.PropertyType.Name + ")";
                        fieldIndex++;
                    }

                    Array.Sort(fieldNames, StringComparer.OrdinalIgnoreCase);

                    condition.fieldIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.fieldIndex, fieldNames);


                    if (fieldInfos.Length > 0 && condition.fieldIndex < fieldInfos.Length)
                    {
                        FieldInfo field = fieldInfos[condition.fieldIndex];

                        if (field.FieldType.GetInterfaces().Contains(typeof(IComparable)))
                        {
                            condition.operatorIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.operatorIndex, System.Enum.GetNames(typeof(Operator)));
                        }
                        else
                        {
                            condition.operatorIndex = EditorGUI.Popup(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.operatorIndex, new string[2] { Operator.EQUALS.ToString(), Operator.DIFFERENT.ToString() });
                        }

                        if (!field.FieldType.IsValueType && field.FieldType.Name != "String")
                        {
                            condition.atom = null;

                            condition.comparedObject = EditorGUI.ObjectField(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), condition.comparedObject, field.FieldType, true);
                        }
                        else
                        {
                            condition.comparedObject = null;

                            switch (field.FieldType.Name)
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

                                        (condition.atom as UGS_Int).value = EditorGUI.IntField(position.RatioPositionXLayout(1, 1 + (0.5f * (condition.atom as UGS_Int).value.ToString().Length), ref widthProg).SkipLine(i, 20), (condition.atom as UGS_Int).value);
                                        break;
                                    }
                                case string s when s == (typeof(string)).Name:
                                    {
                                        if (condition.atom.GetType().Name != typeof(UGS_String).Name) condition.atom = new UGS_String();

                                        (condition.atom as UGS_String).value = EditorGUI.TextField(position.RatioPositionXLayout(1, 4 + (0.4f * (condition.atom as UGS_String).value.ToString().Length), ref widthProg).SkipLine(i, 20), (condition.atom as UGS_String).value);
                                        break;
                                    }
                                case string s when s == (typeof(Vector2)).Name:
                                    {
                                        if (condition.atom.GetType().Name != typeof(UGS_Vector2).Name) condition.atom = new UGS_Vector2();

                                        (condition.atom as UGS_Vector2).value = EditorGUI.Vector2Field(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), "", (condition.atom as UGS_Vector2).value);
                                        break;
                                    }
                                case string s when s == (typeof(Vector3)).Name:
                                    {
                                        if (condition.atom.GetType().Name != typeof(UGS_Vector3).Name) condition.atom = new UGS_Vector3();

                                        (condition.atom as UGS_Vector3).value = EditorGUI.Vector3Field(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), "", (condition.atom as UGS_Vector3).value);
                                        break;
                                    }
                                case string s when s == (typeof(Color)).Name:
                                    {
                                        if (condition.atom.GetType().Name != typeof(UGS_Color).Name) condition.atom = new UGS_Color();

                                        (condition.atom as UGS_Color).value = EditorGUI.ColorField(position.RatioPositionXLayout(1, 10, ref widthProg).SkipLine(i, 20), (condition.atom as UGS_Color).value);
                                        break;
                                    }
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

        return 20 * (higher) + 10;
    }
}
public enum ConditionType {Logic, Method, None}

public enum Operator {EQUALS, DIFFERENT, SUPERIOR, INFERIOR}

public enum ObserverAction {Add, Reset}


