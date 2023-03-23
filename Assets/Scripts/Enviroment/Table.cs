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
    private int oneBeerPrice;
    private int beerSellBonus;
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
        oneBeerPrice = GameConfigManager.BeerSoldRewardInCoins;
    }

    private void OnEnable()
    {
        TavernEventsManager.OnBeerIncomeImproved += BeerIncomeImprovedHandler;
    }
    private void OnDisable()
    {
        TavernEventsManager.OnBeerIncomeImproved -= BeerIncomeImprovedHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if (player.isHoldingCleaningMaterials || player.isHoldingGlassOfBeer)
            {
                player.SetTarget(gameObject);
                outline.OutlineWidth = 2.5f;
            }

        }
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
        if(tryToTakeCarrotCoroutine != null)
        {
            StopCoroutine(tryToTakeCarrotCoroutine);
        }
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
            //TavernEventsManager.OneBeerGlassSold(visitor);
            visitor.OnBeerDrinkEffect();
            UpdateVisitorUI();
            TavernEventsManager.CoinsAdded(oneBeerPrice + beerSellBonus);
            TavernEventsManager.SoulsAdded(GameConfigManager.BeerSoldRewardInSouls);

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
       float time = secondsToLeave;
       float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            leaveTimerBar.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            yield return null;
        }
        VisiterGoingOut();
    }

    
public void VisiterGoingOut()
    {
        drinksCount = 0;
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

    private void BeerIncomeImprovedHandler()
    {
        beerSellBonus++;
    }

}
