using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float targetThreshold = 0.2f;
    private VisitorType visitorType;

    public int Strength => strength;
    public VisitorType CurrentType => visitorType;

    private void Start() => characterController = GetComponent<CharacterController>();

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

    public void SetStats(int strength, VisitorType type)
    {
        this.strength = strength;
        this.visitorType = type;
    }

    public void OnBeerDrinkEffect()
    {
        AddStrength(GameConfigManager.OnBeerDrinkStrengthReward);
    }

    public void OnCarrotEatEffect()
    {
        AddStrength(GameConfigManager.OnCarrotEatStrengthReward);
    }

    public void AddStrength(int valueToAdd)
    {
        strength += valueToAdd;
    }

}
public enum VisitorType
{
    VisitorType1,
    VisitorType2
}
