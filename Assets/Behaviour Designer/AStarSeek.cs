using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AStarSeek : Action
{
    // The GameObject that the agent is seeking
    public GameObject target;

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

    public override void OnStart()
    {
        base.OnStart();
        path = new List<AStarNode>();
        //path[0] = new AStarNode(true, transform.position, 0, 0);
        path.Add(new AStarNode(true, transform.position, 0, 0));
        targetPositionTemp = Vector3.zero; // Initialise (0,0,0)
    }


    public override TaskStatus OnUpdate(){
        if (Vector3.Distance(transform.position, Target()) < 2)
        {
            return TaskStatus.Success;
        }
        UpdatePath(targetPosition.Value);
        FollowPath();
        //targetPositionTemp = target.transform.position;
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
            return target.transform.position;
        }
        return targetPosition.Value;
    }
    
    protected void UpdatePath(Vector3 target)
    {
        // Calculate the path whenever the target moves
        if (!target.Equals(targetPositionTemp))
        {
            pathfinding.FindPath(transform.position, target);
            path = pathfinding.GetPath();
            pathIndex = 0;
            targetPositionTemp = target;
        }
    }
    
    // Method for following the path
    protected void FollowPath()
    {
        if (Vector3.Distance(transform.position, path[pathIndex].worldPosition) < 2)
        {
            if (pathIndex < path.Count - 2)
            {
                pathIndex++;
            }
        }

        //Vector3 direction = (path[0].worldPosition - transform.position).normalized;
        Vector3 direction = (path[pathIndex].worldPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //Vector3 targetDirection = (Target() - transform.position).normalized;
        //Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        
        //Debug.Log("Direction: " + direction);

        //Vector3 newPosition = transform.position + direction * speed;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            maxRotationAngle * Time.deltaTime);
    }
}
