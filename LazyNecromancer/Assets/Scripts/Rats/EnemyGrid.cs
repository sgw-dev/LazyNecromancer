using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : MonoBehaviour {

    public bool ShowCells;
    float cellSize;
    Vector3 origin;

    public EnemyCell[,] cells;
    public EnemyCell filledCells;
    public int numOfFilled;

    public BoidSettings boidSettings;

    [HideInInspector] public Vector3 corner; 
    [HideInInspector] public float Xmin,Xmax,Ymin,Ymax;
    
    public void Initialize(int s,float xmin,float xmax,float ymin,float ymax)
    {
        Xmin=xmin;
        Xmax=xmax;
        Ymin=ymin;
        Ymax=ymax;
        
        corner = new Vector3(xmin,ymax) + transform.position;
        origin = corner;

        int size = s;
        cellSize = ((xmax-xmin)/(float)s);
        float halfsize = cellSize/2f;

        cells = new EnemyCell[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j] = new EnemyCell();
                cells[i, j].grid = this;
                //cells[i, j].cellPosition = new Vector3(origin.x + j * cellSize, origin.y - i * cellSize);
                cells[i, j].cellPosition = new Vector3(j * cellSize + halfsize,  -i * cellSize - halfsize) + corner;
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j].perceptionCells = GetCellsInRadius(cellSize, i, j, boidSettings.perceptionRadius);//larger 2.5
                cells[i, j].avoidCells = GetCellsInRadius(cellSize, i, j, boidSettings.avoidanceRadius);//smaller 1
                cells[i, j].avoidCells.Add(cells[i, j]);
            }
        }
    }

    public void AddBoidToGrid(EnemyBoid b)
    {
        EnemyCell cell = GetCellFromPosition(b.position);

        if(cell.head == null)
        {
            AddCell(cell);
        }

        cell.AddBoid(b);
    }


    public void Move(EnemyBoid b) {

        EnemyCell cell = GetCellFromPosition(b.position);

        //stays in cell
        if(cell == b.cell)
        {
            cell.UpdateValues(b.position - b.oldPosition, Vector3.zero);
            return;
        }

        //remove boid from old cell
        b.cell.RemoveBoid(b);
        b.cell.UpdateValues(-b.oldPosition, Vector3.zero);

        //add new cell to filled if was empty
        if (cell.numOfBoids == 0)
        {
            AddCell(cell);
        }

        //add boid to new cell;
        cell.AddBoid(b);
        cell.UpdateValues(b.position, b.up);
    }

    public void AddCell(EnemyCell c) {
        c.previousCell = null;
        c.nextCell = filledCells;
        filledCells = c;
        if (c.nextCell != null)
        {
            c.nextCell.previousCell = c;
        }
        numOfFilled++;
    }

    public void RemoveCell(EnemyCell c)
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

    public List<EnemyCell> GetCellsInRadius(float cellSize, int r, int c, float radius)
    {
        int size = (int)(radius * 2 / cellSize);
        List<EnemyCell> rtn = new List<EnemyCell>();

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

    private EnemyCell GetCellFromPosition(Vector3 pos)
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
        EnemyCell c = filledCells;
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

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        //show the bounds for the grid
        Vector3 topleft_  = new Vector3(Xmin,Ymax,0) + transform.position,
        topright_ = new Vector3(Xmax,Ymax,0) + transform.position,
        botleft_  = new Vector3(Xmin,Ymin,0) + transform.position,
        botright_ = new Vector3(Xmax,Ymin,0) + transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(topleft_,topright_);
        Gizmos.DrawLine(topright_,botright_);
        Gizmos.DrawLine(botright_,botleft_);
        Gizmos.DrawLine(botleft_,topleft_);

        //show the grid
        if(cells == null || !ShowCells) {
            return;
        }
    
        Gizmos.color = Color.red;
        
        float h = cellSize/2;
        corner = new Vector3(Xmin,Ymax) + transform.position;

        for(int i = 0 ; i < cells.GetLength(0); i++) {
            for(int j = 0; j < cells.GetLength(1) ; j++ ) {

                Vector3 topleft  = new Vector3(j * cellSize           , -1*cellSize*i           ) + corner;
                Vector3 topright = new Vector3(j * cellSize + cellSize, -1*cellSize*i           ) + corner;
                Vector3 botright = new Vector3(j * cellSize + cellSize, -1*cellSize*i - cellSize) + corner;
                Vector3 botleft  = new Vector3(j * cellSize           , -1*cellSize*i - cellSize) + corner;
            
                Gizmos.DrawLine(topleft,topright);
                Gizmos.DrawLine(topright,botright);
                Gizmos.DrawLine(botright,botleft);
                Gizmos.DrawLine(botleft,topleft);
            }
        }
    }
#endif

}

public class EnemyCell {
    public EnemyBoid head;
    public EnemyCell nextCell, previousCell;
    public EnemyGrid grid;

    public int numOfBoids = 0;
    Vector3 flockHeading = Vector3.zero;
    Vector3 flockCentre = Vector3.zero;

    public Vector3 flockHeadingTotal = Vector3.zero;
    public Vector3 flockCentreTotal = Vector3.zero;
    public int totalNumOfBoids = 0;

    public List<EnemyCell> perceptionCells;
    public List<EnemyCell> avoidCells;

    public Vector3 cellPosition;


    public Vector3 cellFlockCohesion;
    public Vector3 cellFlockAvoidance;
    public Vector3 cellFlockSeparation;

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

    public void AddToCells()
    {
        foreach (EnemyCell c in perceptionCells)
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

    public void AddBoid(EnemyBoid b)
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

    public void RemoveBoid(EnemyBoid b)
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
        EnemyBoid b = head;
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
        foreach (EnemyCell ac in perceptionCells)
        {
            Debug.DrawLine(cellPosition, ac.cellPosition, Color.blue);
        }
        foreach (EnemyCell ac in avoidCells)
        {
            Debug.DrawLine(cellPosition, ac.cellPosition, Color.red);
        }
    }


}