using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;


public static class ActionsMenus
{
    public static T Create<T>(string name) where T : ScriptableObject
    {
        T action = ScriptableObject.CreateInstance<T>();


        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        string pathToCurrentFolder = obj.ToString();

        string path = pathToCurrentFolder + "/" + name + ".asset";
        AssetDatabase.CreateAsset(action, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //EditorUtility.FocusProjectWindow();
        Selection.activeObject = action;

        return action;
    }

    [MenuItem("UGS/Actions/Move")] public static void Move() => Create<UGS_A_Move>("Move");
    [MenuItem("UGS/Actions/Select")] public static void Select() => Create<UGS_A_Move>("Select");
    [MenuItem("UGS/Actions/Pathfinding/Weight")] public static void Weight() => Create<UGS_A_Weight>("Weight");
    [MenuItem("UGS/Actions/Color")] public static void Color() => Create<UGS_A_Colorize>("Color");
    [MenuItem("UGS/Actions/Camera/Zoom")] public static void Zoom() => Create<UGS_A_Zoom>("Zoom");
    [MenuItem("UGS/Actions/Push")] public static void Push() => Create<UGS_A_Push>("Push");
    [MenuItem("UGS/Actions/LockIndex")] public static void LockIndex() => Create<UGS_A_LockIndex>("LockIndex");
    [MenuItem("UGS/Actions/Camera/Shake")] public static void Shake() => Create<UGS_A_Shake>("Shake");
    [MenuItem("UGS/Actions/Library/Extract")] public static void Extract() => Create<UGS_A_Extract>("Extract");
    [MenuItem("UGS/Actions/Library/Load")] public static void Load() => Create<UGS_A_Load>("Load");










}

