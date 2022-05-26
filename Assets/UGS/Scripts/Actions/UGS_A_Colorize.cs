using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Colorize : UGS_Action
{
    public Color color;

    public CellType cellType;

    public override void Play(UGS_Grid grid)
    {
        Cell c = grid.GetCellFromType(cellType);

        if(c != null) c.SetBackgroundColor(color);
    }
}
