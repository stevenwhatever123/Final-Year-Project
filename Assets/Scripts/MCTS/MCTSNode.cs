using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSNode: IHeapItem<MCTSNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    
    public int gCost;
    public int hCost;

    public MCTSNode parent;

    public List<MCTSNode> childs;

    private int heapIndex;

    public MCTSNode(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        childs = new List<MCTSNode>();
    }
    
    public int fCost()
    {
        return gCost + hCost;
    }


    public bool IsWalkable()
    {
        return this.walkable;
    }

    public void AddParent(MCTSNode parent)
    {
        this.parent = parent;
    }

    public void AddChild(MCTSNode child)
    {
        childs.Add(child);
    }
    
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(MCTSNode other)
    {
        int compare = fCost().CompareTo(other.fCost());
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return compare;
    }
}
