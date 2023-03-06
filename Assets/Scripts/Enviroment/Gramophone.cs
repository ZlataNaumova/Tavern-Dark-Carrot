using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gramophone : PlayerInteractable
{
    [SerializeField] private TMP_Text gramophoneVolumeText;
    [SerializeField] private Image warningSignImage;

    private int maxVolume = 100;
    private int currentVolume = 100;
    private bool isVolumeLow;
    private int volumeThreshold = 50;
    private Coroutine volumeDecreaseCoroutine;

    private void Start()
    {
        volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
        PlayerInteraction();
        warningSignImage.enabled = false;
        
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
            int happinessRateChange = isVolumeLow ?
                -GameConfigManager.LowGramophoneVolumeHappinessEffect : GameConfigManager.LowGramophoneVolumeHappinessEffect;
            TavernEventsManager.HappinessRateChanged(happinessRateChange);
            warningSignImage.enabled = isVolumeLow;
        }
    }
}
