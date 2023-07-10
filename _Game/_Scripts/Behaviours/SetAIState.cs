using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class SetAIState : Conditional
{
    public AIState valueState;
    public SharedAIState sharedVariableState;
    public override TaskStatus OnUpdate()
    {
        if (sharedVariableState != null)
        {
            sharedVariableState.SetValue(valueState);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
