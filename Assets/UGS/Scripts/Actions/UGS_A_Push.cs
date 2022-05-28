using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Push : UGS_Action
{
    public CellsLine cellsLineType;
    public int index;

    public PushDirection direction;

    public int step;

    public bool teleport;
    public bool includeBackground;




    public override void Play(UGS_Grid grid)
    {
        switch (cellsLineType)
        {
            case CellsLine.Row:

                Cell[] row = grid.GetRowAtIndex(index);
                for (int i = 0; i < row.Length; i++)
                {
                    Cell c = row[i];
                    grid.SwapCells(c, grid.GetCellAtPosition(c.gridPosition.x, c.gridPosition.y + (direction == PushDirection.Up ? step : -step)), includeBackground ? true : false);
                }

                break;
            case CellsLine.Column:

                Cell[] column = grid.GetColumnAtIndex(index);
                for (int i = 0; i < column.Length; i++)
                {
                    Cell c = column[i];
                    grid.SwapCells(grid.GetCellAtPosition(c.gridPosition.x + (direction == PushDirection.Right ? step : -step), c.gridPosition.y), c, includeBackground ? true : false);
                }
                break;
        }
    }
}

public enum CellsLine {Row, Column}
public enum PushDirection {Up, Down, Left, Right}
