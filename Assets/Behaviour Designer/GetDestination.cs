using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible calculating a random destination
 * Author: Steven Ho
 * Date: 18-4-2021
 * Code version: 1.0
 */
public class GetDestination : Action
{
    private Vector3 destination = Vector3.zero;

    public SharedVector3 CommonDestination;

    public MovableArea movableArea;

    [UnityEngine.Tooltip("The maximum number of retries per tick (set higher if using a slow tick time)")]
    public SharedInt targetRetries = 1;

    public AStarPathfinding pathfinding;
    
    [UnityEngine.Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
    public SharedFloat minWanderDistance = 20;
    [UnityEngine.Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
    public SharedFloat maxWanderDistance = 20;
    
    private bool hasValidDestination = false;

    public override TaskStatus OnUpdate()
    {
        if (TrySetTarget())
        {
            CommonDestination.Value = destination; 
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
    
    private bool CheckWithinACertainArea(Vector3 destination, float movableMaxZ, float movableMinZ, 
        float movableMaxX, float movableMinX){

        for(int i = 0; i < movableArea.point.Count; i++){
            if(movableArea.point[i].position.z > movableMaxZ){
                movableMaxZ = movableArea.point[i].position.z;
            }
            if(movableArea.point[i].position.z < movableMinZ){
                movableMinZ = movableArea.point[i].position.z;
            }
            if(movableArea.point[i].position.x > movableMaxX){
                movableMaxX = movableArea.point[i].position.x;
            }
            if(movableArea.point[i].position.y < movableMaxX){
                movableMinX = movableArea.point[i].position.x;
            }
        }

        if(destination.x > movableMaxX|| 
           destination.x < movableMinX|| 
           destination.z > movableMaxZ|| 
           destination.z < movableMinZ){
            return false;
        } else {
            return true;
        }
    }
    
    private bool TrySetTarget()
    {
        float movableMaxZ = 0;
        float movableMinZ = 0;
        float movableMaxX = 0;
        float movableMinX = 0;

        for(int i = 0; i < movableArea.point.Count; i++){
            if(movableArea.point[i].position.z > movableMaxZ){
                movableMaxZ = movableArea.point[i].position.z;
            }
            if(movableArea.point[i].position.z < movableMinZ){
                movableMinZ = movableArea.point[i].position.z;
            }
            if(movableArea.point[i].position.x > movableMaxX){
                movableMaxX = movableArea.point[i].position.x;
            }
            if(movableArea.point[i].position.y < movableMaxX){
                movableMinX = movableArea.point[i].position.x;
            }
        }

        var direction = transform.forward;
        var validDestination = false;
        var attempts = targetRetries.Value;
        destination = transform.position;
        while (!validDestination && attempts > 0) {
            direction = direction + Random.insideUnitSphere;
            destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
            validDestination = pathfinding.IsWalkable(destination)
                               && CheckWithinACertainArea(destination, movableMaxZ, movableMinZ,
                                   movableMaxX, movableMinX)
                               && (Vector3.Distance(transform.position, destination) <= maxWanderDistance.Value
                                   || Vector3.Distance(transform.position, destination) >= minWanderDistance.Value);
            attempts--;
        }
        return validDestination;
    }
}
