using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
public class MoveToTargetPos : Conditional
{
    public SharedVector3 targetPos;
    public override TaskStatus OnUpdate()
    {

        NavMeshAgent agent =  gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(targetPos.Value);
           return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
