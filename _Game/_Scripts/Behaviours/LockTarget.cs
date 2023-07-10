using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class LockTarget : Conditional
{
    public bool lockTarget;
    PlayerMovement movement;

    public override void OnAwake()
    {
        movement=GetComponent<PlayerMovement>();


    }
    public override TaskStatus OnUpdate()
    {
        if(!movement)
            return TaskStatus.Failure;
        movement.LockTarget(lockTarget);
        return TaskStatus.Success;

    }

    public bool CheckWithinSight(Transform targetTransform, float fieldOfViewAngle)
    {
        Vector3 direction = targetTransform.position - transform.position;
        Debug.Log(Vector3.Angle(direction, transform.forward));
        return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
    }
}
