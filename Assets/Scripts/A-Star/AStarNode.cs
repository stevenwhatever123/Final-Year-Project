using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for storing information of a node
 * Author: Sebastian Lague
 * Date: 26-2-2021
 * Code version: 1.4
 * Available at: https://github.com/SebLague/Pathfinding
 */
public class AStarNode : IHeapItem<AStarNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public AStarNode parent;

    private int heapIndex;

    public AStarNode(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost()
    {
        return gCost + hCost;
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

    public int CompareTo(AStarNode other)
    {
        int compare = fCost().CompareTo(other.fCost());
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return compare;
    }

    /*
    * This method returns is the node is walkable
    * Author: Steven Ho
    * Date: 26-2-2021
    */
    public bool IsWalkable()
    {
        return this.walkable;
    }
}
