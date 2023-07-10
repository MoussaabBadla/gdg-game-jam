using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class CompareAIState : Conditional
{

    public bool equal;
    public SharedAIState StateToCompare;
    public SharedAIState StateToCompareTo;
    public override TaskStatus OnUpdate()
    {
        if(equal && StateToCompare.Value == StateToCompareTo.Value)
        {
            return TaskStatus.Success;
        }

        if (!equal && StateToCompare.Value != StateToCompareTo.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
