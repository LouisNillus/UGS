using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Cell
{
    object item;
    object background;

    [HideInInspector] public UnityEvent OnSelectCell;
    [HideInInspector] public UnityEvent OnReleaseCell;

    public Vector3 position;
    public Vector2Int gridPosition;

    public bool occupied = false;

    public UGS_Node node;

    public void Initialize()
    {
        occupied = item != null;
    }

    public void Clear(bool includeBackground = false)
    {
        occupied = false;
        item = null;

        if(includeBackground) background = null;
    }

    #region Set Item / Background

    public void SetBackground(object _background)
    {
        if(_background == null) return;
            
        background = _background;
    }

    public void SetItem(object _item)
    {
        if(_item == null) return;

        item = _item;
        occupied = true;
    }

    #region Colors
    public void SetItemColor(Color col)
    {
        if(item is GameObject && (item as GameObject).GetComponent<SpriteRenderer>())
        {
            (item as GameObject).GetComponent<SpriteRenderer>().color = col;
        }
    }

    public void SetBackgroundColor(Color col)
    {
        if (background is GameObject && (background as GameObject).GetComponent<SpriteRenderer>())
        {
            (background as GameObject).GetComponent<SpriteRenderer>().color = col;
        }
    }
    
    #endregion

    #endregion


    #region Item Access

    public T Item<T>()
    {
        try
        {
            return (T)item;
        }
        catch
        {
            Debug.LogError("Invalid cast for \"ITEM\"");
            return default(T);
        }        
    }

    public T Background<T>()
    {
        try
        {
            return (T)background;
        }
        catch
        {
            Debug.LogError("Invalid cast for \"BACKGROUND\"");
            return default(T);
        }
    }

    public GameObject ItemGO()
    {
        try
        {
            return (GameObject)item;
        }
        catch
        {
            Debug.LogError("Can't cast item as GameObject");
            return null;
        }
    }
    #endregion
}
