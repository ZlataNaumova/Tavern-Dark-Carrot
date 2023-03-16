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
    private int currentVolume;
    private bool isVolumeLow;
    private int volumeThreshold = 50;
    private Coroutine volumeDecreaseCoroutine;

    private void Start()
    {
        currentVolume = GameConfigManager.StartVolumeLevel;
        gramophoneVolumeText.text = currentVolume.ToString();
        volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
        warningSignImage.enabled = false;
        
    }

    public override void PlayerInteraction()
    {
        currentVolume = maxVolume;
        gramophoneVolumeText.text = currentVolume.ToString();
        HappinessHandler();
    }

        IEnumerator VolumeDecreaseCoroutine()
    {
        yield return new WaitForSeconds(GameConfigManager.DecreaseStartDelay);

        while(currentVolume > 0)
        {
            yield return new WaitForSeconds(1);
            currentVolume = currentVolume - GameConfigManager.DecreaseRate;
            if (currentVolume < 0)
            {
                currentVolume = 0;
            }
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
