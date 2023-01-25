using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernHeart : MonoBehaviour
{
    [SerializeField] private GameObject heartPanel;
    [SerializeField] private Material damaged;
    [SerializeField] private Material notDamaged;

    private bool isHeartDamaged = true;

    private void Start()
    {
        heartPanel.SetActive(false);
        gameObject.GetComponent<Renderer>().material = damaged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHeartDamaged && other.CompareTag("Player"))
        {
            heartPanel.SetActive(true);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (isHeartDamaged && other.CompareTag("Player"))
        {
            heartPanel.SetActive(false);
        }
        
    }

    public void HeartRepair()
    {
        heartPanel.SetActive(false);
        isHeartDamaged = false;
        TavernEventsManager.OnHeartRepaired();
        gameObject.GetComponent<Renderer>().material = notDamaged;
    }
}
