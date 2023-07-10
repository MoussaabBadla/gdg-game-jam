using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
public class MoveToTargetObject : Conditional
{
    public SharedGameObject target;
    public override TaskStatus OnUpdate()
    {

        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(target.Value.transform.position);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
