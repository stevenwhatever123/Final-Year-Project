using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AddTogetherCounter : Action
{
    public SharedFloat togetherCounter = 0.0f;

    public override TaskStatus OnUpdate(){
        togetherCounter.Value += Time.deltaTime;
        return TaskStatus.Running;
    }

    // Reset the public variables
    public override void OnReset()
    {
        togetherCounter.Value = 0.0f;
    }
}