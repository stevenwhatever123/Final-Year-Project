using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible create grid in worldspace
 * Author: Sebastian Lague
 * Date: 14-4-2021
 * Code version: 1.1
 * Available at: https://github.com/SebLague/Pathfinding
 */
public class MCTSGrid : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    
    public Transform seeker;
    public Transform target;
    
    public LayerMask unwalkableMask;
    public LayerMask walkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private MCTSNode[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new MCTSNode[gridSizeX, gridSizeY];
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
                grid[x, y] = new MCTSNode(walkable, worldPoint, x, y);
            }
        }
    }
    
    public List<MCTSNode> GetNeighbours(MCTSNode node)
    {
        List<MCTSNode> neighbours = new List<MCTSNode>();

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
    
    public MCTSNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z - transform.position.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

        return grid[x, y];
    }
    
    public List<MCTSNode> path;
    
    /*
     * This method draws the grid on the editor
     * Author: Steven Ho
     */
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
                foreach (MCTSNode n in path)
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
                MCTSNode seekNode = NodeFromWorldPoint(seeker.position);
                MCTSNode targetNode = NodeFromWorldPoint(target.position);

                foreach (MCTSNode n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (seekNode == n)
                    {
                        Gizmos.color = Color.blue;
                    }

                    if (targetNode == n)
                    {
                        Gizmos.color = Color.green;
                    }
                    
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        
    }
}
