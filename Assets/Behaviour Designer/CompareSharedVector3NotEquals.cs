using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for comparing if both Vector3 variables are not the same
 * Author: Steven Ho
 * Date: 27-1-2021
 * Code version: 1.0
 */
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