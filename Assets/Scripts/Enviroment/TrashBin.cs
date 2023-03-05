using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : PlayerInteractable
{
    public override void PlayerInteraction()
    {
        player.ReleaseAnyItem();
    }

   
}
