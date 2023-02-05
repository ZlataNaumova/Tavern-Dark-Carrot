using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VisitorsManager : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private Transform visitorSpawnPoint;
    [SerializeField] private int visitorsSpawnQuantity;
    [SerializeField] private int visitorsSpawnDelayMin;
    [SerializeField] private int visitorsSpawnDelayMax;

    private System.Random random = new System.Random();
    private Chair[] chairs;
    private List<VisitorAI> defenders = new List<VisitorAI>();
    private ObjectPool<GameObject> pool;

    private void OnEnable()
    {
        TavernEventsManager.HeartRepaired += StartVisitersSpawn;
        TavernEventsManager.VisitorLeaveTavern += RespawnVisitor;
        TavernEventsManager.VisitorBecomeDefender += AddVisitorToDefendersList;
        TavernEventsManager.NightStarts += NightHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.HeartRepaired -= StartVisitersSpawn;
        TavernEventsManager.VisitorLeaveTavern -= RespawnVisitor;
        TavernEventsManager.VisitorBecomeDefender -= AddVisitorToDefendersList;
        TavernEventsManager.NightStarts -= NightHandler;
    }
   
    private void AddVisitorToDefendersList(VisitorAI visitor)
    {
        defenders.Add(visitor);
    }

    private void Start() => chairs = FindObjectsOfType<Chair>();
    
    private void Awake() => pool = new ObjectPool<GameObject>(CreateVisitor, OnGetVisitorFromPool, OnReturnVisitorToPool);

    private GameObject CreateVisitor()
    {
        var v = Instantiate(visitorPrefab, visitorSpawnPoint);
        v.GetComponent<VisitorAI>().SetPool(pool);
        v.SetActive(false);
        return v;
    }

    private void OnGetVisitorFromPool(GameObject visitor)
    {
        Chair emptyChair;
        VisitorAI v;
        if (TryGetEmptyChair(out emptyChair))
        {
            emptyChair.isEmpty = false;
            visitor.SetActive(true);
            v = visitor.GetComponent<VisitorAI>();
            v.SetStats(10, 1);
            v.SetTarget(emptyChair.transform);
        }
    }

    private void OnReturnVisitorToPool(GameObject visitor)
    {
        visitor.SetActive(false);
    }

    private void RespawnVisitor(VisitorAI v)
    {
        StartCoroutine(SpawnOneVisitorCoroutine());
    }

    private void StartVisitersSpawn()
    {
        StartCoroutine(SpawnDelayCoroutine(visitorsSpawnQuantity));
    }

    IEnumerator SpawnDelayCoroutine(int quantity)
    {
        for (int i = 0; i < quantity; ++i)
        {
            TrySpawnVisitor();
            yield return new WaitForSeconds(random.Next(visitorsSpawnDelayMin,visitorsSpawnDelayMax));
        }
    }
    IEnumerator SpawnOneVisitorCoroutine()
    {
        yield return new WaitForSeconds(random.Next(visitorsSpawnDelayMin, visitorsSpawnDelayMax));
        TrySpawnVisitor();
    }

    private void TrySpawnVisitor()
    {
        pool.Get();
    }

    private bool TryGetEmptyChair(out Chair chair)
    {
        chair = null;
        Chair[] emptyChairs = Array.FindAll(chairs, chair => chair.isEmpty == true);
        if(emptyChairs.Length > 0)
        {
            chair = emptyChairs[random.Next(0, emptyChairs.Length)];
        }
            return (chair != null);
    }

    private void NightHandler()
    {
        if(defenders.Count > 0)
        {
            TavernEventsManager.OnDefendersToCards(defenders);
        }
    }
   
}