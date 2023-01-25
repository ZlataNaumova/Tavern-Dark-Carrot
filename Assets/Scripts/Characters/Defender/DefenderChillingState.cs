using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderChillingState : IDefenderState
{
    public IDefenderState DoState(Defender defender)
    {
        defender.Chill();
        return defender.DefenderStateTransitionHandler();
    }

}
