using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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