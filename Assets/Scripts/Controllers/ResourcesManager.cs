using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private TMP_Text happinessText;
    [SerializeField] private TMP_Text happinessRateText;

    private int coins = 0;
    private int souls = 10;
    private int happiness = 0;
    private int happinessRate = 0;
    private List<VisitorAI> defendersCards = new List<VisitorAI>();
    private Coroutine updateHappinessCoroutine;

    private void OnEnable()
    {
        TavernEventsManager.OnCoinsAdded += AddCoins;
        TavernEventsManager.OnSoulsAdded += AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard += VisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnHappinessRateChanged += HappinessRateChangedHandler;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
        TavernEventsManager.OnHeartRepaired += DayStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnCoinsAdded -= AddCoins;
        TavernEventsManager.OnSoulsAdded -= AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard -= VisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnHappinessRateChanged -= HappinessRateChangedHandler;
        TavernEventsManager.OnNightStarted -= NightStartedHandler;
        TavernEventsManager.OnHeartRepaired -= DayStartedHandler;
    }

    private void Start()
    {
        TavernEventsManager.CoinsValueChanged(coins);
        TavernEventsManager.SoulsValueChanged(souls);
        //updateHappinessCoroutine = StartCoroutine(UpdateHappiness());
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

    private void VisitorBecomeDefenderCardHandler(VisitorAI visitor)
    {
        defendersCards.Add(visitor);
        TavernEventsManager.CardsQuantityChanged(defendersCards.Count);
    }
    private void NightStartedHandler()
    {
        StopCoroutine(updateHappinessCoroutine);
        happiness = happinessRate = 0;
        if(defendersCards.Count > 0)
        {
            TavernEventsManager.DefendersToCards(defendersCards);
        }
    }

    private void DayStartedHandler()
    {
        updateHappinessCoroutine = StartCoroutine(UpdateHappiness());
    }

    private IEnumerator UpdateHappiness()
    {
        while (true)
        {
            if (happinessRate == 0)
            {
                happiness += GameConfigManager.PositiveHappinessEffect;
            }
            if (happiness > GameConfigManager.HappinessMaxLevel)
            {
                happiness = GameConfigManager.HappinessMaxLevel;
            }
            if (happiness < -GameConfigManager.HappinessMaxLevel)
            {
                happiness = -GameConfigManager.HappinessMaxLevel;
            }
            happiness += happinessRate;
            TavernEventsManager.HappinessChanged(happiness);
            UpdateText();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateText()
    {
        happinessText.text = happiness.ToString();
        happinessRateText.text = happinessRate.ToString();
    }

    private void HappinessRateChangedHandler(int value)
    {
        happinessRate += value;
    }
}
