using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BackToStandBy : AStarSeek
{

    public GameObject target;

    public float rotateSpeed;
    
    public override TaskStatus OnUpdate()
    {
        UpdatePath(target.transform.position);
        if (path.Count == 0 || Vector3.Equals(transform.position, target.transform.position))
        {
            transform.rotation = target.transform.rotation;
            return TaskStatus.Success;
        }
        else
        {
            FollowPath();
        }

        return TaskStatus.Running;
    }
}
