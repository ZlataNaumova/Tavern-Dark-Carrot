using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gramophone : PlayerInteractable
{
    [SerializeField] private TMP_Text gramophoneVolumeText;
    private int maxVolume = 100;
    private int currentVolume;

    private void Start()
    {
        PlayerInteraction();
        StartCoroutine(VolumeDecreaseCoroutine());
    }

    public override void PlayerInteraction()
    {
        currentVolume = maxVolume;
        gramophoneVolumeText.text = currentVolume.ToString();
    }

    IEnumerator VolumeDecreaseCoroutine()
    {
        while(currentVolume >= 0)
        {
            yield return new WaitForSeconds(1);
            currentVolume--;
            gramophoneVolumeText.text = currentVolume.ToString();

        }


    }
}
