using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SendDestinationToOthers : Action
{
    public SharedGameObjectList friends;

    public SharedVector3 CommonDestination;
    
    public override TaskStatus OnUpdate()
    {
        foreach(GameObject bot in friends.Value)
        {
            bot.GetComponent<BehaviorTree>().SetVariableValue("CommonDestination", CommonDestination);
        }

        return TaskStatus.Success;
    }
}
