using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//doubly linked list
public class Cell {
    public PBoid head;
    public Cell nextCell, previousCell;
    public Grid grid;

    public int numOfBoids = 0;
    Vector3 flockHeading = Vector3.zero;
    Vector3 flockCentre = Vector3.zero;

    public Vector3 flockHeadingTotal = Vector3.zero;
    public Vector3 flockCentreTotal = Vector3.zero;
    public int totalNumOfBoids = 0;

    public List<Cell> perceptionCells;
    public List<Cell> avoidCells;

    public Vector3 cellPosition;

    public void UpdateValues(Vector3 pos, Vector3 up)
    {
        flockCentre = new Vector3(pos.x + flockCentre.x, pos.y + flockCentre.y, 0);
        flockHeading = new Vector3 (up.x + flockHeading.x, up.y + flockHeading.y, 0);
    }

    public void ResetTotals()
    {
        flockCentreTotal = flockCentre;
        flockHeadingTotal = flockHeading;
        totalNumOfBoids = numOfBoids;
    }

    public void CalcValues()
    {
        flockCentre = Vector3.zero;
        flockHeading = Vector3.zero;
        numOfBoids = 0;
        PBoid b = head;
        while(b != null)
        {
            flockCentre += b.position;
            flockHeading += b.up;
            numOfBoids += 1;
            b = b.nextBoid;
        }
    }

    public void AddToCells()
    {
        foreach (Cell c in perceptionCells)
        {
            if (c.numOfBoids != 0)
            {
                c.Add(numOfBoids, flockHeading, flockCentre);
            }
        }
    }

    public void Add(int f, Vector3 h, Vector3 c)
    {
        totalNumOfBoids += f;
        flockHeadingTotal = new Vector3(h.x + flockHeadingTotal.x, h.y + flockHeadingTotal.y, 0);
        flockCentreTotal = new Vector3(c.x + flockCentreTotal.x, c.y + flockCentreTotal.y, 0);
    }

    public void AddBoid(PBoid b)
    {
        b.previousBoid = null;
        b.nextBoid = head;
        head = b;
        if (b.nextBoid != null)
        {
            b.nextBoid.previousBoid = b;
        }
        b.cell = this;
        numOfBoids++;
    }

    public void RemoveBoid(PBoid b)
    {
        if (b.previousBoid != null)
        {
            b.previousBoid.nextBoid = b.nextBoid;
        }

        if (b.nextBoid != null)
        {
            b.nextBoid.previousBoid = b.previousBoid;
        }

        if (head == b)
        {
            head = b.nextBoid;
        }
        numOfBoids--;
        if(numOfBoids == 0)
        {
            grid.RemoveCell(this);
        }
    }

    public void DebugBoidsInCell()
    {
        int cc = 0;
        PBoid b = head;
        while (b != null)
        {
            if (b == head)
                Debug.DrawLine(b.position, cellPosition, Color.yellow);
            else
                Debug.DrawLine(b.position, cellPosition);
            b = b.nextBoid;
            cc++;
        }
        Debug.Log("cell = " + cc);
    }

    public void DebugOtherCells()
    {
        foreach (Cell ac in perceptionCells)
        {
            Debug.DrawLine(cellPosition, ac.cellPosition, Color.blue);
        }
        foreach (Cell ac in avoidCells)
        {
            Debug.DrawLine(cellPosition, ac.cellPosition, Color.red);
        }
    }


}

public class Grid {
    float cellSize;
    Vector3 origin;

    public Cell[,] cells;
    public Cell filledCells;
    public int numOfFilled;

    public BoidSettings boidSettings;

