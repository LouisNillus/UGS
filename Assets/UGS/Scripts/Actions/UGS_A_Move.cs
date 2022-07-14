using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Move : UGS_Action
{

    public Direction direction;

    [Min(1)]
    public int range = 1;

    public override void Play(UGS_Grid grid)
    {
        grid.selectedCell = grid.GetCellFromDirection(grid.selectedCell, direction);
    }
}
public enum Direction { Up, Down, Left, Right, All }
