using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    private IDefenderState currentState;
    private Transform target;
    private CharacterController controller;
    private bool isHired = false;
    private bool isChilling = false;

    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject playerInterractionalCollider;

    private IDefenderState moveToTargetState = new DefenderMoveToTargetState();
    private IDefenderState chillingState = new DefenderChillingState();


    private void Start()
    {
        currentState = moveToTargetState;
        controller = GetComponent<CharacterController>();
        playerInterractionalCollider.GetComponent<BoxCollider>().enabled = false;

    }

    private void Update()
    {
        currentState = currentState.DoState(this);
    }

   
    public void MoveToTarget()
    {
        if(target)
        {
            Vector3 targetDirection = (target.transform.position - this.transform.position).normalized;
            controller.Move(new Vector3(targetDirection.x, 0f, targetDirection.z) * speed * Time.deltaTime);
        }
    }
  


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void Chill()
    {

    }

    public void GetHired()
    {
        playerInterractionalCollider.GetComponent<PlayerInterractionalCollider>().SetIsDefenderHired(true);
        playerInterractionalCollider.GetComponent<BoxCollider>().enabled = false;
        playerInterractionalCollider.GetComponent<PlayerInterractionalCollider>().SetAllPanelsInactive();
        isHired = true;
        isChilling = false;
        TavernEventsManager.OnDefenderHired(this);
    }

    public void GetKickedOut()
    {
        
    }

    public void DefenderIsOnHiringPoint()
    {
        isChilling = true;
        playerInterractionalCollider.GetComponent<BoxCollider>().enabled = true;
    }
    public void DefenderOnTable()
    {
        isChilling = true;
        playerInterractionalCollider.GetComponent<BoxCollider>().enabled = true;

    }

    public IDefenderState DefenderStateTransitionHandler()
    {
        
        if (isChilling)
        {
            return chillingState;
        } else
        {
            return moveToTargetState;
        }
        
    }
    
}
