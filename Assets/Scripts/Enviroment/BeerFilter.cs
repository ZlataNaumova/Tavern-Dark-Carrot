using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerFilter : PlayerInteractable
{

    [SerializeField] private int fillingGlassTime;
    [SerializeField] private GameObject kegOfBeer;

    private bool isFillingGlass = false;
    private int beerGlasses;
    private Coroutine fillingGlass;

    private void Start()
    {
        kegOfBeer.SetActive(false);
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
                Debug.Log("FillingGlass fail");
            }
           
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
            fillingGlass = StartCoroutine(FillingGlass());
        } else
        {
            Debug.Log("You can not start filling glass now");
        }
        
    }
    private IEnumerator FillingGlass()
    {
        isFillingGlass = true;
        yield return new WaitForSeconds(fillingGlassTime);
        isFillingGlass = false;
        player.GetGlassOfBeer();
    }
}
