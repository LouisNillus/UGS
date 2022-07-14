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
            grid.lockX = (lockType == LockType.X || lockType == LockType.XY);
            grid.lockY = (lockType == LockType.Y || lockType == LockType.XY);

            if(grid.hoveredCell != null)
            {
                if((lockType == LockType.X || lockType == LockType.XY))
                grid.lockXIndex = grid.hoveredCell.gridPosition.x;
                if((lockType == LockType.Y || lockType == LockType.XY))
                grid.lockYIndex = grid.hoveredCell.gridPosition.y;
            }
        }
    }
}

public enum LockType {Unlocked, X, Y, XY}
