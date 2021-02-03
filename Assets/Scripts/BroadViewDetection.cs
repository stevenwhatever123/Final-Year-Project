using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BroadViewDetection : MonoBehaviour
{
    private BehaviorTree behaviorTree;
    
    // Start is called before the first frame update
    void Start()
    {
        behaviorTree = gameObject.GetComponent<BehaviorTree>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            behaviorTree.SetVariableValue("PlayerDetected", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            behaviorTree.SetVariableValue("PlayerDetected", false);
        }
    }
}
