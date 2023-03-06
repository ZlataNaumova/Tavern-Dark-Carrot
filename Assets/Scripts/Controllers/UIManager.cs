using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dayCanvas;
    [SerializeField] private GameObject nightCanvas;
    [SerializeField] private GameObject nightAutoFightCanvas;
    [SerializeField] private TMP_Text coinsValueText;
    [SerializeField] private TMP_Text soulsValueText;
    [SerializeField] private TMP_Text cardsValueText;
    [SerializeField] private Image nigthTimerIndicator;
    [SerializeField] private Image happinesIconImage;
    [SerializeField] private Sprite happinessGoodSprite;
    [SerializeField] private Sprite happinessNormalSprite;
    [SerializeField] private Sprite happinessBadSprite;
    [SerializeField] private Slider happinessSlider;

    private void Start() => happinesIconImage.sprite = happinessNormalSprite;
    
    private void OnEnable()
    {
        TavernEventsManager.OnSwitchedToDayCanvas += SwitchToDayCanvas;
        TavernEventsManager.OnSwitchedToNightCanvas += SwitchToNightCanvas;
        TavernEventsManager.OnCoinsValueChanged += CoinsValueChangedHandler;
        TavernEventsManager.OnSoulsValueChanged += SoulsValueChangedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessChangeHandler;
        TavernEventsManager.OnCardsQuantityChanged += CardsQuantityChangedHandler;
        TavernEventsManager.OnHeartRepaired += NightTimerHandler;
        TavernEventsManager.OnSwitchedToNigthAutoFightCanvas += OnSwitchedToNigthAutoFightCanvasHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnSwitchedToDayCanvas -= SwitchToDayCanvas;
        TavernEventsManager.OnSwitchedToNightCanvas -= SwitchToNightCanvas;
        TavernEventsManager.OnCoinsValueChanged -= CoinsValueChangedHandler;
        TavernEventsManager.OnSoulsValueChanged -= SoulsValueChangedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessChangeHandler;
        TavernEventsManager.OnCardsQuantityChanged -= CardsQuantityChangedHandler;
        TavernEventsManager.OnHeartRepaired -= NightTimerHandler;
        TavernEventsManager.OnSwitchedToNigthAutoFightCanvas -= OnSwitchedToNigthAutoFightCanvasHandler;
    }

    private void CoinsValueChangedHandler(int newValue) => coinsValueText.text = newValue.ToString();
    
    private void SoulsValueChangedHandler(int newValue) => soulsValueText.text = newValue.ToString();

    private void CardsQuantityChangedHandler(int newCardsQuantity) => cardsValueText.text = newCardsQuantity.ToString();
    
    private void HappinessChangeHandler(int currentHappinessValue)
    {
        happinessSlider.value = currentHappinessValue;
        switch (currentHappinessValue)
        {
            case int happiness when currentHappinessValue < -5:
                happinesIconImage.sprite = happinessBadSprite;
                break;
            case int happiness when currentHappinessValue > -5 && currentHappinessValue < 5:
                happinesIconImage.sprite = happinessNormalSprite;
                break;
            case int happiness when currentHappinessValue > 6:
                happinesIconImage.sprite = happinessGoodSprite;
                break;
            default:
                break;
        }
    }

    private void OnSwitchedToNigthAutoFightCanvasHandler()
    {
        dayCanvas.SetActive(false);
        nightCanvas.SetActive(false);
        nightAutoFightCanvas.SetActive(true);
    }

    private void SwitchToDayCanvas()
    {
        dayCanvas.SetActive(true);
        nightCanvas.SetActive(false);
        nightAutoFightCanvas.SetActive(false);
    }

    private void SwitchToNightCanvas()
    {
        dayCanvas.SetActive(false);
        nightCanvas.SetActive(true);
        nightAutoFightCanvas.SetActive(false);
    }

    private void NightTimerHandler() => StartCoroutine(NightTimerCoroutine());
    
    private IEnumerator NightTimerCoroutine()
    {
        int counter = GameConfigManager.SecondsToNightStarts;
        while(counter > 0)
        {
            counter--;
            nigthTimerIndicator.fillAmount = (float)counter / GameConfigManager.SecondsToNightStarts;
            yield return new WaitForSeconds(1);
        }
    }
   
}
