using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject startNightButton;
    [SerializeField] private TMP_Text coinsValueText;
    [SerializeField] private TMP_Text soulsValueText;

    private void OnEnable()
    {
        TavernEventsManager.TavernReadyForNight += TavernReadyForNightHandler;
        TavernEventsManager.CoinsValueChanged += CoinsValueChangedHandler;
        TavernEventsManager.SoulsValueChanged += SoulsValueChangedHandler;
    }
    private void OnDisable()
    {
        TavernEventsManager.TavernReadyForNight -= TavernReadyForNightHandler;
        TavernEventsManager.CoinsValueChanged -= CoinsValueChangedHandler;
        TavernEventsManager.SoulsValueChanged -= SoulsValueChangedHandler;
    }

    private void TavernReadyForNightHandler()
    {
        startNightButton.SetActive(true);
    }
    private void CoinsValueChangedHandler(int newValue)
    {
        coinsValueText.text = "Coins: " + newValue.ToString();
    }
    private void SoulsValueChangedHandler(int newValue)
    {
        soulsValueText.text = "Souls: " + newValue.ToString();
    }
}
