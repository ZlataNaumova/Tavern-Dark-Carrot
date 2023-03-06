using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Table : PlayerInteractable
{
    [SerializeField] private Transform visitorTargetPoint;
    [SerializeField] private Image leaveTimerBar;
    [SerializeField] private GameObject dirt;

    [SerializeField] private GameObject visitorInfoBar;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private Image orderImage;
    [SerializeField] private Image warningSignImage;
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerSprite;

    private bool isEmpty = true;
    private bool isDirty = false;
    private bool isServed;
    private VisitorAI visitor;
    private int drinksCount;
    private int secondsToLeave;
    private IObjectPool<GameObject> pool;
    private VisitorType currentVisitorType;
    private Sprite currentBeerSprite;

    private Coroutine visitorLeaveTimer = null;
    private Coroutine tryToTakeCarrotCoroutine = null;

    public bool IsEmpty { get { return isEmpty; } }
    public Transform VisitorTargetPoint { get { return visitorTargetPoint; } }
    public VisitorAI Visitor { get { return visitor; } }

    private void Start()
    {
        secondsToLeave = GameConfigManager.VisitorSecondsToLeave;
        leaveTimerBar.enabled = false;
        warningSignImage.enabled = false;
        dirt.SetActive(false);
        visitorInfoBar.SetActive(false);
    }

    public void TableGetDirty()
    {
        isDirty = true;
        dirt.SetActive(true);
        TavernEventsManager.HappinessRateChanged(-GameConfigManager.DirtyTableHappinessEffect);
        warningSignImage.enabled = true;
    }

    public void TableCleaned()
    {
        isDirty = false;
        dirt.SetActive(false);
        TavernEventsManager.HappinessRateChanged(GameConfigManager.DirtyTableHappinessEffect);
        warningSignImage.enabled = false;
    }

    public void ClearVisitor()
    {
        isEmpty = true;
        visitor = null;
        visitorInfoBar.SetActive(false);

    }

    public override void PlayerInteraction()
    {
        if (isDirty && player.isHoldingCleaningMaterials)
        {
            TableCleaned();
        }
        if (player.isHoldingGlassOfBeer && !isEmpty)
        {
            if (!isDirty)
            {
                TableGetDirty();
            }

            if (!isServed)
            {
                StopCoroutine(visitorLeaveTimer);
                isServed = true;
                leaveTimerBar.enabled = false;
            }

            player.ReleaseGlassOfBeer();
            drinksCount++;
            TavernEventsManager.OneBeerGlassSold(visitor);
            UpdateVisitorUI();
            if (drinksCount >= GameConfigManager.DrinksToBecomeDefenderCard)
            {
                VisitorBecomeDefenderCardHandler();
            }
        }
    }

    private void VisitorBecomeDefenderCardHandler()
    {
        StopCoroutine(tryToTakeCarrotCoroutine);
        visitorInfoBar.SetActive(false);
        TavernEventsManager.VisitorBecomeDefenderCard(visitor);
    }

    public void SetVisitor(VisitorAI newVisitor)
    {
        visitor = newVisitor;
        currentBeerSprite = visitor.CurrentType == VisitorType.VisitorType1 ? greenBeerSprite : redBeerSprite;
        isEmpty = false;
        UpdateVisitorUI();
    }

    public void VisitorReachTheTable()
    {
        leaveTimerBar.enabled = true;
        visitorLeaveTimer = StartCoroutine(VisitorLeaveTimer(GameConfigManager.VisitorSecondsToLeave));
        tryToTakeCarrotCoroutine = StartCoroutine(TryToTakeCarrotCoroutine(GameConfigManager.SecondsToGetHungry));
        visitorInfoBar.SetActive(true);
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
        visitorInfoBar.SetActive(false);
        isEmpty = true;
        StopCoroutine(tryToTakeCarrotCoroutine);
        visitor?.SetTarget(FindObjectOfType<TavernDoor>().VisitorTargetPoint, VisitorTargets.Door);
    }

    IEnumerator TryToTakeCarrotCoroutine(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            TavernEventsManager.VisitorTriedTakeCarrot(visitor);
        }

    }

    private void UpdateVisitorUI()
    {
        strengthText.text = visitor.Strength.ToString();
        orderImage.sprite = currentBeerSprite;
    }


}
