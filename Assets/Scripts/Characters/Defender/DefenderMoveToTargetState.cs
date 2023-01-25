using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderMoveToTargetState : IDefenderState
{
    public IDefenderState DoState(Defender defender)
    {
        defender.MoveToTarget();
        return defender.DefenderStateTransitionHandler();
    }

   
}
