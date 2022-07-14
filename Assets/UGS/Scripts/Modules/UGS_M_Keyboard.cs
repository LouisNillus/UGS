using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UGS_M_Keyboard : UGS_Module
{
    [Header("Key Down")]
    public List<UGS_Input> inputsKD = new List<UGS_Input>();
    [Header("Key")]
    public List<UGS_Input> inputsK = new List<UGS_Input>();
    public bool onMouseMoveOnly;
    public Vector2 mouseMovementThreshold;
    [Header("Key Up")]
    public List<UGS_Input> inputsKU = new List<UGS_Input>();

    [Header("Mouse Scroll Up")]
    public List<UGS_Action> actionsSU = new List<UGS_Action>();

    [Header("Mouse Scroll Down")]
    public List<UGS_Action> actionsSD = new List<UGS_Action>();

    bool holdingDown;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (UGS_Input input in inputsKD)
            {
                if (Input.GetKeyDown(input.key))
                {
                    if (input.reverse) input.PlayActionsReverse(grid); 
                    else input.PlayActions(grid);
                }
            }
        }

        if (Input.anyKey && (!onMouseMoveOnly || (onMouseMoveOnly && (Input.GetAxis("Mouse X").IsOutof(-mouseMovementThreshold.x, mouseMovementThreshold.x) || Input.GetAxis("Mouse Y").IsOutof(-mouseMovementThreshold.y, mouseMovementThreshold.y) ) ) ) )
        {
            foreach (UGS_Input input in inputsK)
            {
                if (Input.GetKey(input.key))
                {
                    if (input.reverse) input.PlayActionsReverse(grid);
                    else input.PlayActions(grid);
                }
            }
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            foreach (UGS_Action action in actionsSU)
            {
                action.Play(grid);             
            }
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            foreach (UGS_Action action in actionsSD)
            {
                action.Play(grid);
            }
        }
    }

    private void OnGUI()
    {        
        Event e = Event.current;

        if (e.type == EventType.KeyUp)
        {
            foreach (UGS_Input input in inputsKU)
            {
                if (Input.GetKeyUp(input.key))
                {
                    if (input.reverse) input.PlayActionsReverse(grid);
                    else input.PlayActions(grid);
                }
            }
        }
    }

    public void Log() => Debug.Log("yeeees!");

    [Button("BindKey")]
    public bool readInput;
    public void BindKey()
    {

    }
}

[System.Serializable]
public class UGS_Input
{

    public KeyCode key;
    public bool reverse;

    [ColorField(0,1,0,1,0,0, true)]
    public bool disabled;

    public List<UGS_Action> actions = new List<UGS_Action>();

    public void PlayActions(UGS_Grid grid)
    {
        if(!disabled)
        foreach(UGS_Action action in actions)
        {
            action.Play(grid);
        }
    }

    public void PlayActionsReverse(UGS_Grid grid)
    {
        if (!disabled)
        for (int i = actions.Count - 1; i >= 0; i--)
        {
            actions[i].Play(grid);
        }
    }

}



