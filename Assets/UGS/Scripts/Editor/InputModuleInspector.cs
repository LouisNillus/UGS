using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UGS_M_Keyboard))]
public class InputModuleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Input Module Window"))
        {
            InputModuleWindow.Init();
        }
    }
}
