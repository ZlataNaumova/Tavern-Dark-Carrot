using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class VisitorAI : MonoBehaviour
{
    private Transform target;
    private VisitorTargets currentTargetType;
    private Table occupiedTable;
    private CharacterController characterController;
    private int strength = 1;
    private int defenderType;
    private float targetThreshold = 0.2f;

    public int Strength => strength;
    public int DefenderType => defenderType;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(target)
        {
            MoveToTarget();
        }
    }
   
    private void MoveToTarget()
    {
        
        if (target)
        {
            Vector3 targetDirection = (target.transform.position - this.transform.position).normalized;
            characterController.Move(new Vector3(targetDirection.x, 0f, targetDirection.z) * GameConfigManager.VisitorSpeed * Time.deltaTime);

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < targetThreshold) 
            {
                Debug.Log("Reached Table");
                ReachTargetHandler();
            }
        }
    }

    private void ReachTargetHandler()
    {
        switch (currentTargetType)
        {
            case VisitorTargets.Table:
                occupiedTable.VisitorReachTheTable();
                break;
            case VisitorTargets.Door:
                TavernEventsManager.VisitorLeftTavern(gameObject);
                break;
            default:
                break;
        }
        target = null;
    }

    public void SetTarget(Transform newTarget, VisitorTargets targetType)
    {
        target = newTarget;
        currentTargetType = targetType;
        if(targetType == VisitorTargets.Table)
        {
            occupiedTable = newTarget.GetComponentInParent<Table>();


        }
    }

    public void SetStats(int strength, int type)
    {
        this.strength = strength;
        this.defenderType = type;
    }
   
    public void OnBeerDrinkEffect() => strength += GameConfigManager.OnBeerDrinkStrengthReward;

    public void OnCarrotEatEffect() => strength += GameConfigManager.OnCarrotEatStrengthReward;
        
}
