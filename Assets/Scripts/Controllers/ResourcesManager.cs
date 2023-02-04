using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    private int coins = 0;
    private int souls = 10;

    private void OnEnable()
    {
        TavernEventsManager.AddCoins += AddCoins;
        TavernEventsManager.AddSouls += AddSouls;
    }

    private void OnDisable()
    {
        TavernEventsManager.AddCoins -= AddCoins;
        TavernEventsManager.AddSouls -= AddSouls;
    }

    private void Start()
    {
        TavernEventsManager.OnCoinsValueChanged(coins);
        TavernEventsManager.OnSoulsValueChanged(souls);
        
    }

    public void AddCoins(int value)
    {
        coins += value;
        TavernEventsManager.OnCoinsValueChanged(coins);
    }
    public bool TrySpendCoins(int value)
    {
        if(value <= coins)
        {
            coins -= value;
            TavernEventsManager.OnCoinsValueChanged(coins);
            return true;
        }
        return false;
    }
    public void AddSouls(int value)
    {
        souls += value;
        TavernEventsManager.OnSoulsValueChanged(souls);
    }
    public bool TrySpendSouls(int value)
    {
        if(value <= souls)
        {
            souls -= value;
            TavernEventsManager.OnSoulsValueChanged(souls);
            return true;
        }
        return false;
    }
}
