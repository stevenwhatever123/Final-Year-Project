using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/*
 * This class is responsible for triggering the attack animation and attack the target
 * Author: Steven Ho
 * Date: 27-1-2021
 * Code version: 1.0
 */
public class Attack : Action
{
    const string k_AnimAttackParameter = "Attack";

    public Animator animator;

    public WeaponController m_WeaponController;
    public EnemyController m_EnemyController;

    public SharedGameObject target;

    public Vector3 targetOffset;

    public override TaskStatus OnUpdate(){
        animator.SetTrigger(k_AnimAttackParameter);
        m_EnemyController.OrientTowards(target.Value.transform.position);
        m_EnemyController.TryAtack(target.Value.transform.position + targetOffset);
        return TaskStatus.Running;
    }
}
