using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AStarSearchTarget : AStarSeek
{
    //Search for target in last seen area
    public SharedFloat minWanderDistance = 20;
    //Maximum distance ahead of the current position to look ahead for a destination
    public SharedFloat maxWanderDistance = 20;
    //The amount that the agent rotates direction")]
    public SharedFloat wanderRate = 2;
    //The minimum length of time that the agent should pause at each destination
    public SharedFloat minPauseDuration = 0;
    //The maximum length of time that the agent should pause at each destination (zero to disable)
    public SharedFloat maxPauseDuration = 0;
    //The maximum number of retries per tick (set higher if using a slow tick time)
    public SharedInt targetRetries = 1;
    //If target is null then use the target position
    //public SharedVector3 targetPosition;
    //Length and width of a square area
    public float distance = 10;
    //Number of times to perform this action before exit
    public int numberOfTimes = 4;
    
    public Vector3 offset;

    private float pauseTime;
    private float destinationReachTime;

    private bool hasValidDestination = false;
    
    // Counting number of times
    private int counter = 0;

    // There is no success or fail state with wander - the agent will just keep wandering
    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, path[path.Count-1].worldPosition) < 1) {
            Debug.Log("GG");
            // The agent should pause at the destination only if the max pause duration is greater than 0
            if (maxPauseDuration.Value > 0) {
                if (destinationReachTime == -1) {
                    destinationReachTime = Time.time;
                    pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);

                    counter++;
                    Debug.Log("Counter: " + counter);

                    if(counter == numberOfTimes){
                        counter = 0;
                        return TaskStatus.Success;
                    }
                }
                if (destinationReachTime + pauseTime <= Time.time) {
                    // Only reset the time if a destination has been set.
                    if (TrySetTarget()) {
                        destinationReachTime = -1;
                    }
                }
            }
            FollowPath();
            Debug.Log("Moving");
        }

        return TaskStatus.Running;
    }
    
    private bool TrySetTarget()
        {
            //Vector3 currentPosition = transform.position;
            float currentPositionX = Target().x;
            float currentPositionZ = Target().z;
            // Four different points of a square
            float movableMaxZ = currentPositionZ + distance;
            float movableMinZ = currentPositionZ - distance;
            float movableMaxX = currentPositionX + distance;
            float movableMinX = currentPositionX - distance;

            var direction = transform.forward;
            var validDestination = false;
            var attempts = targetRetries.Value;
            var destination = transform.position;
            while (!validDestination && attempts > 0) {
                direction = direction + Random.insideUnitSphere * wanderRate.Value;
                destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
                validDestination = pathfinding.IsWalkable(destination) 
                                   && CheckWithinACertainArea(destination, movableMaxZ, movableMinZ, 
                                    movableMaxX, movableMinX);
                attempts--;
            }
            if (validDestination)
            {
                UpdatePath(destination);
                //FollowPath();
                //Debug.Log("Moving to " + destination);
            } else {
                Vector3 position = new Vector3(Random.Range(movableMinX, movableMaxX), transform.position.y, 
                    Random.Range(movableMinZ, movableMaxZ));
            }
            return validDestination;
        }

        private bool CheckWithinACertainArea(Vector3 destination, float movableMaxZ, float movableMinZ, 
            float movableMaxX, float movableMinX){

            //Vector3 currentPosition = transform.position;
            float currentPositionX = Target().x;
            float currentPositionZ = Target().z;
            // Four different points of a square
            movableMaxZ = currentPositionZ + distance;
            movableMinZ = currentPositionZ - distance;
            movableMaxX = currentPositionX + distance;
            movableMinX = currentPositionX - distance;

            if(destination.x > movableMaxX|| 
                destination.x < movableMinX|| 
                destination.z > movableMaxZ|| 
                destination.z < movableMinZ){
                return false;
            } else {
                return true;
            }
        }

        private Vector3 Target()
        {
            return targetPosition.Value;
        }
        
        void UpdatePath(Vector3 target)
        {
            pathfinding.FindPath(transform.position, target);
            path = pathfinding.GetPath();
            pathIndex = 0;
        }
        
        protected void FollowPath()
        {
            if (Vector3.Distance(transform.position, path[pathIndex].worldPosition) < 1)
            {
                if (pathIndex < path.Count - 1)
                {
                    pathIndex++;
                }
            }

            //Vector3 direction = (path[0].worldPosition - transform.position).normalized;
            Vector3 direction = (path[pathIndex].worldPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            //Debug.Log("Direction: " + direction);

            //Vector3 newPosition = transform.position + direction * speed;
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
                maxRotationAngle * Time.deltaTime);
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