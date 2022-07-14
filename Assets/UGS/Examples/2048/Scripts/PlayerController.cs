using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game_2048
{
    public class PlayerController : MonoBehaviour
    {
    
    
        public static PlayerController instance;
    
        public UGS_Grid grid;
        public GameObject template;
        public Gradient boxesGradient;
    
        private void Awake()
        {
            instance = this;
        }
    
        private void Start()
        {
            if (grid == null) grid = FindObjectOfType<UGS_Grid>();

            Spawn();
            Spawn();
            StartCoroutine(RollTitle(1.5f, 0.05f));

        }
    
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PushLines(Direction.Left);
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                PushLines(Direction.Up);
    
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PushLines(Direction.Right);
    
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PushLines(Direction.Down);
            }
    
            if (Input.GetKeyDown(KeyCode.E)) Spawn();
        }
    
        public void Spawn()
        {
            Cell c = grid.GetRandomEmptyCell();
    
            if(c == null)
            {
                return;
            }
            
            BoxControl box = Instantiate(template, c.position, Quaternion.identity).GetComponent<BoxControl>();
    
            c.SetItem(box.gameObject);
    
        }
    
        int moved = 0;
    
        public void PushLines(Direction dir, bool secondPass = false)
        {
    
            if (secondPass == false) moved = 0;
    
            if(dir == Direction.Left)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(grid.cells[i,j].occupied)
                    {
                        Cell c = grid.cells[i, j];
                        while (grid.GetAdjacentCells(c, dir).Length > 0 && grid.GetAdjacentCells(c, dir)[0].occupied == false)
                        {
                            grid.SwapCells(c, dir, true);
                            moved++;
                        }
                    }

                }
            }
        }
            else if(dir == Direction.Right)
        {
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid.cells[i, j].occupied)
                    {
                        Cell c = grid.cells[i, j];
                        while (grid.GetAdjacentCells(c, dir).Length > 0 && grid.GetAdjacentCells(c, dir)[0].occupied == false)
                        {
                            grid.SwapCells(c, dir, true);
                            moved++;
                        }
                    }

                }
            }
        }
            else if (dir == Direction.Up)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 3; j >= 0; j--)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
                            while (grid.GetAdjacentCells(c, dir).Length > 0 && grid.GetAdjacentCells(c, dir)[0].occupied == false)
                            {
                                grid.SwapCells(c, dir, true);
                                moved++;
                            }
                        }
    
                    }
                }
            }
            else if (dir == Direction.Down)
            {
                for (int i = 3; i >= 0; i--)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
                            while (grid.GetAdjacentCells(c, dir).Length > 0 && grid.GetAdjacentCells(c, dir)[0].occupied == false)
                            {
                                grid.SwapCells(c, dir, true);
                                moved++;
                            }
                        }
    
                    }
                }
            }
    
            if (secondPass == false)
            {
                Merge(dir);
            }
            else
            {
                if (moved > 0) StartCoroutine(DelayedSpawn(0.2f));
                else if (moved == 0 && grid.GetEmptyCells().Count == 0 && !RemainingMoves())
                {
                    StopAllCoroutines();
                    StartCoroutine(GameOverScreen(1.5f, 0.05f));
                }
            }
        }
    
        public void Merge(Direction dir)
        {
            if (dir == Direction.Left)
            {
    
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
    
                            MergeCellWithNeighbour(c, Direction.Right);
                        }
                    }
                }
    
            }
            else if (dir == Direction.Right)
            {
                for (int i = 3; i >= 0; i--)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
    
    
                            MergeCellWithNeighbour(c, Direction.Left);
                        }
    
                    }
                }
            }
            else if (dir == Direction.Up)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 3; j >= 0; j--)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
    
                            MergeCellWithNeighbour(c, Direction.Down);
                        }
    
                    }
                }
            }
            else if (dir == Direction.Down)
            {
                for (int i = 3; i >= 0; i--)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid.cells[i, j].occupied)
                        {
                            Cell c = grid.cells[i, j];
    
                            MergeCellWithNeighbour(c, Direction.Up);
                        }
                    }
                }
            }
    
            PushLines(dir, true);
        }
    
        public void MergeCellWithNeighbour(Cell c, Direction dir)
        {
            if (grid.GetAdjacentCells(c, dir).Length > 0 && grid.GetAdjacentCells(c, dir)[0].occupied)
            {
                BoxControl thisBox = c.ItemGO().GetComponent<BoxControl>();
                BoxControl neighbour = grid.GetAdjacentCells(c, dir)[0].ItemGO().GetComponent<BoxControl>();
    
                if (thisBox.value == neighbour.value)
                {
                    thisBox.Double();
                    grid.GetAdjacentCells(c, dir)[0].Clear();
                    neighbour.Kill();
                    moved++;
    
                    score += thisBox.value;
                }
            }
    
            scoreText.text = "SCORE: " + score.ToString();
        }
    
        public IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        Spawn();
    }
    
        public void NewGame()
        {
            foreach(Cell c in grid.cells)
            {
                if (c.ItemGO() != null)
                {
                    c.ItemGO().GetComponent<BoxControl>().Kill();
                    c.Clear();
                }
            }
    
            score = 0;
            scoreText.text = "SCORE: " + score.ToString();
    
            Spawn();
            Spawn();
    
            StopAllCoroutines();
            StartCoroutine(RollTitle(1.5f, 0.05f));
        }
    
    
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI scoreText;
        int score;
        public IEnumerator RollTitle(float duration, float period)
        {
            float t = 0f;
    
            while(t < duration)
            {
                yield return new WaitForSeconds(period);
                titleText.text = Random.Range(1000, 10000).ToString();
                t += period;
            }
    
            titleText.text = "2048";
        }
    
        public IEnumerator GameOverScreen(float duration, float period)
        {
            float t = 0f;
    
            while (t < duration)
            {
                yield return new WaitForSeconds(period);
                titleText.text = Random.Range(100000000, 1000000000).ToString();
                t += period;
            }
    
            titleText.text = "GAME OVER";
        }
    
        public bool RemainingMoves()
        {
            foreach(Cell c in grid.GetOccupiedCells())
            {
                foreach(Cell x in grid.GetAdjacentCells(c, Direction.All))
                {
                    if(x.ItemGO() != null)
                    {
                        if(c.ItemGO().GetComponent<BoxControl>().value == x.ItemGO().GetComponent<BoxControl>().value) return true;
                    }
                }
            }
    
            return false;
        }
    
    }
}
