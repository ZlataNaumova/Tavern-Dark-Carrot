using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Table : PlayerInteractable
{
    [SerializeField] private Transform visitorTargetPoint;
    [SerializeField] private Image leaveTimerBar;
    private bool isEmpty = true;
    private bool isDirty = false;
    private bool isVisitorReachTheTable = false;
    private bool isServed;
    private bool isLeaving;
    private bool isHungry;
    private VisitorAI visitor;
    private int drinksCount;
    private int secondsToLeave;
    private IObjectPool<GameObject> pool;

    private Coroutine visitorLeaveTimer = null;
    private Coroutine tryToTakeCarrotCoroutine = null;

    public bool IsEmpty { get { return isEmpty; } }
    public Transform VisitorTargetPoint { get { return visitorTargetPoint; } }
    public VisitorAI Visitor { get { return visitor; } }

    private void Start()
    {
        secondsToLeave = GameConfigManager.VisitorSecondsToLeave;
        leaveTimerBar.enabled = false;

    }

    public void GetDirty()
    {
        isDirty = true;
    }

    public void ClearVisitor()
    {
        isEmpty = true;
        visitor = null;
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingGlassOfBeer)
        {

            if (!isServed)
            {
                StopCoroutine(visitorLeaveTimer);
                isServed = true;
                leaveTimerBar.enabled = false;
            }
            player.SellGlassOfBeer();
            drinksCount++;
            TavernEventsManager.OneBeerGlassSold(visitor);
            if (drinksCount >= GameConfigManager.DrinksToBecomeDefenderCard)
            {
                VisitorBecomeDefenderCardHandler();
            }
        }
    }

    private void VisitorBecomeDefenderCardHandler()
    {
        StopCoroutine(tryToTakeCarrotCoroutine);
        TavernEventsManager.VisitorBecomeDefenderCard(visitor);
    }

    public void SetVisitor(VisitorAI newVisitor)
    {
        visitor = newVisitor;
        isEmpty = false;
    }

    public void VisitorReachTheTable()
    {
        isVisitorReachTheTable = true;
        leaveTimerBar.enabled = true;
        visitorLeaveTimer = StartCoroutine(VisitorLeaveTimer(GameConfigManager.VisitorSecondsToLeave));
        tryToTakeCarrotCoroutine = StartCoroutine(TryToTakeCarrotCoroutine(GameConfigManager.SecondsToGetHungry));
    }

    IEnumerator VisitorLeaveTimer(int secondsToLeave)
    {
        int counter = secondsToLeave;
        while (counter > 0)
        {
            counter--;
            leaveTimerBar.fillAmount = (float)counter / secondsToLeave;
            yield return new WaitForSeconds(1);
        }
        isLeaving = true;
        isEmpty = true;
        isVisitorReachTheTable = false;
        StopCoroutine(tryToTakeCarrotCoroutine);
        visitor.SetTarget(FindObjectOfType<TavernDoor>().VisitorTargetPoint, VisitorTargets.Door);
    }

    IEnumerator TryToTakeCarrotCoroutine(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            isHungry = true;
            TavernEventsManager.VisitorTriedTakeCarrot(visitor);
        }

    }


}
