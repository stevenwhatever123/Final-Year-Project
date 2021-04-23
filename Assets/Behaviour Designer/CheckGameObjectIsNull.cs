using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckGameObjectIsNull : Conditional{
    public SharedGameObject leader;

    public override TaskStatus OnUpdate()
    {
        Debug.Log("Leader + " + leader.Value);
        return leader.Value == null ? TaskStatus.Success : TaskStatus.Failure;
    }

}