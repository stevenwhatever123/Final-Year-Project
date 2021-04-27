using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

/*
 * This class is responsible broad view detection
 * Author: Steven Ho
 * Date: 25-4-2021
 * Code version: 1.0
 */
public class BroadView : MonoBehaviour
{
    public Collider[] hit;

    public Vector3 position;

    private Vector3 center;

    public float radius;

    public float height;

    public BehaviorTree behaviorTree;

    void Start()
    {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    // Update is called once per frame
    void Update()
    {
        center = transform.position + position;
        
        Vector3 p1 = center - Vector3.forward * height/2;
        Vector3 p2 = center + Vector3.forward * height/2;
        
        hit = Physics.OverlapCapsule(p1, p2, radius);

        GameObject closestHit = this.gameObject;
        float closestHitDistance = Mathf.Infinity;
        bool foundValidHit = false;
        
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].gameObject.name.CompareTo("HitBox") != 0)
            {
                float distance = Vector3.Distance(hit[i].transform.position, transform.position);
                if (distance < closestHitDistance)
                {
                    closestHitDistance = distance;
                    closestHit = hit[i].gameObject;
                    foundValidHit = true;
                }
            }
        }

        if (foundValidHit)
        {
            if (closestHit.gameObject.CompareTag("Player"))
            {
                behaviorTree.SetVariableValue("PlayerDetected", true);
            }
        }
    }
}
