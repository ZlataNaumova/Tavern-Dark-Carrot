using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dayCanvas;
    [SerializeField] private GameObject nightCanvas;
    [SerializeField] private TMP_Text coinsValueText;
    [SerializeField] private TMP_Text soulsValueText;

    [SerializeField] private int secondsCameraToFly;

    private void OnEnable()
    {
        TavernEventsManager.SwitchToDayCanvas += SwitchToDayCanvas;
        TavernEventsManager.SwitchToNightCanvas += SwitchToNightCanvas;
        TavernEventsManager.CoinsValueChanged += CoinsValueChangedHandler;
        TavernEventsManager.SoulsValueChanged += SoulsValueChangedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.SwitchToDayCanvas -= SwitchToDayCanvas;
        TavernEventsManager.SwitchToNightCanvas -= SwitchToNightCanvas;
        TavernEventsManager.CoinsValueChanged -= CoinsValueChangedHandler;
        TavernEventsManager.SoulsValueChanged -= SoulsValueChangedHandler;
    }

    private void CoinsValueChangedHandler(int newValue)
    {
        coinsValueText.text = "Coins: " + newValue.ToString();
    }

    private void SoulsValueChangedHandler(int newValue)
    {
        soulsValueText.text = "Souls: " + newValue.ToString();
    }

    private void SwitchToDayCanvas()
    {
        dayCanvas.SetActive(true);
        nightCanvas.SetActive(false);
    }

    private void SwitchToNightCanvas()
    {
        dayCanvas.SetActive(false);
        nightCanvas.SetActive(true);
    }

}
