using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeerFilter : PlayerInteractable
{
    [SerializeField] private int fillingGlassTime;
    [SerializeField] private GameObject kegOfBeer;
    [SerializeField] private Image beerProducingIndicator;
    [SerializeField] private Image beerTypeImage;
    [SerializeField] private Sprite redBeerGlassSprite;
    [SerializeField] private Sprite greenBeerGlassSprite;

    private bool isFillingGlass = false;
    private int beerGlasses;
    private Coroutine fillingGlass;
    private int currentBeerType;

    private void Start()
    {
        kegOfBeer.SetActive(false);
        beerTypeImage.enabled = false;
        beerProducingIndicator.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetTarget(player.gameObject);
            if(fillingGlass != null && isFillingGlass)
            {
                StopCoroutine(fillingGlass);
                isFillingGlass = false;
                beerProducingIndicator.enabled = false;
                Debug.Log("FillingGlass fail");
            }
            outline.OutlineWidth = 0;
        }
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingBeerKeg)
        {
            SetBeerKeg(player.CurrentBeerType);
        }
        else if (beerGlasses > 0 && !player.isHoldingGlassOfBeer)
        {
            TryGiveGlassToPlayer();
        }
    }

    private void SetBeerKeg(int beerType)
    {
        player.ReleaseBeerKeg();
        kegOfBeer.SetActive(true);
        beerGlasses += 4;
        currentBeerType = beerType;
        UpdadeBeerTypeSprite();
        beerTypeImage.enabled = true;
    }

    private void TryGiveGlassToPlayer()
    {
        if (!isFillingGlass)
        {
            if(--beerGlasses <= 0)
            {
                kegOfBeer.SetActive(false);
                beerTypeImage.enabled = false;
            }
            fillingGlass = StartCoroutine(FillingGlassCroutine());
        } else
        {
            Debug.Log("You can not start filling glass now");
        }
    }

    private IEnumerator FillingGlassCroutine()
    {
        isFillingGlass = true;
        beerProducingIndicator.enabled = true;
        float counter = (float)fillingGlassTime;
        while(counter > 0)
        {
            counter -= 0.5f;
            beerProducingIndicator.fillAmount = (float)counter / fillingGlassTime;
            yield return new WaitForSeconds(.5f);
        }
        isFillingGlass = false;
        beerProducingIndicator.enabled = false;
        player.TakeGlassOfBeer(currentBeerType);
    }
    
    private void UpdadeBeerTypeSprite() => beerTypeImage.sprite = currentBeerType == 1 ? greenBeerGlassSprite : redBeerGlassSprite;
}
