using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputModuleWindow : EditorWindow
{

    public static UGS_M_Keyboard inputs;

    SerializedObject serializedObject;

    Vector2 scrollPos;

    public SerializedProperty inputsKD;
    public SerializedProperty onMouseMoveOnly;
    public SerializedProperty mouseMovementThreshold;
    public SerializedProperty inputsK;
    public SerializedProperty inputsKU;
    public SerializedProperty actionsSU;
    public SerializedProperty actionsSD;

    [MenuItem("UGS/Windows/Input Module")]
    public static void Init()
    {
        inputs = FindObjectOfType<UGS_M_Keyboard>();

        if (inputs != null)
        {
            InputModuleWindow window = (InputModuleWindow)GetWindow(typeof(InputModuleWindow));
            window.Show();
        }
        else Debug.LogWarning("Please add an Input Module to the scene first");
    }
    public static void Init(UGS_M_Keyboard _inputs)
    {
        // Get existing open window or if none, make a new one:
        InputModuleWindow window = (InputModuleWindow)GetWindow(typeof(InputModuleWindow));

        inputs = _inputs;

        window.Show();
    }

    int currentTab;

    private void OnEnable()
    {
        serializedObject = new SerializedObject(inputs);
        inputsKD = serializedObject.FindProperty("inputsKD");      
        onMouseMoveOnly = serializedObject.FindProperty("onMouseMoveOnly");
        mouseMovementThreshold = serializedObject.FindProperty("mouseMovementThreshold");
        inputsK = serializedObject.FindProperty("inputsK");
        inputsKU = serializedObject.FindProperty("inputsKU");
        actionsSU = serializedObject.FindProperty("actionsSU");
        actionsSD = serializedObject.FindProperty("actionsSD");

        this.minSize = new Vector2(600, 500);
    }

    public void OnGUI()
    {        
        serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, false);

        currentTab = GUILayout.Toolbar(currentTab, new string[] { "Key Down", "Key", "Key Up", "Scroll Up", "Scroll Down" });

        switch(currentTab)
        {
            case 0:
                EditorGUILayout.PropertyField(inputsKD);
                break;
            case 1:
                EditorGUILayout.PropertyField(onMouseMoveOnly);
                EditorGUILayout.PropertyField(mouseMovementThreshold);
                EditorGUILayout.PropertyField(inputsK);
                break;
            case 2:
                EditorGUILayout.PropertyField(inputsKU);
                break;
            case 3:
                EditorGUILayout.PropertyField(actionsSU);
                break;
            case 4:
                EditorGUILayout.PropertyField(actionsSD);
                break;
        }

        EditorGUILayout.EndScrollView();


        serializedObject.ApplyModifiedProperties();
    }
}
