using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject startNightButton;

    private void OnEnable()
    {
        TavernEventsManager.TavernReadyForNight += TavernReadyForNightHandler;
    }
    private void OnDisable()
    {
        TavernEventsManager.TavernReadyForNight -= TavernReadyForNightHandler;
    }
    private void TavernReadyForNightHandler()
    {
        startNightButton.SetActive(true);
    }
}
