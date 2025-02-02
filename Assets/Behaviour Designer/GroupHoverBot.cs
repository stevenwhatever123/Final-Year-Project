﻿using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for adding all nearby bots to a list
 * Author: Steven Ho
 * Date: 18-2-2021
 * Code version: 1.0
 */
public class GroupHoverBot : Action
{
    public SharedGameObjectList friends;

    public Collider[] ObjectsAround;

    public override TaskStatus OnUpdate()
    {
        ObjectsAround = Physics.OverlapSphere(transform.position, 9);
        for (int i = 0; i < ObjectsAround.Length; i++)
        {
            if (ObjectsAround[i].CompareTag("Bot"))
            {
                if (!friends.Value.Contains(ObjectsAround[i].gameObject))
                {
                    ObjectsAround[i].GetComponent<BehaviorTree>().SetVariableValue("InGroup", true);
                    ObjectsAround[i].GetComponent<BehaviorTree>().SetVariableValue("LeaderToFollow", 
                        this.gameObject);
                    friends.Value.Add(ObjectsAround[i].gameObject);
                }
            }
        }
        
        
        return TaskStatus.Success;
    }
}
