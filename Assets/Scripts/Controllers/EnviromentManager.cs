using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{
    [SerializeField] private GameObject enviroment;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        TavernEventsManager.SwitchToNightCanvas += DisableDayEnviroment;
        TavernEventsManager.DayStarts += EnableDayEnviroment;
    }

    private void OnDisable()
    {
        TavernEventsManager.SwitchToNightCanvas -= DisableDayEnviroment;
        TavernEventsManager.DayStarts -= EnableDayEnviroment;
    }

    private void DisableDayEnviroment()
    {
        enviroment.SetActive(false);
        player.SetActive(false);
    }

    private void EnableDayEnviroment()
    {
        enviroment.SetActive(true);
        player.SetActive(true);
    }
}
