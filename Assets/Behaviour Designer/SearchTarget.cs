using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{   
    [TaskDescription("Search for target in last seen area")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class SearchTarget : NavMeshMovement
    {
        [Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat minWanderDistance = 20;
        [Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat maxWanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 2;
        [Tooltip("The minimum length of time that the agent should pause at each destination")]
        public SharedFloat minPauseDuration = 0;
        [Tooltip("The maximum length of time that the agent should pause at each destination (zero to disable)")]
        public SharedFloat maxPauseDuration = 0;
        [Tooltip("The maximum number of retries per tick (set higher if using a slow tick time)")]
        public SharedInt targetRetries = 1;
        [Tooltip("Length and width of a square area")]
        public float distance = 10;
        [Tooltip("Number of times to perform this action before exit")]
        public int numberOfTimes = 4;

        public Vector3 offset;

        private float pauseTime;
        private float destinationReachTime;

        // Counting number of times
        private int counter = 0;

        // There is no success or fail state with wander - the agent will just keep wandering
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                // The agent should pause at the destination only if the max pause duration is greater than 0
                if (maxPauseDuration.Value > 0) {
                    if (destinationReachTime == -1) {
                        destinationReachTime = Time.time;
                        pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);

                        counter++;
                        Debug.Log("Counter: " + counter);

                        if(counter == numberOfTimes){
                            return TaskStatus.Success;
                        }
                    }
                    if (destinationReachTime + pauseTime <= Time.time) {
                        // Only reset the time if a destination has been set.
                        if (TrySetTarget()) {
                            destinationReachTime = -1;
                        }
                    }
                } else {
                    TrySetTarget();
                }
            
                /*
                if(counter == numberOfTimes){
                    return TaskStatus.Success;
                }
                */
            }
            return TaskStatus.Running;
        }

        private bool TrySetTarget()
        {
            Vector3 currentPosition = transform.position;
            float currentPositionX = currentPosition.x;
            float currentPositionZ = currentPosition.z;
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
                validDestination = SamplePosition(destination) 
                    && CheckWithinACertainArea(destination, movableMaxZ, movableMinZ, 
                    movableMaxX, movableMinX);
                attempts--;
            }
            if (validDestination) {
                SetDestination(destination);
            } else {
                Vector3 position = new Vector3(Random.Range(movableMinX, movableMaxX), transform.position.y, 
                    Random.Range(movableMinZ, movableMaxZ));
                SetDestination(position);
            }
            return validDestination;
        }

        private bool CheckWithinACertainArea(Vector3 destination, float movableMaxZ, float movableMinZ, 
            float movableMaxX, float movableMinX){

            Vector3 currentPosition = transform.position;
            float currentPositionX = currentPosition.x;
            float currentPositionZ = currentPosition.z;
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
}
