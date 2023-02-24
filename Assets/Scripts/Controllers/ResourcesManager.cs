using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    private int coins = 0;
    private int souls = 10;
    private List<VisitorAI> defendersCards = new List<VisitorAI>();

    private void OnEnable()
    {
        TavernEventsManager.OnCoinsAdded += AddCoins;
        TavernEventsManager.OnSoulsAdded += AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard += OnVisitorBecomeDefenderCardHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnCoinsAdded -= AddCoins;
        TavernEventsManager.OnSoulsAdded -= AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard -= OnVisitorBecomeDefenderCardHandler;
    }

    private void Start()
    {
        TavernEventsManager.CoinsValueChanged(coins);
        TavernEventsManager.SoulsValueChanged(souls);
        
    }

    private void AddCoins(int value)
    {
        coins += value;
        TavernEventsManager.CoinsValueChanged(coins);
    }
    public bool TrySpendCoins(int value)
    {
        if(value <= coins)
        {
            coins -= value;
            TavernEventsManager.CoinsValueChanged(coins);
            return true;
        }
        return false;
    }
    private void AddSouls(int value)
    {
        souls += value;
        TavernEventsManager.SoulsValueChanged(souls);
    }
    public bool TrySpendSouls(int value)
    {
        if(value <= souls)
        {
            souls -= value;
            TavernEventsManager.SoulsValueChanged(souls);
            return true;
        }
        return false;
    }

    private void OnVisitorBecomeDefenderCardHandler(VisitorAI visitor)
    {
        defendersCards.Add(visitor);
    }
}
