using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_M_Maps : UGS_Module
{
    public Gradient weightMapGradient;
    public Gradient fillMapGradient;
    public bool forceWeightBounds;
    public Vector2Int weightBounds;

    private void Start()
    {
        //FillMap();
    }

    public void WeightMap()
    {
        if(!forceWeightBounds) weightBounds = GetWeightBounds();

        foreach(Cell c in grid.cells)
        {
            c.SetBackgroundColor(weightMapGradient.Evaluate(Mathf.InverseLerp(weightBounds.x, weightBounds.y, c.node.weight)));
        }    
    }

    public void FillMap()
    {
        foreach (Cell c in grid.cells)
        {
            int i = c.Item<object>() == null ? 0 : 1;
            c.SetBackgroundColor(fillMapGradient.Evaluate(Mathf.InverseLerp(0, 1, i)));
        }
    }

    public Vector2Int GetWeightBounds()
    {
        int min = int.MaxValue;
        int max = int.MinValue;

        foreach(Cell c in grid.cells)
        {
            if (c.node.weight < min) min = c.node.weight;
            if (c.node.weight > max) max = c.node.weight;
        }

        return new Vector2Int(min, max);
    }
}
