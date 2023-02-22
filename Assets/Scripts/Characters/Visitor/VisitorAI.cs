using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class VisitorAI : PlayerInteractable
{
    [SerializeField] private Image leaveTimerBar;

    private Transform target;
    private Chair occupiedChair;
    private CharacterController controller;
    private BoxCollider coll;
    private int strength = 1;
    private int defenderType;
    private int drinksCount;
    private IObjectPool<GameObject> pool;
    private float speed;
    private int secondsToLeave;
    private bool isDrunk;
    public bool isLeaving;
    private bool isHungry;
    private Coroutine visitorLeaveTimer = null;
    private Coroutine tryToTakeCarrotCoroutine = null;

    public int Strength => strength;
    public int DefenderType => defenderType;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;

        secondsToLeave = GameConfigManager.VisitorSecondsToLeave;
        speed = GameConfigManager.VisitorSpeed;

    }
    private void OnEnable()
    {
        isDrunk = false;
        isLeaving = false;
    }

    private void Update()
    {
        if(target)
        {
            MoveToTarget();
        }
    }
 
    public void VisitorSits()
    {
        coll.enabled = true;
        leaveTimerBar.enabled = true;
        StartCoroutine(SnapToChairCoroutine());
        visitorLeaveTimer = StartCoroutine(VisitorLeaveTimer(secondsToLeave));
        tryToTakeCarrotCoroutine = StartCoroutine(TryToTakeCarrotCoroutine(GameConfigManager.SecondsToGetHungry));
    }

    IEnumerator VisitorLeaveTimer(int secondsToLeave)
    {
        int counter = secondsToLeave;
        while(counter > 0)
        {
            counter--;
            leaveTimerBar.fillAmount = 1 / (float)secondsToLeave * counter;
            yield return new WaitForSeconds(1);
                    }
            GetOut();
    }

    IEnumerator SnapToChairCoroutine()
    {
        yield return new WaitForSeconds(.4f);
        target = null;
    }

    IEnumerator TryToTakeCarrotCoroutine(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            isHungry = true;
            TavernEventsManager.OnVisitorTriedTakeCarrot(this);
        }
       
    }

    public void EatCarrot()
    {
        isHungry = false;
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingGlassOfBeer)
        {
            
            if (!isDrunk)
            {
                StopCoroutine(visitorLeaveTimer);
                isDrunk = true;
                leaveTimerBar.enabled = false;
                //TavernEventsManager.OnDefenderAdded(this);
            }
            player.SellGlassOfBeer();
            drinksCount++;
            strength += 10;
            TavernEventsManager.OnOneBeerGlassSold();
            if (drinksCount >= GameConfigManager.DrinksToBecomeDefenderCard)
            {
                VisitorBecomeDefenderCardHandler();
            }
        }
    }

    private void VisitorBecomeDefenderCardHandler()
    {
        occupiedChair.isEmpty = true;
        StopCoroutine(tryToTakeCarrotCoroutine);
        TavernEventsManager.OnVisitorBecomeDefenderCard(this);
        pool.Release(gameObject);
    }

    private void MoveToTarget()
    {
        if (target)
        {
            Vector3 targetDirection = (target.transform.position - this.transform.position).normalized;
            controller.Move(new Vector3(targetDirection.x, 0f, targetDirection.z) * speed * Time.deltaTime);
        }
    }

    private void GetOut()
    {
        isLeaving = true;
        coll.enabled = false;
        occupiedChair.isEmpty = true;
        StopCoroutine(tryToTakeCarrotCoroutine);
        SetTarget(FindObjectOfType<TavernDoor>().transform);
    }

    public void ReachedDoor()
    {
        TavernEventsManager.OnVisitorLeftTavern(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        occupiedChair = target.GetComponent<Chair>();
        if(occupiedChair != null)
        {
            occupiedChair.isEmpty = false;
        }
    }

    public void SetStats(int strength, int type)
    {
        this.strength = strength;
        this.defenderType = type;
    }
    public void SetPool(ObjectPool<GameObject> myPool)
    {
        pool = myPool;
    }

    
}
