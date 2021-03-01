using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    
    public Transform seeker;
    public Transform target;
    
    public LayerMask unwalkableMask;
    public LayerMask walkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private AStarNode[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector3 worldBottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward
            * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottonLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward
                    * (y * nodeDiameter + nodeRadius);
                bool walkable = (!Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)
                    && Physics.CheckSphere(worldPoint, nodeRadius, walkableMask));
                grid[x, y] = new AStarNode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<AStarNode> GetNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }

            }
        }

        return neighbours;
    }

    public AStarNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        /*
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        */
        float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z - transform.position.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

        return grid[x, y];
    }

    public List<AStarNode> path;

    private void OnDrawGizmos()
    {
        //Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //Gizmos.matrix = rotationMatrix;
        //Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        //Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (AStarNode n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                AStarNode seekNode = NodeFromWorldPoint(seeker.position);

                foreach (AStarNode n in grid)
                {
                    /*
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (playerNode == n)
                    {
                        Gizmos.color = Color.blue;
                    }
                    
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                    */
                    if (n.walkable)
                    {
                        Gizmos.color = Color.white;
                        if (path != null)
                        {
                            if (path.Contains(n))
                            {
                                Gizmos.color = Color.black;
                            }
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    
                        if (seekNode == n)
                        {
                            Gizmos.color = Color.blue;
                        
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
                }
            }
        }
    }
}
