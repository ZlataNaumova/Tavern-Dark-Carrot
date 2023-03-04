using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brewery : PlayerInteractable
{
    [SerializeField] private GameObject kegOfBeer;
    [SerializeField] private Image beerProducingIndicator;
    [SerializeField] private int currentBeerType;
    [SerializeField] private Image beerTypeImage;
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerType;

    private bool isBeerProduced;
    private bool isBeerProducing;
    private int beerKegPriceInSouls;
    private int beerKegProducingTime = 3;
    private ResourcesManager resourcesManager;

    private void Start()
    {
        beerTypeImage.sprite = currentBeerType == 1 ? greenBeerType : redBeerSprite;
        resourcesManager = FindObjectOfType<ResourcesManager>();
        beerProducingIndicator.fillAmount = 0;
        kegOfBeer.SetActive(false);
    }

    public override void PlayerInteraction()
    {
        if (isBeerProduced)
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

        }
    }

    private bool TryProduceBeerKeg()
    {
        if (!isBeerProducing && !player.isHoldingBeerKeg && resourcesManager.TrySpendSouls(beerKegPriceInSouls))
        {
            isBeerProducing = true;
            StartCoroutine(KegProducingCoroutine());
            return true;
        }
        else
        {
            Debug.Log("Not enought souls to produce beer");
            return false;
        }
    }

    IEnumerator KegProducingCoroutine()
    {
        int counter = beerKegProducingTime;
        while(counter > 0)
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

    
}
