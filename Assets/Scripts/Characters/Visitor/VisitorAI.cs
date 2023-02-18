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
    private int strenght = 1;
    private int defenderType;
    private int drinksCount;
    private IObjectPool<GameObject> pool;

    [SerializeField] private Image leaveTimerBar;
    [SerializeField] private float speed;
    [SerializeField] private int secondsToLeave;

    private bool isDrunk;
    public bool isLeaving;

    private Coroutine timerCoroutine = null;

    public int Strenght => strenght;
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
                TavernEventsManager.OnVisitorBecomeDefender(this);
            }
            player.SellGlassOfBeer();
            drinksCount++;
            strenght += 10;
            TavernEventsManager.OnAddCoins(10);
            TavernEventsManager.OnAddSouls(3);
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
        this.strenght = strength;
        this.defenderType = type;
    }
    public void SetPool(ObjectPool<GameObject> myPool)
    {
        pool = myPool;
    }

    
}
