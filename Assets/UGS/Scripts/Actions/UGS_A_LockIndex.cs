using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_LockIndex : UGS_Action
{
    public bool lockAtMousePos;

    public LockType lockType;

    public override void Play(UGS_Grid grid)
    {
        if(lockAtMousePos)
        {
            grid.lockX = lockType == LockType.X;
            grid.lockY = lockType == LockType.Y;

            if(grid.hoveredCell != null)
            {
                grid.lockXIndex = grid.hoveredCell.gridPosition.x;
                grid.lockYIndex = grid.hoveredCell.gridPosition.y;
            }
        }
    }
}

public enum LockType {X, Y}
