using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorAI : PlayerInterractible
{

    private Transform target;
    private CharacterController controller;
    private BoxCollider coll;
    private int _strength;
    private string _type;

    [SerializeField] private float speed = 1f;
    [SerializeField] private int secondsToLeave = 15;

    private bool isDrunk = false;

    private Coroutine timerCoroutine = null;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;

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
        timerCoroutine = StartCoroutine(VisitorLeaveTimer());
    }

    IEnumerator VisitorLeaveTimer()
    {
        yield return new WaitForSeconds(secondsToLeave);
        GetOut();
    }

    public override void PlayerInterraction()
    {
        if (player.isHoldingGlassOfBeer)
        {
            if (!isDrunk)
            {
                StopCoroutine(timerCoroutine);
                isDrunk = true;

            }
            player.SellGlassOfBeer();
            SetStrength(_strength + 1);
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
        SetTarget(FindObjectOfType<TavernDoor>().transform);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void SetStats(int strength, string type)
    {
        _strength = strength;
        _type = type;
    }
    public void SetStrength(int value)
    {
        _strength = value;
    }
}
