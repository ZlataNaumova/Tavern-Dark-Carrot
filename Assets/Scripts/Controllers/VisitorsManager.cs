using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorsManager : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private Transform visitorSpawnPoint;
    [SerializeField] private int visitorsSpawnQuantity;

    private void OnEnable()
    {
        TavernEventsManager.HeartRepaired += StartVisitersSpawn;
    }


    private void OnDisable()
    {
        TavernEventsManager.HeartRepaired -= StartVisitersSpawn;
    }

    private void StartVisitersSpawn()
    {
        StartCoroutine(SpawnDelayCoroutine());
    }

  

    IEnumerator SpawnDelayCoroutine()
    {
        for (int i = 0; i < visitorsSpawnQuantity; ++i)
        {
            TrySpawnVisitor();
            yield return new WaitForSeconds(visitorsSpawnQuantity);
        }


    }

    private void TrySpawnVisitor()
    {
        Chair emptyChair;
        if (TryGetEmptyChair(out emptyChair))
        {
            emptyChair.isEmpty = false;
            GameObject visitor = Instantiate(visitorPrefab, visitorSpawnPoint);
            visitor.GetComponent<VisitorAI>().SetTarget(emptyChair.transform);
        }
        else
        {
            Debug.Log("No empty hire places");
        }

    }

    private bool TryGetEmptyChair(out Chair chair)
    {
            chair = Array.Find(FindObjectsOfType<Chair>(), chair => chair.isEmpty == true);
            return (chair != null);
    }
}