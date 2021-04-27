using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for the movement of the agent
 * Author: Steven Ho
 * Date: 7-3-2021
 * Code version: 1.1
 */

public class AStarAgent : MonoBehaviour
{
    public Transform target;
    public float speed = 1.2f;
    private List<AStarNode> path;
    private int pathIndex = 0;

    public AStarPathfinding pathfinding;
    private Vector3 targetPositionTemp;
    
    void Start()
    {
        path[0] = new AStarNode(true, transform.position, 0, 0);
        targetPositionTemp = Vector3.zero; // Initialise (0,0,0)
    }

    void Update()
    {
        UpdatePath();
        FollowPath();
        targetPositionTemp = target.position;
    }

    void UpdatePath()
    {
        if (!target.position.Equals(targetPositionTemp))
        {
            pathfinding.FindPath(transform.position, target.position);
            path = pathfinding.GetPath();
        }
    }

    void FollowPath()
    {
        if (Vector3.Distance(transform.position, path[pathIndex].worldPosition) < 1)
        {
            pathIndex++;
        }
        
        Vector3 direction = (path[pathIndex].worldPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
