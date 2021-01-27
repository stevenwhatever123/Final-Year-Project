using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TargetDetect : Action
{
    const string k_AnimAlertedParameter = "Alerted";

    public ParticleSystem[] onDetectVFX;

    public AudioClip onDetectSFX;

    public Animator animator;

    public override TaskStatus OnUpdate()
   {
       OnDetectedTarget();
       return TaskStatus.Success;
   }

   void OnDetectedTarget()
    {

        for (int i = 0; i < onDetectVFX.Length; i++)
        {
            onDetectVFX[i].Play();
        }

        if (onDetectSFX)
        {
            AudioUtility.CreateSFX(onDetectSFX, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
        }

        animator.SetBool(k_AnimAlertedParameter, true);
    }
}
