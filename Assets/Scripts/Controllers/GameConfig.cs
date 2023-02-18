using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Config", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player Config")]
    [SerializeField] private int playerSpeed = 6;
    public int PlayerSpeed { get { return playerSpeed; } }

    [Header("Visitors Config")]
    [SerializeField] private int visitorSpeed = 3;
    [SerializeField] private int visitorSecondsToLeave = 5;
    public int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public int VisitorSpeed { get { return visitorSpeed; } }

    [Header("Main Config")]
    [SerializeField] private int secondsToNightStarts = 120;
    public int SecondsToNightStarts { get { return secondsToNightStarts; } }

    //[Header("Card Game Config")]
    
}
