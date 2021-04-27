using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for checking if the assigned Gameobject is empty
 * Author: Steven Ho
 * Date: 23-2-2021
 * Code version: 1.0
 */
public class CheckGameObjectIsNull : Conditional{
    public SharedGameObject leader;

    public override TaskStatus OnUpdate()
    {
        Debug.Log("Leader + " + leader.Value);
        return leader.Value == null ? TaskStatus.Success : TaskStatus.Failure;
    }

}