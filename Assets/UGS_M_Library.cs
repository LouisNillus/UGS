using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UGS_M_Library : UGS_Module
{
    
    public List<Cell[,]> savedGrids = new List<Cell[,]>();

    public void ExtractCurrentGrid()
    {
        Cell[,] extractedGrid = new Cell[grid.cells.GetLength(0), grid.cells.GetLength(1)];
        Array.Copy(grid.cells, extractedGrid, grid.cells.Length);

        savedGrids.Add(extractedGrid);
    }

    public void LoadSavedGrid(int index)
    {
        Array.Copy(savedGrids[index], grid.cells, savedGrids[index].Length);
    }
}
