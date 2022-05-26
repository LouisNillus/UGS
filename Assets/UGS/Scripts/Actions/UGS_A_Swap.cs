using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Swap : UGS_Action
{
    public CellType first;
    public CellType second;

    public bool includeBackground;

    public override void Play(UGS_Grid grid)
    {
        grid.SwapCells(grid.GetCellFromType(first), grid.GetCellFromType(second), includeBackground);
    }
}
