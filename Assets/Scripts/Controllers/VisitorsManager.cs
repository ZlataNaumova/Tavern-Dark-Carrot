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

    private System.Random random = new System.Random();
    private Table[] tables;
    private static List<GameObject> activeVisitors = new List<GameObject>();
    private ObjectPool<GameObject> pool;
    private Coroutine spawnCoroutine;
    private Table emptyTable;

    private void OnEnable()
    {
        TavernEventsManager.OnHeartRepaired += HeartRepairedHandler;
        TavernEventsManager.OnVisitorLeftTavern += OnVisitorLeftHandler;
        TavernEventsManager.OnVisitorBecomeDefenderCard += OnVisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnNightStarted += OnNightStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnHeartRepaired -= HeartRepairedHandler;
        TavernEventsManager.OnVisitorLeftTavern -= OnVisitorLeftHandler;
        TavernEventsManager.OnVisitorBecomeDefenderCard -= OnVisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnNightStarted -= OnNightStartedHandler;
    }

    private void Start() => tables = FindObjectsOfType<Table>();

    private void Awake() => pool = new ObjectPool<GameObject>(CreateVisitor, OnGetVisitorFromPool, OnReturnVisitorToPool);

    private GameObject CreateVisitor()
    {
        var visitor = Instantiate(visitorPrefab, visitorSpawnPoint);
        visitor.SetActive(false);
        return visitor;
    }

    private void OnGetVisitorFromPool(GameObject visitor)
    {
        visitor.SetActive(true);
        visitor.GetComponent<VisitorAI>().SetStats(10, 1);
        visitor.transform.position = visitorSpawnPoint.transform.position;
    }

    private void OnReturnVisitorToPool(GameObject visitor) => visitor.SetActive(false);
    
    private void HeartRepairedHandler() => spawnCoroutine = StartCoroutine(VisitorsSpawnCoroutine());

    private bool IsSpawnNeeded() => activeVisitors.Count <= GameConfigManager.MaxVisitersQuantity;

    IEnumerator VisitorsSpawnCoroutine()
    {
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
            tempVisitor.SetStats(10, 1);
            Debug.Log(emptyTable);
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

    private void OnVisitorBecomeDefenderCardHandler(VisitorAI visitor) => pool.Release(visitor.gameObject);

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
}

public enum VisitorTargets
{
    Table,
    Door,
}