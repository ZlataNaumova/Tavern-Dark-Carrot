using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class VisitorsManager : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private Transform visitorSpawnPoint;
    [SerializeField] private int visitorsSpawnQuantity;
    [SerializeField] private Collider PlayersCollider;


    private System.Random random = new System.Random();
    private Table[] tables;
    private static List<GameObject> activeVisitors = new List<GameObject>();
    private ObjectPool<GameObject> pool;
    private Coroutine spawnCoroutine;
    private Coroutine randomVisitersLeaveCoroutine;
    private Table emptyTable;
    private bool isHappinessLevelLow;

    private void OnEnable()
    {
        TavernEventsManager.OnHeartRepaired += HeartRepairedHandler;
        TavernEventsManager.OnVisitorLeftTavern += OnVisitorLeftHandler;
        TavernEventsManager.OnVisitorBecomeDefenderCard += OnVisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnNightStarted += OnNightStartedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessLevelHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnHeartRepaired -= HeartRepairedHandler;
        TavernEventsManager.OnVisitorLeftTavern -= OnVisitorLeftHandler;
        TavernEventsManager.OnVisitorBecomeDefenderCard -= OnVisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnNightStarted -= OnNightStartedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessLevelHandler;
    }

    private void Start() => tables = FindObjectsOfType<Table>();

    private void Awake() => pool = new ObjectPool<GameObject>(CreateVisitor, OnGetVisitorFromPool, OnReturnVisitorToPool);

    private GameObject CreateVisitor()
    {
        var visitor = Instantiate(visitorPrefab, visitorSpawnPoint);
        visitor.SetActive(false);
        Physics.IgnoreCollision(PlayersCollider, visitor.GetComponent<CharacterController>());
        return visitor;
    }

    private void OnGetVisitorFromPool(GameObject visitor)
    {
        visitor.SetActive(true);
        visitor.GetComponent<VisitorAI>().SetStats(10, GetRandomVisiterType());
    }

    private void OnReturnVisitorToPool(GameObject visitor) => visitor.SetActive(false);

    private void HeartRepairedHandler() => spawnCoroutine = StartCoroutine(VisitorsSpawnCoroutine());

    private bool IsSpawnNeeded() => activeVisitors.Count <= GameConfigManager.MaxVisitersQuantity;

    IEnumerator VisitorsSpawnCoroutine()
    {
        print("visitersSpawn");
        yield return new WaitForSeconds(random.Next(GameConfigManager.VisitorsSpawnDelayMin, GameConfigManager.VisitorsSpawnDelayMax));
        TrySpawnVisitor();
    }

    private void TrySpawnVisitor()
    {
        VisitorAI tempVisitor;
        if (TryGetEmptyTable(out emptyTable))
        {
            GameObject visitor = pool.Get();
            activeVisitors.Add(visitor);
            tempVisitor = visitor.GetComponent<VisitorAI>();
            tempVisitor.transform.position = visitorSpawnPoint.transform.position;
            tempVisitor.SetStats(10, GetRandomVisiterType());
            tempVisitor.SetTarget(emptyTable.VisitorTargetPoint, VisitorTargets.Table);
            emptyTable.SetVisitor(tempVisitor);
            if (IsSpawnNeeded())
            {
                spawnCoroutine = StartCoroutine(VisitorsSpawnCoroutine());
            }
        }
        else
        {
            Debug.Log("No empty chairs");
        }
    }

    private bool TryGetEmptyTable(out Table emptyTable)
    {
        emptyTable = tables.Where(table => table.IsEmpty).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        return (emptyTable != null);
    }

    private void OnVisitorLeftHandler(GameObject visitor)
    {
        activeVisitors.Remove(visitor);
        pool.Release(visitor);
        if (IsSpawnNeeded())
        {
            spawnCoroutine = StartCoroutine(VisitorsSpawnCoroutine());
        }
    }

    private void OnVisitorBecomeDefenderCardHandler(VisitorAI visitor)
    {
        activeVisitors.Remove(visitor.gameObject);
        pool.Release(visitor.gameObject);
    }

    private void OnNightStartedHandler()
    {

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        foreach (Table table in tables)
        {
            table.ClearVisitor();
        }
        foreach (GameObject visitor in activeVisitors)
        {
            pool.Release(visitor);
        }
        activeVisitors.Clear();
    }

    private VisitorType GetRandomVisiterType() => random.NextDouble() >= 0.5 ? VisitorType.VisitorType1 : VisitorType.VisitorType2;

    private void HappinessLevelHandler(int currentHappiness)
    {

        bool wasHappinesLevelLow = isHappinessLevelLow;
        isHappinessLevelLow = currentHappiness <= GameConfigManager.VisitorHappinessLevelToLeave;
        if (wasHappinesLevelLow != isHappinessLevelLow)
        {
            if (isHappinessLevelLow)
            {
                randomVisitersLeaveCoroutine = StartCoroutine(RandomVisitersLeaveCoroutine());
            }
            else
            {
                if(randomVisitersLeaveCoroutine != null)
                {
                    StopCoroutine(randomVisitersLeaveCoroutine);
                }
            }
        }
    }

    private IEnumerator RandomVisitersLeaveCoroutine()
    {
        while (activeVisitors.Count > 0)
        {
            RandomVisiterLeave();
            yield return new WaitForSeconds(3);
        }
    }

    private void RandomVisiterLeave()
    {
         activeVisitors.OrderBy(x => Guid.NewGuid()).FirstOrDefault().GetComponent<VisitorAI>().OccupiedTable.VisiterGoingOut();
    }

}



public enum VisitorTargets
{
    Table,
    Door,
}