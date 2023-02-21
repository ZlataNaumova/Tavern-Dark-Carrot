using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Config", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player Config")]
    [SerializeField] private int playerSpeed;
    public int PlayerSpeed { get { return playerSpeed; } }

    [Header("Visitors Config")]
    [SerializeField] private int visitorSpeed;
    [SerializeField] private int visitorSecondsToLeave;
    [SerializeField] private int drinksToBecomeDefenderCard;
    [SerializeField] private int visitorsSpawnDelayMin;
    [SerializeField] private int visitorsSpawnDelayMax;
    [SerializeField] private int maxVisitersQuantity;

    public int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public int VisitorSpeed { get { return visitorSpeed; } }
    public int DrinksToBecomeDefenderCard { get { return drinksToBecomeDefenderCard; } }
    public int VisitorsSpawnDelayMin { get { return visitorsSpawnDelayMin; } }
    public int VisitorsSpawnDelayMax { get { return visitorsSpawnDelayMax; } }
    public int MaxVisitersQuantity { get { return maxVisitersQuantity; } }


    [Header("Main Config")]
    [SerializeField] private int secondsToNightStarts;
    [SerializeField] private int beerSoldRewardInCoins;
    [SerializeField] private int beerSoldRewardInSouls;

    public int SecondsToNightStarts { get { return secondsToNightStarts; } }
    public int BeerSoldRewardInCoins { get { return beerSoldRewardInCoins; } }
    public int BeerSoldRewardInSouls { get { return beerSoldRewardInSouls; } }

    //[Header("Card Game Config")]

}
