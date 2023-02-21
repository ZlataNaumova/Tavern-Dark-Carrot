using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameState State;

    private static int secondsToNight;

   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        secondsToNight = GameConfigManager.SecondsToNightStarts;
        UpdateGameState(GameState.Day);
    }

    private void OnEnable()
    {
        TavernEventsManager.HeartRepaired += HeartRepairedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.HeartRepaired -= HeartRepairedHandler;
    }

    private void HeartRepairedHandler()
    {
        Instance.StartCoroutine(NightTimerCoroutine());

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
        TavernEventsManager.OnDayStarted();
        TavernEventsManager.OnCameraSwitchedToFollowPlayer();
        TavernEventsManager.OnSwitchedToDayCanvas();
    }

    private static void NightHandler()
    {
        TavernEventsManager.OnNightStarted();
        Instance.StartCoroutine(CardGameTiming(6));
    }

    private static IEnumerator NightTimerCoroutine()
    {
        yield return new WaitForSeconds(secondsToNight);
        UpdateGameState(GameState.Night);
    }

    private static IEnumerator CardGameTiming(int seconds)
    {
        int counter = seconds;
        TavernEventsManager.OnCameraSwitchedToCardGame();
        while (counter > 0)
        {
            counter--;
            if(counter == 3)
            {
                TavernEventsManager.OnSwitchedToNightCanvas();
            }
            if(counter == 2)
            {
                TavernEventsManager.OnCardsRendered();
            }
            yield return new WaitForSeconds(1);
        }
    }
    
    public enum GameState
    {
        GameStart,
        Day,
        Night
    }
}