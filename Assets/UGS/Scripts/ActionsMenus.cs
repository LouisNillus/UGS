using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class ActionsMenus
{
    public static T Create<T>(string name) where T : ScriptableObject
    {
        T action = ScriptableObject.CreateInstance<T>();
        
        string path = "Assets/UGS/Data/Actions/" + name + ".asset";
        AssetDatabase.CreateAsset(action, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = action;

        return action;
    }

    [MenuItem("UGS/Actions/Move")] public static void Move() => Create<UGS_A_Move>("Move");
    [MenuItem("UGS/Actions/Select")] public static void Select() => Create<UGS_A_Move>("Select");
    [MenuItem("UGS/Actions/Weight")] public static void Weight() => Create<UGS_A_Weight>("Weight");
    [MenuItem("UGS/Actions/Color")] public static void Color() => Create<UGS_A_Colorize>("Color");
    [MenuItem("UGS/Actions/Zoom")] public static void Zoom() => Create<UGS_A_Zoom>("Zoom");










}

