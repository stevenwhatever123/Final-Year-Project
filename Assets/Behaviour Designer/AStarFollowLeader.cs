﻿using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

/*
 * This class is responsible for the movement of the AI
 * Author: Steven Ho
 * Date: 23-2-2021
 * Code version: 1.0
 */
public class AStarFollowLeader : Action
{
    // The GameObject that the agent is seeking
    public SharedGameObject target;

    public SharedVector3 targetPosition;

    public AStarPathfinding pathfinding;
    
    // Speed for movement
    public float speed = 1.2f;
    // Maximum speed for rotation, higher the number, less smoother the rotation
    public float maxRotationAngle = 90f;
    
    // Path to the target
    protected List<AStarNode> path;
    // Index for the path list
    protected int pathIndex = 0;
    
    // Temp vector3 to store target previous position
    protected Vector3 targetPositionTemp;

    public float distanceBetweenLeader = 4f;

    public override void OnStart()
    {
        base.OnStart();
        path = new List<AStarNode>();
        //path[0] = new AStarNode(true, transform.position, 0, 0);
        path.Add(new AStarNode(true, transform.position, 0, 0));
        targetPositionTemp = Vector3.zero; // Initialise (0,0,0)
    }


    public override TaskStatus OnUpdate(){
        //UpdatePath(targetPosition.Value);
        UpdatePath(Target());
        if (path.Count == 0 || Vector3.Distance(transform.position, path[path.Count-1].worldPosition) <= distanceBetweenLeader)
        {
            Debug.Log("I get you bitch");
            return TaskStatus.Success;
        }
        FollowPath();
        return TaskStatus.Running;
    }

    public override void OnReset()
    {
        base.OnReset();
        target = null;
    }
    
    protected Vector3 Target()
    {
        if (target != null) {
            return target.Value.transform.position;
        }
        return targetPosition.Value;
    }
    
    protected void UpdatePath(Vector3 target)
    {
        // Calculate the path whenever the target moves
        pathfinding.FindPath(transform.position, target);
        path = pathfinding.GetPath();
        pathIndex = 0;
        targetPositionTemp = target;
    }
    
    // Method for following the path
    protected void FollowPath()
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
        
        Debug.Log("Current Position: " + transform.position);
        Debug.Log("Next Position: " + nodeToMove);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //transform.position += direction * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,
            nodeToMove, speed * Time.deltaTime);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            maxRotationAngle * Time.deltaTime);


        Debug.Log("Total Index: " + path.Count);
        Debug.Log("Current Index: " + pathIndex);
    }
}
