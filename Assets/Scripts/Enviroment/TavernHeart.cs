using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernHeart : PlayerInteractable
{

    [SerializeField] private GameObject kegOfBeer;
    [SerializeField] private Image beerProducingIndicator;
    [SerializeField] private Image beerTypeImage;
    [SerializeField] private Image warningSignImage;
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerSprite;
    [SerializeField] private Transform bubblesTransform;

    private ResourcesManager resourcesManager;

    private bool isHeartDamaged = true;
    private bool isBeerProduced = false;
    private bool isBeerProducing = false;

    private int currentBeerType;


    private void OnEnable()
    {
        TavernEventsManager.OnNightStarted += NigthStartsHandler;
        resourcesManager = FindObjectOfType<ResourcesManager>();
        kegOfBeer.SetActive(false);
        beerProducingIndicator.enabled = false;
        beerTypeImage.enabled = false;
        warningSignImage.enabled = true;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnNightStarted -= NigthStartsHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if (player.isHoldingBeerIngredient || isBeerProduced)
            {
                player.SetTarget(gameObject);
                outline.OutlineWidth = 2.5f;
            }
           
        }
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingCleaningMaterials || isBeerProducing || player.isHoldingBeerKeg || player.isHoldingGlassOfBeer)
        {
            return;
        }
        else if(isBeerProduced && !player.isHoldingCleaningMaterials && !player.isHoldingBeerKeg && !player.isHoldingGlassOfBeer && !player.isHoldingBeerIngredient)
        {
            GivePlayerBeerKeg();
            return;
        }
        else if(!isBeerProduced && player.isHoldingBeerIngredient)
        {
            if (isHeartDamaged)
            {
                HeartRepair();
            }
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
        if (player.isHoldingBeerIngredient && resourcesManager.TrySpendSouls(GameConfigManager.BeerKegPriceInSouls))
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
        float time = GameConfigManager.BeerKegProducingTime;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            beerProducingIndicator.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            bubblesTransform.Rotate(new Vector3(0f, 2f, 0f));
            yield return null;
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
        warningSignImage.enabled = false;
    }

    private void NigthStartsHandler()
    {
        isHeartDamaged = true;
        warningSignImage.enabled = false;

    }


}
