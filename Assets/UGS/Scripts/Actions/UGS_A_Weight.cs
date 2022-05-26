using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UGS_A_Weight : UGS_Action
{
    public bool unwalkable;

    public int weightModification;


    public CellType cellType;


    public override void Play(UGS_Grid grid)
    {
        Cell c = grid.GetCellFromType(cellType);

        if(c != null)
        {
            c.node.isWalkable = !unwalkable;
            c.node.weight = c.node.initialWeight + weightModification;
        }

    }

}
