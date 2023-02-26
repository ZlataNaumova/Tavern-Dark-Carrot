using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gramophone : PlayerInteractable
{
    [SerializeField] private TMP_Text gramophoneVolumeText;
    private int maxVolume = 100;
    private int currentVolume = 100;
    private bool isVolumeLow;
    private int volumeThreshold = 50;
    private Coroutine volumeDecreaseCoroutine;

    private void Start()
    {
        volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
        PlayerInteraction();
        
    }

    public override void PlayerInteraction()
    {
        if (currentVolume <= 0)
        {
            volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
        }
        currentVolume = maxVolume;
        gramophoneVolumeText.text = currentVolume.ToString();
    }

    IEnumerator VolumeDecreaseCoroutine()
    {
        while(currentVolume > 0)
        {
            yield return new WaitForSeconds(1);
            currentVolume--;
            gramophoneVolumeText.text = currentVolume.ToString();
            HappinessHandler();
        }
    }

    private void HappinessHandler()
    {
        bool wasVolumeLow = isVolumeLow;
        isVolumeLow = (currentVolume <= volumeThreshold);

        if (wasVolumeLow != isVolumeLow)
        {
            int happinessRateChange = isVolumeLow ? -3 : 3;
            TavernEventsManager.HappinessRateChanged(happinessRateChange);
        }
    }
}
