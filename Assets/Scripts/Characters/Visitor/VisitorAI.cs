using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class VisitorAI : PlayerInterractible
{

    private Transform target;
    private CharacterController controller;
    private BoxCollider coll;
    private int strength;
    private int type;
    private int drinksCount;
    private IObjectPool<GameObject> pool;

    [SerializeField] private Image leaveTimerBar;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int secondsToLeave = 15;

    private bool isDrunk;
    public bool isLeaving;


    private Coroutine timerCoroutine = null;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;


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
        StartCoroutine(SnapToChairCoroutine());
        timerCoroutine = StartCoroutine(VisitorLeaveTimer(secondsToLeave));
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

    public override void PlayerInterraction()
    {
        if (player.isHoldingGlassOfBeer)
        {
            if (!isDrunk)
            {
                StopCoroutine(timerCoroutine);
                isDrunk = true;
                leaveTimerBar.enabled = false;

            }
            player.SellGlassOfBeer();
            drinksCount++;
            StatsUpdate(drinksCount);
        }
    }

    private void StatsUpdate(int drinksCount)
    {
        switch (drinksCount){
            case 1:
                strength += 10;
                TavernEventsManager.OnAddCoins(10);
                TavernEventsManager.OnAddSouls(3);
                break;
            case 2:
                strength += 10;
                TavernEventsManager.OnAddCoins(15);
                TavernEventsManager.OnAddSouls(2);
                break;
            case 3:
                strength += 10;
                TavernEventsManager.OnAddCoins(20);
                TavernEventsManager.OnAddSouls(1);
                break;
            default:
                break;
        }
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
        SetTarget(FindObjectOfType<TavernDoor>().transform);
    }

    public void ReachedDoor()
    {
        TavernEventsManager.OnVisitorLeaveTavern(this);
        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetStats(int strength, int type)
    {
        this.strength = strength;
        this.type = type;
    }
    public void SetPool(ObjectPool<GameObject> myPool)
    {
        pool = myPool;
    }

}
