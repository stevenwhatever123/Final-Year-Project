using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CompareListLargerThanValue : Conditional
{
    public SharedGameObjectList list;

    public int size;
    
    public override TaskStatus OnUpdate()
    {
        return list.Value.Count > size ? TaskStatus.Success : TaskStatus.Failure;
    }
}
