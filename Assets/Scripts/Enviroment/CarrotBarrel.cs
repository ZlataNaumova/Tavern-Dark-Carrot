using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarrotBarrel : PlayerInteractable
{
    [SerializeField] private TMP_Text carrotsQuantityText;
    [SerializeField] private TMP_Text barrelStatusText;


    private int currentCarrotsQuantity;

    private void OnEnable()
    {
        TavernEventsManager.OnVisitorTriedTakeCarrot += VisitorTriedTakeCarrotHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnVisitorTriedTakeCarrot -= VisitorTriedTakeCarrotHandler;
    }

    private void Start()
    {
        UpdateCarrotsText();
    }

    private void VisitorTriedTakeCarrotHandler(VisitorAI visitor)
    {
        if (TryTakeCarrot())
        {
            TavernEventsManager.OneCarrotSold(visitor);
        }
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingCarrots)
        {
            AddCarrots(GameConfigManager.CarrotsPlayerHolding);
            player.FillCarrotBarrel();
        }
    }

    public void AddCarrots(int carrotsValue)
    {
        currentCarrotsQuantity += carrotsValue;
        if (GameConfigManager.MaxCarrotsInBarrel < currentCarrotsQuantity)
        {
            currentCarrotsQuantity = GameConfigManager.MaxCarrotsInBarrel;
        }
        UpdateCarrotsText();
    }

    private bool TryTakeCarrot()
    {
        if(currentCarrotsQuantity > 0)
        {
            currentCarrotsQuantity--;
            UpdateCarrotsText();
            return true;
        }
        return false;
    }

    private void UpdateCarrotsText()
    {
        carrotsQuantityText.text = currentCarrotsQuantity.ToString();
        if(currentCarrotsQuantity == GameConfigManager.MaxCarrotsInBarrel)
        {
            barrelStatusText.text = "Full";
        }
        else if(currentCarrotsQuantity == 0)
        {
            barrelStatusText.text = "Empty";
        }
        else
        {
            barrelStatusText.text = "Have some";
        }
    }
    
}
