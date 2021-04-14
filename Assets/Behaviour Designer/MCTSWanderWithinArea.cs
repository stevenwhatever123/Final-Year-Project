using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MCTSWanderWithinArea : MCTSSeek
{
    [UnityEngine.Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
    public SharedFloat minWanderDistance = 20;
    [UnityEngine.Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
    public SharedFloat maxWanderDistance = 20;
    [UnityEngine.Tooltip("The amount that the agent rotates direction")]
    public SharedFloat wanderRate = 2;
    [UnityEngine.Tooltip("The minimum length of time that the agent should pause at each destination")]
    public SharedFloat minPauseDuration = 0;
    [UnityEngine.Tooltip("The maximum length of time that the agent should pause at each destination (zero to disable)")]
    public SharedFloat maxPauseDuration = 0;
    [UnityEngine.Tooltip("The maximum number of retries per tick (set higher if using a slow tick time)")]
    public SharedInt targetRetries = 1;

    public Vector3 offset;

    private float pauseTime;
    private float destinationReachTime;

    public MovableArea movableArea;
    
    private bool hasValidDestination = false;
    
    // Counting number of times
    private int counter = 0;
    
    public override TaskStatus OnUpdate()
    {
        if (!hasValidDestination)
        {
            if (TrySetTarget())
            {
                hasValidDestination = true;
            }
        }
        else
        {
            if (path.Count > 0)
            {
                if (Vector3.Distance(transform.position, path[path.Count - 1].worldPosition) < 2)
                {
                    if (maxPauseDuration.Value > 0) {
                        if (destinationReachTime == -1) {
                            destinationReachTime = Time.time;
                            pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);

                            counter++;
                            Debug.Log("Counter: " + counter);
                        }
                        if (destinationReachTime + pauseTime <= Time.time)
                        {
                            hasValidDestination = false;
                            // Only reset the time if a destination has been set.
                            destinationReachTime = -1;
                        }
                    }
                }
                else
                {
                    FollowPath();
                }
            }
        }
        return TaskStatus.Running;
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
        var destination = transform.position;
        while (!validDestination && attempts > 0) {
            direction = direction + Random.insideUnitSphere * wanderRate.Value;
            destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
            validDestination = pathfinding.IsWalkable(destination) 
                               && CheckWithinACertainArea(destination, movableMaxZ, movableMinZ, 
                                   movableMaxX, movableMinX) 
                               && (Vector3.Distance(transform.position, destination) <= maxWanderDistance.Value
                                    || Vector3.Distance(transform.position, destination) >= minWanderDistance.Value);
            attempts--;
        }
        if (validDestination)
        {
            UpdatePath(destination);
            Debug.Log("Moving to " + destination);
        }
        return validDestination;
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

    // Reset the public variables
    public override void OnReset()
    {
        minWanderDistance = 20;
        maxWanderDistance = 20;
        wanderRate = 2;
        minPauseDuration = 0;
        maxPauseDuration = 0;
        targetRetries = 1;
    }
}
