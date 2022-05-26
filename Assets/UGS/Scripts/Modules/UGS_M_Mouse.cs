using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UGS_M_Mouse : UGS_Module
{
    Cell selectedCell;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) SelectAtMousePos();
    }

    public Vector2 MousePos2D()
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
    }

    public Vector2Int MousePos2DInt()
    {
        return new Vector2Int(Mathf.RoundToInt(MousePos2D().x), Mathf.RoundToInt(MousePos2D().y));
    }

    public void SelectCell(Cell c)
    {
        if(selectedCell != null) selectedCell.OnReleaseCell.Invoke(); //Release Event on last cell if it exists

        selectedCell = c; //Set the new cell
        c.OnSelectCell.Invoke(); //Select Event on new cell
    }

    public void SelectAtMousePos()
    {
        grid.lastCell = grid.selectedCell;
        grid.selectedCell = grid.GetCellFromMousePos();

        UGS_M_Pathfinding pathFinder = grid.FindModuleOfType<UGS_M_Pathfinding>();

        if(grid.selectedCell == null || grid.lastCell == null) return;

        List<UGS_Node> path = pathFinder.FindPath(grid.selectedCell.gridPosition, grid.lastCell.gridPosition);

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].cell.position.x, path[i].cell.position.y), new Vector3(path[i + 1].cell.position.x, path[i + 1].cell.position.y) * 1f, Color.red, 5f);
            }
        }
    }
}
