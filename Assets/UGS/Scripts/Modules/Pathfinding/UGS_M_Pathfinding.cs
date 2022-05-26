using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UGS_M_Pathfinding : UGS_Module
{
    UGS_Node[,] nodes;

    public int straightCost;
    public int diagonalCost;

    List<UGS_Node> openList;
    List<UGS_Node> closedList;

    List<UGS_Action> startActions = new List<UGS_Action>();

    public override void Initialize()
    {
        nodes = new UGS_Node[grid.dimensions.x, grid.dimensions.y];

        for (int i = 0; i < grid.dimensions.x; i++)
        {
            for (int j = 0; j < grid.dimensions.y; j++)
            {
                nodes[i,j] = new UGS_Node(i, j, grid.cells[i,j]);
            }
        }

        foreach(UGS_Action action in startActions)
        {
            action.Play(grid);
        }
    }

    public List<UGS_Node> FindPath(Vector2Int start, Vector2Int end)
    {
        UGS_Node startNode = nodes[start.x, start.y];
        UGS_Node endNode = nodes[end.x, end.y];

        if(startNode == null || endNode == null) return null;

        openList = new List<UGS_Node>() { startNode };
        closedList = new List<UGS_Node>();

        foreach (UGS_Node node in nodes)
        {
            node.startCost = int.MaxValue;
            node.previousNode = null;
            node.CalculateTotalCost();
        }

        startNode.startCost = 0;
        startNode.remainingCost = CostBetweenNodes(startNode, endNode);
        startNode.CalculateTotalCost();


        while (openList.Count > 0)
        {
            UGS_Node currentNode = GetLowestTotalCostNode();

            if (currentNode == endNode) return CalculatePath(endNode);
            else
            {
                openList.Remove(currentNode);
                closedList.Add(currentNode);
            }

            foreach (UGS_Node neighbour in NeighboursOf(currentNode))
            {

                if (!neighbour.isWalkable && !closedList.Contains(neighbour)) closedList.Add(neighbour);
                if (closedList.Contains(neighbour)) continue;

                int tentativeStartCost = currentNode.startCost + CostBetweenNodes(currentNode, neighbour);
                if(tentativeStartCost < neighbour.startCost)
                {
                    neighbour.previousNode = currentNode;
                    neighbour.startCost = tentativeStartCost;
                    neighbour.remainingCost = CostBetweenNodes(neighbour, endNode);
                    neighbour.CalculateTotalCost();

                    if(!openList.Contains(neighbour)) openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    public List<UGS_Node> CalculatePath(UGS_Node endNode)
    {
        List<UGS_Node> pathNodes = new List<UGS_Node>();

        pathNodes.Add(endNode);

        UGS_Node currentNode = endNode;

        while (currentNode.previousNode != null)
        {
            pathNodes.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        pathNodes.Reverse();

        return pathNodes;
    }

    public List<UGS_Node> NeighboursOf(UGS_Node node)
    {
        List<UGS_Node> result = new List<UGS_Node>();

        //Left
        if (node.pos.x - 1 >= 0)
        {
            result.Add(nodes[node.pos.x - 1, node.pos.y]);

            if (allowDiagonals)
            {
                if (node.pos.y - 1 >= 0) result.Add(nodes[node.pos.x - 1, node.pos.y - 1]);
                if (node.pos.y + 1 < grid.dimensions.y) result.Add(nodes[node.pos.x - 1, node.pos.y + 1]);
            }
        }

        //Right
        if (node.pos.x + 1 < grid.dimensions.x)
        {
            result.Add(nodes[node.pos.x + 1, node.pos.y]);

            if(allowDiagonals)
            {
                if (node.pos.y - 1 >= 0) result.Add(nodes[node.pos.x + 1, node.pos.y - 1]);
                if (node.pos.y + 1 < grid.dimensions.y) result.Add(nodes[node.pos.x + 1, node.pos.y + 1]);
            }
        }

        //Down
        if (node.pos.y - 1 >= 0) result.Add(nodes[node.pos.x, node.pos.y - 1]);
        
        //Up
        if (node.pos.y + 1 < grid.dimensions.y) result.Add(nodes[node.pos.x, node.pos.y + 1]);

        return result;
    }

    public bool allowDiagonals;
    public int CostBetweenNodes(UGS_Node from, UGS_Node to)
    {
        int xDist = Mathf.Abs(from.pos.x - to.pos.x);
        int yDist = Mathf.Abs(from.pos.y - to.pos.y);
        int remaining = Mathf.Abs(xDist - yDist);

        if(allowDiagonals)
        {            
            return diagonalCost * Mathf.Min(xDist, yDist) + straightCost * remaining + to.weight;
        }
        else
        {
            return straightCost * remaining + to.weight;
        }
    }

    public UGS_Node GetLowestTotalCostNode()
    {
        UGS_Node lowestCostNode = openList[0];

        foreach(UGS_Node node in openList)
        {
            if(node != openList[0] && node.totalCost < lowestCostNode.totalCost) lowestCostNode = node;
        }

        return lowestCostNode;
    }
}
