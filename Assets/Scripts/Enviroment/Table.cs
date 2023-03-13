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
    [SerializeField] private GameObject visitorInfoBar;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private Image orderImage;
    [SerializeField] private Image warningSignImage;
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerSprite;
    [SerializeField] private MeshRenderer tableMeshRenderer;
    [SerializeField] private Texture cleanTableAlbedo;
    [SerializeField] private Texture cleanTableMetallic;
    [SerializeField] private Texture dirtyTableAlbedo;
    [SerializeField] private Texture dirtyTableMetallic;


    private bool isEmpty = true;
    private bool isDirty = false;
    private bool isServed;
    private VisitorAI visitor;
    private int drinksCount;
    private int secondsToLeave;
    private IObjectPool<GameObject> pool;
    private VisitorType currentVisitorType;
    private Sprite currentBeerSprite;
    private Material tableMaterial;

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
        visitorInfoBar.SetActive(false);
        tableMaterial = tableMeshRenderer.material;
    }

    public void TableGetDirty()
    {
        isDirty = true;
        TavernEventsManager.HappinessRateChanged(-GameConfigManager.DirtyTableHappinessEffect);
        warningSignImage.enabled = true;
        tableMaterial.SetTexture("_MainTex", dirtyTableAlbedo);
        tableMaterial.SetTexture("_MetallicGlossMap", dirtyTableMetallic);
    }

    public void TableCleaned()
    {
        isDirty = false;
        TavernEventsManager.HappinessRateChanged(GameConfigManager.DirtyTableHappinessEffect);
        warningSignImage.enabled = false;
        tableMaterial.SetTexture("_MainTex", cleanTableAlbedo);
        tableMaterial.SetTexture("_MetallicGlossMap", cleanTableMetallic);
    }

    public void ClearVisitor()
    {
        isEmpty = true;
        visitor = null;
        visitorInfoBar.SetActive(false);
        StopCoroutine(tryToTakeCarrotCoroutine);
    }

    public override void PlayerInteraction()
    {
        if (isDirty && player.isHoldingCleaningMaterials)
        {
            TableCleaned();
        }
        if (isDirty)
        {
            return;
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
        VisiterGoingOut();
    }

    public void VisiterGoingOut()
    {
        visitorInfoBar.SetActive(false);
        isEmpty = true;
        if(tryToTakeCarrotCoroutine != null)
        {
            StopCoroutine(tryToTakeCarrotCoroutine);
        }
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
