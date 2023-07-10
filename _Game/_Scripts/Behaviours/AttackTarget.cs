using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
public class AttackTarget : Conditional
{
    //public SharedVector3 targetPos;
    public override TaskStatus OnUpdate()
    {

        PlayerAttackManager attackManager= gameObject.GetComponent<PlayerAttackManager>();
        if (attackManager != null)
        {
            attackManager.Attack(AttackType.Light);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
