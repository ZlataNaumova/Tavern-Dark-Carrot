using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeerFilter : PlayerInteractable
{
    [SerializeField] private TMP_Text filterStatusText;
    [SerializeField] private int fillingGlassTime;
    [SerializeField] private GameObject kegOfBeer;

    private bool isFillingGlass = false;
    private int beerGlasses;
    private Coroutine fillingGlass;

    private void Start()
    {
        kegOfBeer.SetActive(false);
        filterStatusText.text = "waiting for player";
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
                filterStatusText.text = "Filling Glass fail";
                StartCoroutine(TextUpdateCoroutine());
                Debug.Log("FillingGlass fail");
            }
            outline.OutlineWidth = 0;
        }
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingBeerKeg)
        {
            SetBeerKeg();
        }
        else if (beerGlasses > 0 && !player.isHoldingGlassOfBeer)
        {
            TryGiveGlassToPlayer();
        }
    }

    private void SetBeerKeg()
    {
        player.ReleaseBeerKeg();
        kegOfBeer.SetActive(true);
        beerGlasses += 4;
    }

    private void TryGiveGlassToPlayer()
    {
        if (!isFillingGlass)
        {
            if(--beerGlasses <= 0)
            {
                kegOfBeer.SetActive(false);
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
        filterStatusText.text = "Pouring glass with beer";
        yield return new WaitForSeconds(fillingGlassTime);
        filterStatusText.text = "Beer pouring success";
        StartCoroutine(TextUpdateCoroutine());
        isFillingGlass = false;
        player.TakeGlassOfBeer();
    }

    private IEnumerator TextUpdateCoroutine()
    {
        yield return new WaitForSeconds(1);
        filterStatusText.text = "waiting for player";
    }
}
