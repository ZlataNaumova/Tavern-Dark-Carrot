using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TavernEventsManager 
{
    public static event UnityAction HeartRepaired;
    public static void OnHeartRepaired() => HeartRepaired?.Invoke();

    public static event UnityAction TavernReadyForNight;
    public static void OnTavernReadyForNight() => TavernReadyForNight?.Invoke();

    public static event UnityAction<int> CoinsValueChanged;
    public static void OnCoinsValueChanged(int newValue) => CoinsValueChanged?.Invoke(newValue);

    public static event UnityAction<int> SoulsValueChanged;
    public static void OnSoulsValueChanged(int newValue) => SoulsValueChanged?.Invoke(newValue);

    public static event UnityAction<int> AddCoins;
    public static void OnAddCoins(int value) => AddCoins?.Invoke(value);

    public static event UnityAction<int> AddSouls;
    public static void OnAddSouls(int value) => AddSouls?.Invoke(value);

    public static event UnityAction<VisitorAI> VisitorLeaveTavern;
    public static void OnVisitorLeaveTavern(VisitorAI v) => VisitorLeaveTavern?.Invoke(v);
}
