using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Damaged : Conditional
{
    const string k_AnimOnDamagedParameter = "OnDamaged";

    public ParticleSystem[] randomHitSparks;

    public Animator animator;

    public override TaskStatus OnUpdate()
    {
        OnDamaged();
       return TaskStatus.Success;
    }

    void OnDamaged(){
        if (randomHitSparks.Length > 0)
        {
            int n = Random.Range(0, randomHitSparks.Length - 1);
            randomHitSparks[n].Play();
        }

        animator.SetTrigger(k_AnimOnDamagedParameter);
    }
    
}
