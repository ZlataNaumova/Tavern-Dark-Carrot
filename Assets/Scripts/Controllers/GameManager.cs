using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public static GameState State = GameState.GameStart;


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

   

    public static void UpdateGameState(GameState newState)
    {
       
        State = newState;
        switch (newState)
        {
            case GameState.GameStart:
              
                break;

            case GameState.Day:
               
                break;
            case GameState.Night:
                
                break;
            case GameState.GameOver:

                break;
            default:
                break;
        }

    }

   
}
public enum GameState
{
    GameStart,
    Day,
    Night,
    GameOver
}
