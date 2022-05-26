using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UGS_Grid : MonoBehaviour
{
    public UGS_ColorProfile colors;

    public Vector2Int dimensions;

    public Transform origin;
    public float cellsStep;

    public Cell[,] cells;
    public Transform cellsContainer;

    public Object template;


    public List<UGS_Module> modules = new List<UGS_Module>();

    public Cell selectedCell;
    public Cell lastCell;
    public Cell hoveredCell;

    public UGS_M_Mouse mouseTracker;

    public Vector2 cellsDimensions;

    public GridFocusPoint initialFocusPoint = GridFocusPoint.Middle;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
        InitializeModules();


        selectedCell = GetCellAtPosition(0, 0);

        Camera.main.transform.position = GetFocusPoint(initialFocusPoint).ChangeZ(Camera.main.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            FindModuleOfType<UGS_M_Maps>().WeightMap();

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) hoveredCell = GetCellFromMousePos();
    }


    public void InitializeGrid()
    {
        cells = new Cell[dimensions.x, dimensions.y];

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Cell c = new Cell();
                cells[x,y] = c;

                c.gridPosition = new Vector2Int(x,y);
                c.position = origin.position + new Vector3(x * cellsStep, y * cellsStep);

                Object go = Instantiate(template, c.position, Quaternion.identity);

                if (go is GameObject) (go as GameObject).transform.parent = cellsContainer;

                c.SetItem(Random.value > 0.9f ? new GameObject() : null);

                c.SetBackground(go);

                c.Initialize();
            }
        }
    }

    public Cell GetCellAtPosition(int x, int y)
    {
        return cells[x,y];
    }

    public Cell GetCellFromType(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Selected:
                return selectedCell;
            case CellType.Hovered:
                return hoveredCell;
            case CellType.Last:
                return lastCell;
        }

        return null;
    }

    private void OnDrawGizmos()
    {

        if (colors == null) return;

        Gizmos.color = colors.emptyCellColor;

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                if(Application.isPlaying && cells.Length > 0)
                {
                    /*GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.black;
                    Handles.Label(cells[x, y].position, cells[x, y].gridPosition.ToString(), style);*/

                    Gizmos.color = cells[x, y].occupied ? Gizmos.color = colors.fullCellColor : colors.emptyCellColor;

                    if (Methods.InBoundsXY(cells[x, y].position, mouseTracker.MousePos2D(), cellsDimensions.x/2, cellsDimensions.y/2)) Gizmos.color = Color.blue;

                    if (selectedCell != null && selectedCell.gridPosition == new Vector2(x, y)) Gizmos.color = Color.black;
                }

                Gizmos.DrawWireCube(origin.position + new Vector3(x * cellsStep, y * cellsStep), cellsDimensions);
            }
        }

    }



    public Cell GetCellFromMousePos()
    {
        Cell result = null;

        foreach (Cell cell in cells)
        {
            if (Methods.InBoundsXY(cell.position, mouseTracker.MousePos2D(), cellsDimensions.x / 2, cellsDimensions.y / 2)) return cell;
        }

        return result;
    }


    #region Focus
    public void FocusCamera(Vector3 point, Camera cam = null)
    {
        if(cam != null) cam.transform.position = point + new Vector3(0, 0, cam.transform.position.z);        
        else Camera.main.transform.position = point + new Vector3(0, 0, Camera.main.transform.position.z);
    }

    public Vector3 GetFocusPoint(GridFocusPoint focuspoint = GridFocusPoint.Middle)
    {

        int dX = dimensions.x - 1;
        int dY = dimensions.y - 1;

        bool xEven = dimensions.x % 2 == 0;
        bool yEven = dimensions.y % 2 == 0;

        switch (focuspoint)
        {
            default: return cells[0, (int)Mathf.Floor(dY / 2)].position + Vector3.up * (yEven ? (cellsStep / 2f) : 0f); // Middle

            case GridFocusPoint.Top:
                return cells[(int)Mathf.Floor(dX / 2), dY].position + Vector3.right * (xEven ? (cellsStep / 2f) : 0f);
            case GridFocusPoint.TopLeft:
                return cells[0, dY].position;
            case GridFocusPoint.TopRight:
                return cells[dX, dY].position;
            case GridFocusPoint.Middle:
                return cells[(int)Mathf.Floor(dX / 2), (int)Mathf.Floor(dY / 2)].position + Vector3.right * (xEven ? (cellsStep / 2f) : 0f) + Vector3.up * (yEven ? (cellsStep / 2f) : 0f);
            case GridFocusPoint.MiddleLeft:
                return cells[0, (int)Mathf.Floor(dY / 2)].position + Vector3.up * (yEven ? (cellsStep / 2f) : 0f);
            case GridFocusPoint.MiddleRight:
                return cells[dX, (int)Mathf.Floor(dY / 2)].position + Vector3.up * (yEven ? (cellsStep / 2f) : 0f);
            case GridFocusPoint.Bottom:
                return cells[(int)Mathf.Floor(dX / 2), 0].position + Vector3.right * (xEven ? (cellsStep / 2f) : 0f);
            case GridFocusPoint.BottomLeft:
                return cells[0, 0].position;
            case GridFocusPoint.BottomRight:
                return cells[dX, 0].position;
        }
    }

    #endregion

    #region Adjacents

    public Cell[] GetAdjacentCells(Cell origin, int depth = 1, bool includeSelf = false)
    {

        List<Cell> result = new List<Cell>();

        for (int i = 1; i <= depth; i++)
        {
            int left = origin.gridPosition.x - i;
            int right = origin.gridPosition.x + i;
            int down = origin.gridPosition.y - i;
            int up = origin.gridPosition.y + i;


            if (left >= 0 && left < dimensions.x) result.Add<Cell>(cells[left, origin.gridPosition.y]);
            if (right >= 0 && right < dimensions.x) result.Add<Cell>(cells[right, origin.gridPosition.y]);
            if (down >= 0 && down < dimensions.y) result.Add<Cell>(cells[origin.gridPosition.x, down]);
            if (up >= 0 && up < dimensions.y) result.Add<Cell>(cells[origin.gridPosition.x, up]);

            if (includeSelf) result.Add(origin);
        }

        return result.ToArray();
    }

    public Cell GetCellFromDirection(Cell origin, Direction direction, int depth = 1, bool returnSelf = true)
    {
        int left = origin.gridPosition.x - depth;
        int right = origin.gridPosition.x + depth;
        int down = origin.gridPosition.y - depth;
        int up = origin.gridPosition.y + depth;

        switch (direction)
        {
            case Direction.Left: if (left >= 0 && left < dimensions.x) return cells[left, origin.gridPosition.y]; else break;
            case Direction.Right: if (right >= 0 && right < dimensions.x) return cells[right, origin.gridPosition.y]; else break;
            case Direction.Down: if (down >= 0 && down < dimensions.y) return cells[origin.gridPosition.x, down]; else break;
            case Direction.Up: if (up >= 0 && up < dimensions.y) return cells[origin.gridPosition.x, up]; else break;
        }

        if (returnSelf) return origin;
        else return null;
    }

    public Cell[] GetAdjacentCells(Vector3 position, int depth = 1)
    {
        return null;
    }
    
    public Cell[] GetAdjacentCells(Vector2Int coordinates, int depth = 1)
    {
        return null;
    }

    #endregion

    #region Swaps

    public void SwapCells(Cell a, Cell b, bool includeBackground = false)
    {
        object itemA = a.Item<object>();
        object itemB = b.Item<object>();

        a.SetItem(itemB);
        b.SetItem(itemA);

        if(includeBackground)
        {
            object backgroundA = a.Background<object>();
            object backgroundB = b.Background<object>();

            a.SetBackground(backgroundB);
            b.SetBackground(backgroundA);
        }

    }
    #endregion

    #region Modules

    public T FindModuleOfType<T>() where T : Object
    {
        var type = typeof(T);

        foreach (UGS_Module m in modules)
        {
            if (m is T) return m as T;
        }
        
        Debug.LogWarning("Can't find any module of type : " + type);
        return null;
    }

    public void InitializeModules()
    {
        foreach (UGS_Module m in modules)
        {
            m.grid = this;
            m.Initialize();
        }
    }

    #endregion
}

public enum GridFocusPoint
{
    Top,
    TopLeft,
    TopRight,
    Middle,
    MiddleLeft,
    MiddleRight,
    Bottom,
    BottomLeft,
    BottomRight
}

public enum CellType { Selected, Hovered, Last }