using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernHeart : PlayerInterractible
{
   
    [SerializeField] private Material damaged;
    [SerializeField] private Material notDamaged;

    private bool isHeartDamaged = true;
    private bool isBeerProduced = false;

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material = damaged;
    }

    public override void PlayerInterraction()
    {
        if (isHeartDamaged)
        {
            HeartRepair();
            return;
        }
        else if(isBeerProduced)
        {
            GivePlayerBeerKeg();
            return;
        }
        else
        {
            ProduceBeerKeg();
        }
    }

   private void GivePlayerBeerKeg()
    {
        player.GetBeerKeg();
    }

    private void ProduceBeerKeg()
    {
        isBeerProduced = true;
    }

    private void HeartRepair()
    {
        isHeartDamaged = false;
        TavernEventsManager.OnHeartRepaired();
        gameObject.GetComponent<Renderer>().material = notDamaged;
    }

    
}
