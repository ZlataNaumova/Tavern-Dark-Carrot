using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernHeart : PlayerInteractable
{
    [SerializeField] private Material damaged;
    [SerializeField] private Material notDamaged;

    [SerializeField] private int beerKegPriceInSouls;
    [SerializeField] private int beerKegProducingTime;

    [SerializeField] private GameObject kegOfBeer;
    [SerializeField] private Image beerProducingIndicator;
    [SerializeField] private Image beerTypeImage;
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerSprite;


    private ResourcesManager resourcesManager;

    private bool isHeartDamaged = true;
    private bool isBeerProduced = false;
    private bool isBeerProducing = false;

    private int currentBeerType;


    private void OnEnable()
    {
        TavernEventsManager.OnNightStarted += NigthStartsHandler;
        gameObject.GetComponent<Renderer>().material = damaged;
        resourcesManager = FindObjectOfType<ResourcesManager>();
        kegOfBeer.SetActive(false);
        beerProducingIndicator.enabled = false;
        beerTypeImage.enabled = false;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnNightStarted -= NigthStartsHandler;
    }

    public override void PlayerInteraction()
    {
        if (isHeartDamaged)
        {
            HeartRepair();
            return;
        }
        if (player.isHoldingCleaningMaterials || isBeerProducing || player.isHoldingBeerKeg || player.isHoldingGlassOfBeer)
        {
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
        if (!player.isHoldingBeerKeg && !player.isHoldingGlassOfBeer)
        {
            player.TakeBeerKeg(currentBeerType);
            isBeerProduced = false;
            kegOfBeer.SetActive(false);
            beerTypeImage.enabled = false;
        }
    }

    private bool TryProduceBeerKeg()
    {
        if (player.isHoldingBeerIngredient && resourcesManager.TrySpendSouls(beerKegPriceInSouls))
        {
            isBeerProducing = true;

            player.ReleaseBeerIngredient();
            StartCoroutine(KegProducingCoroutine());
            return true;
        }
        else
        {
            Debug.Log("Can not produce beer right now.");
            return false;
        }
    }

    IEnumerator KegProducingCoroutine()
    {
        currentBeerType = player.CurrentBeerType;
        beerTypeImage.sprite = currentBeerType == 1 ? greenBeerSprite : redBeerSprite;
        beerTypeImage.enabled = true;
        beerProducingIndicator.enabled = true;
        int counter = beerKegProducingTime;
        while (counter > 0)
        {
            counter--;
            beerProducingIndicator.fillAmount = 1 - (float)counter / beerKegProducingTime;
            yield return new WaitForSeconds(1);
        }
        kegOfBeer.SetActive(true);
        isBeerProducing = false;
        isBeerProduced = true;
        beerProducingIndicator.fillAmount = 0;
    }

    private void HeartRepair()
    {
        isHeartDamaged = false;
        TavernEventsManager.HeartRepaired();
        gameObject.GetComponent<Renderer>().material = notDamaged;
    }

    private void NigthStartsHandler()
    {
        isHeartDamaged = true;
        gameObject.GetComponent<Renderer>().material = damaged;
    }


}
