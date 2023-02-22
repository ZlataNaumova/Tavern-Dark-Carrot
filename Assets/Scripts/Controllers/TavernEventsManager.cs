using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TavernEventsManager
{
    public static event UnityAction HeartRepaired;
    public static void OnHeartRepaired() => HeartRepaired?.Invoke();

    public static event UnityAction NightStarted;
    public static void OnNightStarted() => NightStarted?.Invoke();

    public static event UnityAction DayStarted;
    public static void OnDayStarted() => DayStarted?.Invoke();

    public static event UnityAction<int> CoinsValueChanged;
    public static void OnCoinsValueChanged(int newCoinsValue) => CoinsValueChanged?.Invoke(newCoinsValue);

    public static event UnityAction<int> SoulsValueChanged;
    public static void OnSoulsValueChanged(int newSoulsValue) => SoulsValueChanged?.Invoke(newSoulsValue);

    public static event UnityAction<int> CoinsAdded;
    public static void OnCoinsAdded(int coinsValue) => CoinsAdded?.Invoke(coinsValue);

    public static event UnityAction<int> SoulsAdded;
    public static void OnSoulsAdded(int soulsValue) => SoulsAdded?.Invoke(soulsValue);

    public static event UnityAction OneBeerGlassSold;
    public static void OnOneBeerGlassSold()
    {
        CoinsAdded?.Invoke(GameConfigManager.BeerSoldRewardInCoins);
        SoulsAdded?.Invoke(GameConfigManager.BeerSoldRewardInSouls);
        OneBeerGlassSold?.Invoke();
    }

    public static event UnityAction<VisitorAI> VisitorTriedTakeCarrot;
    public static void OnVisitorTriedTakeCarrot(VisitorAI visitor) => VisitorTriedTakeCarrot?.Invoke(visitor);

    public static event UnityAction<VisitorAI> DefenderAdded;
    public static void OnDefenderAdded(VisitorAI v) => DefenderAdded?.Invoke(v);

    public static event UnityAction<VisitorAI> VisitorBecomeDefenderCard;
    public static void OnVisitorBecomeDefenderCard(VisitorAI defender) => VisitorBecomeDefenderCard?.Invoke(defender);

    public static event UnityAction<List<VisitorAI>> DefendersToCards;
    public static void OnDefendersToCards(List<VisitorAI> defenders) => DefendersToCards?.Invoke(defenders);

    public static event UnityAction<GameObject> VisitorLeftTavern;
    public static void OnVisitorLeftTavern(GameObject visiter) => VisitorLeftTavern?.Invoke(visiter);

    public static event UnityAction SwitchedToNightCanvas;
    public static void OnSwitchedToNightCanvas() => SwitchedToNightCanvas?.Invoke();

    public static event UnityAction SwitchedToDayCanvas;
    public static void OnSwitchedToDayCanvas() => SwitchedToDayCanvas?.Invoke();

    public static event UnityAction CameraSwitchedToCardGame;
    public static void OnCameraSwitchedToCardGame() => CameraSwitchedToCardGame?.Invoke();

    public static event UnityAction CameraSwitchedToFollowPlayer;
    public static void OnCameraSwitchedToFollowPlayer() => CameraSwitchedToFollowPlayer?.Invoke();

    public static event UnityAction CardsRendered;
    public static void OnCardsRendered() => CardsRendered?.Invoke();

    public static event UnityAction<PlayingCardView> PlayerChoseCard;
    public static void OnPlayerChoseCard(PlayingCardView card) => PlayerChoseCard?.Invoke(card);
}
