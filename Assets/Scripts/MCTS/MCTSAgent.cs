using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for the movement of the agent
 * Author: Steven Ho
 * Date: 14-4-2021
 * Code version: 1.0
 */
public class MCTSAgent : MonoBehaviour
{
    public Transform target;
    public float speed = 1.2f;
    // Maximum speed for rotation, higher the number, less smoother the rotation
    public float maxRotationAngle = 90f;
    private List<MCTSNode> path;
    private int pathIndex = 0;

    public MCTSPathfinding pathfinding;
    private Vector3 targetPositionTemp;
    
    void Start()
    {
        path = new List<MCTSNode>();
        path.Add(new MCTSNode(true, transform.position, 0, 0));
        targetPositionTemp = Vector3.zero; // Initialise (0,0,0)
    }

    void Update()
    {   
        if (Vector3.Distance(target.position, transform.position) > 2)
        {
            UpdatePath(target.position);
            FollowPath();
        }
    }

    void UpdatePath(Vector3 target)
    {
        pathfinding.FindPath(transform.position, target);
        path = pathfinding.GetPath();
        pathIndex = 0;
        targetPositionTemp = target;
    }

    void FollowPath()
    {
        if (Vector3.Distance(transform.position, path[pathIndex].worldPosition) <= 1)
        {
            if (pathIndex < path.Count - 1)
            {
                pathIndex++;
            }
        }

        Vector3 nodeToMove = path[pathIndex].worldPosition;
        nodeToMove.y = transform.position.y;
        
        //Vector3 direction = (path[pathIndex].worldPosition - transform.position).normalized;
        Vector3 direction = (nodeToMove - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //transform.position += direction * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,
            nodeToMove, speed * Time.deltaTime);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            maxRotationAngle * Time.deltaTime);
    }
}