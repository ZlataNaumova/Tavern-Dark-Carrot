using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefenderState 
{

    IDefenderState DoState(Defender defender);
}
