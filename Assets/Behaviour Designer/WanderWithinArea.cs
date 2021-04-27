using UnityEngine;

/*
 * This class is responsible for wandering in a certain area using Unity's NavMesh
 * Author: Steven Ho
 * Date: 27-1-2021
 * Code version: 1.1
 */
namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Wander using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
    public class WanderWithinArea : NavMeshMovement
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

        public Vector3 offset;

        private float pauseTime;
        private float destinationReachTime;

        public MovableArea movableArea;

        // There is no success or fail state with wander - the agent will just keep wandering
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                // The agent should pause at the destination only if the max pause duration is greater than 0
                if (maxPauseDuration.Value > 0) {
                    if (destinationReachTime == -1) {
                        destinationReachTime = Time.time;
                        pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);
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
                validDestination = SamplePosition(destination) 
                    && CheckWithinACertainArea(destination, movableMaxZ, movableMinZ, 
                    movableMaxX, movableMinX);
                attempts--;
            }
            if (validDestination) {
                SetDestination(destination);
            } else {
                Vector3 position = new Vector3(Random.Range(movableMinX, movableMaxX), movableArea.point[0].position.y, 
                    Random.Range(movableMinZ, movableMaxZ));
                SetDestination(position);
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

            if(destination.x > movableMaxX + offset.x|| 
                destination.x < movableMinX - offset.x|| 
                destination.z > movableMaxZ + offset.z|| 
                destination.z < movableMinZ - offset.z){
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