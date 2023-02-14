using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernHeart : PlayerInterractible
{
    [SerializeField] private Material damaged;
    [SerializeField] private Material notDamaged;

    [SerializeField] private int beerKegPriceInSouls;
    [SerializeField] private int beerKegProducingTime;

    [SerializeField] private GameObject kegOfBeer;

    private ResourcesManager resourcesManager;

    private bool isHeartDamaged = true;
    private bool isBeerProduced = false;
    private bool isBeerProducing = false;

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material = damaged;
        resourcesManager = FindObjectOfType<ResourcesManager>();
        kegOfBeer.SetActive(false);
    }

    private void OnEnable()
    {
        TavernEventsManager.NightStarts += NigthStartsHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.NightStarts -= NigthStartsHandler;
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
            TryProduceBeerKeg();
        }
    }

   private void GivePlayerBeerKeg()
    {
        if(!player.isHoldingBeerKeg && !player.isHoldingGlassOfBeer)
        {
            player.GetBeerKeg();
            isBeerProduced = false;
            kegOfBeer.SetActive(false);

        }
    }

    private bool TryProduceBeerKeg()
    {
        if (!isBeerProducing && !player.isHoldingBeerKeg && resourcesManager.TrySpendSouls(beerKegPriceInSouls))
        {
            isBeerProducing = true;
            StartCoroutine(KegProducingCoroutine());
            return true;
        } else
        {
            Debug.Log("Not enought souls to produce beer");
            return false;
        }
    }

    IEnumerator KegProducingCoroutine()
    {
        yield return new WaitForSeconds(beerKegProducingTime);
        kegOfBeer.SetActive(true);
        isBeerProducing = false;
        isBeerProduced = true;
    }

    private void HeartRepair()
    {
        isHeartDamaged = false;
        TavernEventsManager.OnHeartRepaired();
        gameObject.GetComponent<Renderer>().material = notDamaged;
    }

    private void NigthStartsHandler()
    {
        isHeartDamaged = true;
        gameObject.GetComponent<Renderer>().material = damaged;
    }


}
