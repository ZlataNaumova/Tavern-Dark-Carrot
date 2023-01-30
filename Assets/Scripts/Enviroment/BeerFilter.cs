using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerFilter : PlayerInterractible
{
    private int beerGlasses;

    public override void PlayerInterraction()
    {
        if (player.isHoldingBeerKeg)
        {
            SetBeerKeg();
        }
        else if (beerGlasses > 0)
        {
            beerGlasses -= 1;
            player.GetGlassOfBeer();
        }
        else
        {
            Debug.Log("No Beer in current keg! Bring the new one!");
        }
    }

    private void SetBeerKeg()
    {
        player.ReleaseBeerKeg();
        beerGlasses += 4;
    }
}
