using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

/*
 * This class is responsible sound detection of the AI
 * Author: Steven Ho
 * Date: 24-2-2021
 * Code version: 1.0
 */
public class SoundDetection : MonoBehaviour
{
    public Collider[] colliders;

    public AStarPathfinding pathfinding;

    public int distanceToDetect = 6;

    private BehaviorTree behaviorTree;

    void Start()
    {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    // Update is called once per frame
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, 9);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                AudioSource playerAudioSource = colliders[i].GetComponent<AudioSource>();
                if (playerAudioSource.isPlaying)
                {
                    Vector3 targetPosition = colliders[i].transform.position;
                    Vector3 currentPosition = transform.position;
                    float footstepFrequency =
                        colliders[i].GetComponent<PlayerCharacterController>().footstepSFXFrequency;
                    
                    pathfinding.FindPath(currentPosition, targetPosition);
                    int distance = pathfinding.GetPath().Count;
                    if (distance <= distanceToDetect)
                    {
                        behaviorTree.SetVariableValue("PlayerDetected", true);
                        behaviorTree.SetVariableValue("player_position", targetPosition);
                    }
                }
            }
        }
    }
}
