using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for sending the destination information to the leader
 * Author: Steven Ho
 * Date: 23-4-2021
 * Code version: 1.0
 */
public class SendDestinationToLeader : Action
{
    public SharedGameObject leader;

    public SharedVector3 CommonDestination;
    
    public override TaskStatus OnUpdate()
    {
        if (leader.Value != null)
        {
            leader.Value.GetComponent<BehaviorTree>().SetVariableValue("CommonDestination", CommonDestination);
        }

        return TaskStatus.Success;
    }
}