using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendersManager : MonoBehaviour
{
    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private Transform defenderSpawnPoint;
    [SerializeField] private List<HirePoint> defendersHirePoints;
    [SerializeField] private List<DefenderTable> defendersTables;
    [SerializeField] private float spawnDefendersDelay;
    [SerializeField] private int defendersSpawnQuantity;
    [SerializeField] private int defendersNightReadyQuantity;

    private int hiredDefendersQuantity = 0;


    private void OnEnable()
    {
        TavernEventsManager.HeartRepaired += StartDefendersSpawn;
        TavernEventsManager.DefenderHired += DefenderHired;
    }


    private void OnDisable()
    {
        TavernEventsManager.HeartRepaired -= StartDefendersSpawn;
        TavernEventsManager.DefenderHired -= DefenderHired;
    }

    private void StartDefendersSpawn()
    {
        StartCoroutine(SpawnDelayCoroutine());
    }

    IEnumerator SpawnDelayCoroutine()
    {
        for(int i = 0; i < defendersSpawnQuantity; ++i)
        {
            TrySpawnDefender();
            yield return new WaitForSeconds(spawnDefendersDelay);
        }
        
        
    }

    private void TrySpawnDefender()
    {
        HirePoint emptyHirePoint;
        if(TryGetEmptyHirePoint(out emptyHirePoint))
        {
            emptyHirePoint.isEmpty = false;
            GameObject defender = Instantiate(defenderPrefab, defenderSpawnPoint);
            defender.GetComponent<Defender>().SetTarget(emptyHirePoint.transform);
        } else
        {
            Debug.Log("No empty hire places");
        }
        
    }

   
    private void DefenderHired(Defender defender)
    {
        DefenderTable emptyDefenderTable;
        if(TryGetEmptyDefenderTable(out emptyDefenderTable))
        {
            emptyDefenderTable.isEmpty = false;
            defender.SetTarget(emptyDefenderTable.transform);
            hiredDefendersQuantity++;
            if(hiredDefendersQuantity >= defendersNightReadyQuantity)
            {
                TavernEventsManager.OnTavernReadyForNight();
            }
        }
        else
        {
            Debug.Log("No empty Tables");
        }

    }

    private bool TryGetEmptyHirePoint(out HirePoint hirePoint)
    {
        hirePoint = defendersHirePoints.Find(hp => hp.isEmpty == true);
        return (hirePoint != null);
    }

    private bool TryGetEmptyDefenderTable(out DefenderTable defenderTable)
    {
        defenderTable = defendersTables.Find(dt => dt.isEmpty == true);
        return (defenderTable != null);
    }
}


