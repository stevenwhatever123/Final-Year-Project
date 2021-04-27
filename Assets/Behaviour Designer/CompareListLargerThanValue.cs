using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for comparing if the local variable list is larger than the assigned size
 * Author: Steven Ho
 * Date: 18-2-2021
 * Code version: 1.0
 */
public class CompareListLargerThanValue : Conditional
{
    public SharedGameObjectList list;

    public int size;
    
    public override TaskStatus OnUpdate()
    {
        return list.Value.Count > size ? TaskStatus.Success : TaskStatus.Failure;
    }
}
