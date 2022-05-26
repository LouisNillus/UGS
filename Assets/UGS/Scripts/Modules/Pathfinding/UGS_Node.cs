using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UGS_Node
{

    public Vector2Int pos;
    [System.NonSerialized] public Cell cell;

    
    public UGS_Node previousNode;

    public int startCost;
    public int remainingCost;

    public int totalCost;

    public int initialWeight;
    public int weight;
    public bool isWalkable;


    public UGS_Node(int x, int y, Cell cell)
    {
        pos.x = x;
        pos.y = y;
        this.cell = cell;
        cell.node = this;
        isWalkable = true;
        initialWeight = weight;
    }

    public void CalculateTotalCost()
    {
        totalCost = startCost + remainingCost;
    }

}
