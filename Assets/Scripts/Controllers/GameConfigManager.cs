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
    private static int beerSoldRewardInCoins;
    private static int beerSoldRewardInSouls;
    private static int drinksToBecomeDefenderCard;
    private static int visitorsSpawnDelayMin;
    private static int visitorsSpawnDelayMax;
    private static int maxVisitersQuantity;
    private static int secondsToGetHungry;
    private static int carrotsPlayerHolding;
    private static int maxCarrotsInBarrel;
    private static int onBeerDrinkStrengthReward;
    private static int onCarrotEatStrengthReward;
    private static int carrotSoldRewardInSouls;
    private static int carrotSoldRewardInCoins;
    private static int dirtyTableHappinessEffect;
    private static int emptyCarrotBarrelHappinessEffect;
    private static int lowGramophoneVolumeHappinessEffect;
    private static int positiveHappinessEffect;

    public static int PlayerSpeed { get { return playerSpeed; } }
    public static int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public static int VisitorSpeed { get { return visitorSpeed; } }
    public static int SecondsToNightStarts { get { return secondsToNightStarts; } }
    public static int BeerSoldRewardInCoins { get { return beerSoldRewardInCoins; } }
    public static int BeerSoldRewardInSouls { get { return beerSoldRewardInSouls; } }
    public static int DrinksToBecomeDefenderCard { get { return drinksToBecomeDefenderCard; } }
    public static int VisitorsSpawnDelayMin { get { return visitorsSpawnDelayMin; } }
    public static int VisitorsSpawnDelayMax { get { return visitorsSpawnDelayMax; } }
    public static int MaxVisitersQuantity { get { return maxVisitersQuantity; } }
    public static int CarrotsPlayerHolding { get { return carrotsPlayerHolding; } }
    public static int SecondsToGetHungry { get { return secondsToGetHungry; } }
    public static int MaxCarrotsInBarrel { get { return maxCarrotsInBarrel; } }
    public static int OnBeerDrinkStrengthReward { get { return onBeerDrinkStrengthReward; } }
    public static int OnCarrotEatStrengthReward { get { return onCarrotEatStrengthReward; } }
    public static int CarrotSoldRewardInSouls { get { return carrotSoldRewardInSouls; } }
    public static int CarrotSoldRewardInCoins { get { return carrotSoldRewardInCoins; } }
    public static int DirtyTableHappinessEffect { get { return dirtyTableHappinessEffect; } }
    public static int EmptyCarrotBarrelHappinessEffect { get { return emptyCarrotBarrelHappinessEffect; } }
    public static int LowGramophoneVolumeHappinessEffect { get { return lowGramophoneVolumeHappinessEffect; } }
    public static int PositiveHappinessEffect { get { return positiveHappinessEffect; } }






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
        CurrentGameConfigHandler();
    }

    private void CurrentGameConfigHandler()
    {
        if (selectedPresetIndex >= 0 && selectedPresetIndex < presets.Length)
        {
            playerSpeed = presets[selectedPresetIndex].PlayerSpeed;
            visitorSpeed = presets[selectedPresetIndex].VisitorSpeed;
            visitorSecondsToLeave = presets[selectedPresetIndex].VisitorSecondsToLeave;
            secondsToNightStarts = presets[selectedPresetIndex].SecondsToNightStarts;
            beerSoldRewardInCoins = presets[selectedPresetIndex].BeerSoldRewardInCoins;
            beerSoldRewardInSouls = presets[selectedPresetIndex].BeerSoldRewardInSouls;
            drinksToBecomeDefenderCard = presets[selectedPresetIndex].DrinksToBecomeDefenderCard;
            maxVisitersQuantity = presets[selectedPresetIndex].MaxVisitersQuantity;
            visitorsSpawnDelayMin = presets[selectedPresetIndex].VisitorsSpawnDelayMin;
            visitorsSpawnDelayMax = presets[selectedPresetIndex].VisitorsSpawnDelayMax;
            secondsToGetHungry = presets[selectedPresetIndex].SecondsToGetHungry;
            carrotsPlayerHolding = presets[selectedPresetIndex].CarrotsPlayerHolding;
            maxCarrotsInBarrel = presets[selectedPresetIndex].MaxCarrotsInBarrel;
            onBeerDrinkStrengthReward = presets[selectedPresetIndex].OnBeerDrinkStrengthReward;
            onCarrotEatStrengthReward = presets[selectedPresetIndex].OnCarrotEatStrengthReward;
            carrotSoldRewardInSouls = presets[selectedPresetIndex].CarrotSoldRewardInSouls;
            carrotSoldRewardInCoins = presets[selectedPresetIndex].CarrotSoldRewardInCoins;
            dirtyTableHappinessEffect = presets[selectedPresetIndex].DirtyTableHappinessEffect;
            emptyCarrotBarrelHappinessEffect = presets[selectedPresetIndex].EmptyCarrotBarrelHappinessEffect;
            lowGramophoneVolumeHappinessEffect = presets[selectedPresetIndex].LowGramophoneVolumeHappinessEffect;
            positiveHappinessEffect = presets[selectedPresetIndex].PositiveHappinessEffect;

        }
        else
        {
            Debug.LogError("Invalid preset index!");
        }
    }
}



