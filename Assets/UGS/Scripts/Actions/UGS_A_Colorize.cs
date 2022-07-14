using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Colorize : UGS_Action
{
    public bool item;
    public bool background;

    public Color color;

    public CellType cellType;


    public override void Play(UGS_Grid grid)
    {
        Cell c = grid.GetCellFromType(cellType);

        Debug.Log(c);

        if(c != null && item) c.SetItemColor(color);
        if (c != null && background) c.SetBackgroundColor(color);
    }
}
