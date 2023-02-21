using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{
    [SerializeField] private GameObject enviroment;
    [SerializeField] private GameObject interactableEnviroment;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        TavernEventsManager.SwitchedToNightCanvas += DisableDayEnviroment;
        TavernEventsManager.DayStarted += EnableDayEnviroment;
    }

    private void OnDisable()
    {
        TavernEventsManager.SwitchedToNightCanvas -= DisableDayEnviroment;
        TavernEventsManager.DayStarted -= EnableDayEnviroment;
    }

    private void DisableDayEnviroment()
    {
        enviroment.SetActive(false);
        interactableEnviroment.SetActive(false);
        player.SetActive(false);
    }

    private void EnableDayEnviroment()
    {
        enviroment.SetActive(true);
        interactableEnviroment.SetActive(true);
        player.SetActive(true);
    }
}
