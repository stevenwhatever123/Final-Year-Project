using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CompareSharedVector3NotEquals : Conditional{
    public SharedVector3 variable;
    public SharedVector3 compareTo;

    public override TaskStatus OnUpdate()
    {
        return !variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }

    public override void OnReset()
    {
        variable = Vector3.zero;
        compareTo = Vector3.zero;
    }
}