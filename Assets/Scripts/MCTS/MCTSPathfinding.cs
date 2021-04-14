using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MCTSPathfinding : MonoBehaviour
{
    public Transform seeker, target;

    private Vector3 seekerTemp, targetTemp;
    
    private MCTSGrid grid;

    private bool pathFind = false;


    private int gCostSum;
    private int averageGCost;

    void Awake()
    {
        grid = GetComponent<MCTSGrid>();
        seekerTemp = new Vector3(0, 0, 0);
        targetTemp = new Vector3(0, 0, 0);
    }
    
    void Update()
    {
        //FindPath(seeker.position, target.position);
    }
    
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
                    
        MCTSNode startNode = grid.NodeFromWorldPoint(startPos);
        MCTSNode targetNode = grid.NodeFromWorldPoint(targetPos);
            
        List<MCTSNode> openSet = new List<MCTSNode>();
        HashSet<MCTSNode> closedSet = new HashSet<MCTSNode>();
        openSet.Add(startNode);
            
        while (openSet.Count > 0 && targetNode.walkable && startNode.walkable)
        {
            MCTSNode currentNode = openSet[0];
            gCostSum = 0;
            
            for (int i = 1; i < openSet.Count; i++)
            {
                gCostSum += openSet[i].gCost;
            }
            
            averageGCost = gCostSum / openSet.Count;

            List<MCTSNode> availableNode = new List<MCTSNode>();
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].gCost <= averageGCost)
                {
                    availableNode.Add(openSet[i]);
                }
            }

            if (availableNode.Count > 1)
            {
                int randomIndex = Random.Range(1, availableNode.Count-1);
                currentNode = availableNode[randomIndex];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);

                seekerTemp = seeker.position;
                targetTemp = target.position;
                availableNode = null;
                return;
            }
            
            foreach (MCTSNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                { 
                    continue;
                }
            
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
    
    public void RetracePath(MCTSNode startNode, MCTSNode targetNode)
    {
        List<MCTSNode> path = new List<MCTSNode>();
        MCTSNode currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();

        grid.path = path;
    }
    
    public int GetDistance(MCTSNode nodeA, MCTSNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
    
    public List<MCTSNode> GetPath()
    {
        return grid.path;
    }

    public bool IsWalkable(Vector3 destination)
    {
        return grid.NodeFromWorldPoint(destination).IsWalkable();
    }
}
