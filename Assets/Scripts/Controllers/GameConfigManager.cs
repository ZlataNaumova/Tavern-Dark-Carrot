using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
    public static GameConfigManager Instance;

    public GameConfig[] presets;
    public  int selectedPresetIndex = 0;

    private static int playerSpeed;
    private static int visitorSpeed;
    private static int visitorSecondsToLeave;
    private static int secondsToNightStarts;

    public static int PlayerSpeed { get { return playerSpeed; } }
    public static int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public static int VisitorSpeed { get { return visitorSpeed; } }
    public static int SecondsToNightStarts { get { return secondsToNightStarts; } }


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
        CurrentGameConfigHandler();
    }

    private void CurrentGameConfigHandler()
    {
        if (selectedPresetIndex >= 0 && selectedPresetIndex < presets.Length)
        {
                playerSpeed = presets[selectedPresetIndex].PlayerSpeed;
                visitorSpeed = presets[selectedPresetIndex].VisitorSpeed;
                visitorSecondsToLeave= presets[selectedPresetIndex].VisitorSecondsToLeave;
                secondsToNightStarts = presets[selectedPresetIndex].SecondsToNightStarts;
}
        else
        {
            Debug.LogError("Invalid preset index!");
        }
    }
}

    

