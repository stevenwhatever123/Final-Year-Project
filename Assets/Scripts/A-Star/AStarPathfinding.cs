using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 * This class is responsible the calculation of pathfinding
 * Author: Sebastian Lague
 * Date: 26-2-2021
 * Code version: 1.6
 * Available at: https://github.com/SebLague/Pathfinding
 */
public class AStarPathfinding : MonoBehaviour
{
    public Transform seeker, target;

    private Vector3 seekerTemp, targetTemp;
    
    private AStarGrid grid;
    
    private float totalTime = 0;
    private int numOfRoute = 0;

    void Awake()
    {
        grid = GetComponent<AStarGrid>();
        seekerTemp = new Vector3(0, 0, 0);
        targetTemp = new Vector3(0, 0, 0);
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
                    
        AStarNode startNode = grid.NodeFromWorldPoint(startPos);
        AStarNode targetNode = grid.NodeFromWorldPoint(targetPos);
            
        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
        openSet.Add(startNode);
            
        while (openSet.Count > 0 && targetNode.walkable)
        {
            AStarNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost() < currentNode.fCost() || openSet[i].fCost() == currentNode.fCost()
                    && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                sw.Stop();
                totalTime += sw.ElapsedMilliseconds;
                numOfRoute++;
                print("Path found: " + sw.ElapsedMilliseconds + " ms");

                float average = totalTime / numOfRoute;
                Debug.Log("Average: " + average + " ms");
                
                
                RetracePath(startNode, targetNode);

                seekerTemp = seeker.position;
                targetTemp = target.position;
                return;
            }
            
            foreach (AStarNode neighbour in grid.GetNeighbours(currentNode))
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

    public void RetracePath(AStarNode starNode, AStarNode targetNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = targetNode;

        while (currentNode != starNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();

        grid.path = path;
    }

    public int GetDistance(AStarNode nodeA, AStarNode nodeB)
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
    
    public List<AStarNode> GetPath()
    {
        return grid.path;
    }
    
    /*
    * This method returns a boolean if the destination is walkable
    * Author: Steven Ho
    */
    public bool IsWalkable(Vector3 destination)
    {
        return grid.NodeFromWorldPoint(destination).IsWalkable();
    }
}
