using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AddSeenCounter : Action
{
    public SharedFloat seenCounter = 0.0f;

    public override TaskStatus OnUpdate(){
        seenCounter.Value += Time.deltaTime;
        Debug.Log((int) (seenCounter.Value % 60f));
        return TaskStatus.Running;
    }

    // Reset the public variables
    public override void OnReset()
    {
        seenCounter.Value = 0.0f;
    }
}
