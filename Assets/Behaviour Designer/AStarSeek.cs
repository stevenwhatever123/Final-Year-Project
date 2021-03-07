using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AStarSeek : Action
{
    // The GameObject that the agent is seeking
    public GameObject target;

    public AStarPathfinding pathfinding;
    
    // Speed for movement
    public float speed = 1.2f;
    // Maximum speed for rotation, higher the number, less smoother the rotation
    public float maxRotationAngle = 90f;
    
    // Path to the target
    private List<AStarNode> path;
    // Index for the path list
    private int pathIndex = 0;
    
    // Temp vector3 to store target previous position
    private Vector3 targetPositionTemp;

    public override void OnStart()
    {
        base.OnStart();
        path[0] = new AStarNode(true, transform.position, 0, 0);
        targetPositionTemp = Vector3.zero; // Initialise (0,0,0)
    }


    public override TaskStatus OnUpdate(){
        if (Vector3.Distance(transform.position, target.transform.position) < 1)
        {
            return TaskStatus.Success;
        }
        UpdatePath();
        FollowPath();
        targetPositionTemp = target.transform.position;
        return TaskStatus.Running;
    }

    public override void OnReset()
    {
        base.OnReset();
        target = null;
    }
    
    void UpdatePath()
    {
        // Calculate the path whenever the target moves
        if (!target.transform.position.Equals(targetPositionTemp))
        {
            pathfinding.FindPath(transform.position, target.transform.position);
            path = pathfinding.GetPath();
        }
    }
    
    // Method for following the path
    void FollowPath()
    {
        if (Vector3.Distance(transform.position, path[pathIndex].worldPosition) < 1)
        {
            if (pathIndex != path.Count - 1)
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
        //transform.rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            maxRotationAngle * Time.deltaTime);
    }
}
