using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for sending the destination information to the bots following
 * Author: Steven Ho
 * Date: 18-4-2021
 * Code version: 1.0
 */
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
