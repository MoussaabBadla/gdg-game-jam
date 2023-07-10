using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedAIState : SharedVariable<AIState>
{
    public static implicit operator SharedAIState(AIState value) { return new SharedAIState { Value = value }; }
}

public enum AIState
{
   Idle,Alert,Stalking
}


