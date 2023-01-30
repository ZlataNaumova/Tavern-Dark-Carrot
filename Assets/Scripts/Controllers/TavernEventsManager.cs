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
}
