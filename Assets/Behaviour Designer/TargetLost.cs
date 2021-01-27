using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TargetLost : Action
{

    const string k_AnimAlertedParameter = "Alerted";

    public ParticleSystem[] onDetectVFX;

    public AudioClip onDetectSFX;

    public Animator animator;

    public override TaskStatus OnUpdate()
   {
       OnLostTarget();
       return TaskStatus.Success;
   }

   void OnLostTarget()
    {

        for (int i = 0; i < onDetectVFX.Length; i++)
        {
            onDetectVFX[i].Stop();
        }

        animator.SetBool(k_AnimAlertedParameter, false);
    }
}
