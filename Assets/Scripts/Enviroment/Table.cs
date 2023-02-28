using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Table : PlayerInteractable
{
    [SerializeField] private Transform visitorTargetPoint;
    [SerializeField] private Image leaveTimerBar;
    [SerializeField] private GameObject dirt;
    private bool isEmpty = true;
    private bool isDirty = false;
    private bool isServed;
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
        dirt.SetActive(false);

    }

    public void GetDirty()
    {
        isDirty = true;
        dirt.SetActive(true);
        TavernEventsManager.HappinessRateChanged(-GameConfigManager.DirtyTableHappinessEffect);
    }

    public void GetCleaned()
    {
        isDirty = false;
        dirt.SetActive(false);
        TavernEventsManager.HappinessRateChanged(GameConfigManager.DirtyTableHappinessEffect);
    }

    public void ClearVisitor()
    {
        isEmpty = true;
        visitor = null;
    }

    public override void PlayerInteraction()
    {
        if (isDirty)
        {
            GetCleaned();
        }
        if (player.isHoldingGlassOfBeer)
        {
            if (!isDirty)
            {
                GetDirty();
            }

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
        isEmpty = true;
        StopCoroutine(tryToTakeCarrotCoroutine);
        visitor.SetTarget(FindObjectOfType<TavernDoor>().VisitorTargetPoint, VisitorTargets.Door);
    }

    IEnumerator TryToTakeCarrotCoroutine(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            TavernEventsManager.VisitorTriedTakeCarrot(visitor);
        }

    }


}