    public void Initialize(float cs, Vector3 o, int s)
    {
        cellSize = cs;
        origin = o;
        int size = (int)(s / cs) + 1;

        cells = new Cell[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j] = new Cell();
                cells[i, j].grid = this;
                cells[i, j].cellPosition = new Vector3(origin.x + j * cellSize, origin.y - i * cellSize);
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j].perceptionCells = GetCellsInRadius(cellSize, i, j, boidSettings.perceptionRadius);
                cells[i, j].avoidCells = GetCellsInRadius(cellSize, i, j, boidSettings.avoidanceRadius);
                cells[i, j].avoidCells.Add(cells[i, j]);
            }
        }
    }

    public void AddBoidToGrid(PBoid b)
    {
        Cell cell = GetCellFromPosition(b.position);

        if(cell.head == null)
        {
            AddCell(cell);
        }

        cell.AddBoid(b);
    }

    public void Move(PBoid b) {

        Cell cell = GetCellFromPosition(b.position);

        //stays in cell
        if(cell == b.cell)
        {
            cell.UpdateValues(b.position - b.oldPosition, b.up - b.oldUp);
            return;
        }

        //remove boid from old cell
        b.cell.RemoveBoid(b);
        b.cell.UpdateValues(-b.oldPosition, -b.oldUp);

        //add new cell to filled if was empty
        if (cell.numOfBoids == 0)
        {
            AddCell(cell);
        }

        //add boid to new cell;
        cell.AddBoid(b);
        cell.UpdateValues(b.position, b.up);
    }

    public void AddCell(Cell c) {
        c.previousCell = null;
        c.nextCell = filledCells;
        filledCells = c;
        if (c.nextCell != null)
        {
            c.nextCell.previousCell = c;
        }
        numOfFilled++;
    }

    public void RemoveCell(Cell c)
    {
        if (c.previousCell != null)
        {
            c.previousCell.nextCell = c.nextCell;
        }

        if (c.nextCell != null)
        {
            c.nextCell.previousCell = c.previousCell;
        }

        if (filledCells == c)
        {
            filledCells = c.nextCell;
        }
        numOfFilled--;
    }

    public List<Cell> GetCellsInRadius(float cellSize, int r, int c, float radius)
    {
        int size = (int)(radius * 2 / cellSize);
        List<Cell> rtn = new List<Cell>();

        int startY, startX, endY, endX;
        startY = Mathf.Min(Mathf.Max(0, r - size / 2), cells.GetLength(0) - 1);
        startX = Mathf.Min(Mathf.Max(0, c - size / 2), cells.GetLength(1) - 1);
        endY = Mathf.Min(Mathf.Max(0, r + size / 2), cells.GetLength(0) - 1);
        endX = Mathf.Min(Mathf.Max(0, c + size / 2), cells.GetLength(1) - 1);

        for (int y = 0; startY + y <= endY; y++)
        {
            for (int x = 0; startX + x <= endX; x++)
            {
                if (!(startY + y == r && startX + x == c))
                    rtn.Add(cells[startY + y, startX + x]);
            }
        }

        return rtn;
    }

    private Cell GetCellFromPosition(Vector3 pos)
    {
        int cellY = (int)((origin.y - pos.y) / cellSize);
        int cellX = (int)((pos.x - origin.x) / cellSize);
        return cells[cellY, cellX];
    }

    public void DebugGridSize()
    {
        int l = cells.GetLength(0) - 1;
        Debug.DrawLine(cells[0, 0].cellPosition, cells[l, l].cellPosition);
        //Debug.DrawLine(cells[0, l].cellPosition, cells[l, 0].cellPosition);
    }

    public void DebugGridFilled()
    {
        int cc = 0;
        Cell c = filledCells;
        while (c != null)
        {
            //Debug.DrawRay(c.cellPosition, new Vector3(1, -1) * cellSize, Color.green);
            //c.DebugBoidsInCell();
            c.DebugOtherCells();
            c = c.nextCell;
            cc++;
        }
        //print("filled = " + cc);
    }
}

public class PartionedBoidController : MonoBehaviour
{

    //public BoidSettings 
    public Grid grid;
    
    List<PBoid> boids;

    public Transform target;//where to head toward?

    public int numBoids;
    public int maxFlockSize = 100;

    public bool DEBUG = false;
    
    void Update()
    {
        
    }

    public void FindAllBoids() {
        if(boids == null) {
            boids = new List<PBoid>();
        }
        
        boids.Clear();

        //boids.AddRange(FindObjectsOfType<PBoid>()); //cant do this because not monobehaviour

        numBoids = boids.Count;
        foreach(PBoid pb in boids) {
            pb.Initialize(this,null, target);
            grid.AddBoidToGrid(pb);
        }


    }

}
