using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TavernEventsManager 
{
    public static event UnityAction HeartRepaired;
    public static void OnHeartRepaired() => HeartRepaired?.Invoke();

    public static event UnityAction NightStarts;
    public static void OnNightStarts() => NightStarts?.Invoke();

    public static event UnityAction DayStarts;
    public static void OnDayStarts() => DayStarts?.Invoke();

    public static event UnityAction<int> CoinsValueChanged;
    public static void OnCoinsValueChanged(int newValue) => CoinsValueChanged?.Invoke(newValue);

    public static event UnityAction<int> SoulsValueChanged;
    public static void OnSoulsValueChanged(int newValue) => SoulsValueChanged?.Invoke(newValue);

    public static event UnityAction<int> AddCoins;
    public static void OnAddCoins(int value) => AddCoins?.Invoke(value);

    public static event UnityAction<int> AddSouls;
    public static void OnAddSouls(int value) => AddSouls?.Invoke(value);

    public static event UnityAction<VisitorAI> VisitorBecomeDefender;
    public static void OnVisitorBecomeDefender(VisitorAI v) => VisitorBecomeDefender?.Invoke(v);

    public static event UnityAction<List<VisitorAI>> DefendersToCards;
    public static void OnDefendersToCards(List<VisitorAI> defenders) => DefendersToCards?.Invoke(defenders);

    public static event UnityAction<VisitorAI> VisitorLeaveTavern;
    public static void OnVisitorLeaveTavern(VisitorAI v) => VisitorLeaveTavern?.Invoke(v);

    public static event UnityAction SwitchToNightCanvas;
    public static void OnSwitchToNightCanvas() => SwitchToNightCanvas?.Invoke();

    public static event UnityAction SwitchToDayCanvas;
    public static void OnSwitchToDayCanvas() => SwitchToDayCanvas?.Invoke();

    public static event UnityAction CameraSwitchToCardGame;
    public static void OnCameraSwitchToCardGame() => CameraSwitchToCardGame?.Invoke();

    public static event UnityAction CameraSwitchToFollowPlayer;
    public static void OnCameraSwitchToFollowPlayer() => CameraSwitchToFollowPlayer?.Invoke();

    public static event UnityAction RenderCards;
    public static void OnRenderCards() => RenderCards?.Invoke();

    public static event UnityAction<PlayingCardView> PlayerChooseCard;
    public static void OnPlayerChooseCard(PlayingCardView card) => PlayerChooseCard?.Invoke(card);
}
