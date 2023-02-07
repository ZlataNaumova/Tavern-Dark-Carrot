using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameState State;

    private static int secondsToNight = 30;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.Day);
    }

    public static void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.GameStart:
                StartScreenHandler();
                break;
            case GameState.Day:
                DayHandler();
                break;
            case GameState.Night:
                NightHandler();
                break;
            default:
                break;
        }
    }

    private static void StartScreenHandler()
    {
      
    }

    private static void DayHandler()
    {
        TavernEventsManager.OnDayStarts();
        Instance.StartCoroutine(NightTimerCoroutine());
    }

    private static void NightHandler()
    {
        TavernEventsManager.OnNightStarts();
        Instance.StartCoroutine(NightEndTimer());
    }

    private static IEnumerator NightTimerCoroutine()
    {
        yield return new WaitForSeconds(secondsToNight);
        UpdateGameState(GameState.Night);
    }

    private static IEnumerator NightEndTimer()
    {
        yield return new WaitForSeconds(secondsToNight);
        UpdateGameState(GameState.Day);
    }

    public enum GameState
    {
        GameStart,
        Day,
        Night
    }

}