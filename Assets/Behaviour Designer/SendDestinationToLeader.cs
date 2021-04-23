using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

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