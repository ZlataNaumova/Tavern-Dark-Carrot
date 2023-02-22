using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagOfCarrots : PlayerInteractable
{
    public override void PlayerInteraction()
    {
        if(!player.isHoldingCarrots && !player.isHoldingBeerKeg && !player.isHoldingGlassOfBeer)
        {
            player.TakeCarrots();
        }
    }
}
