using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool isEmpty = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            Debug.Log("visiter Reached chair");
            other.GetComponent<VisitorAI>().VisitorSits();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            isEmpty = true;
        }
    }
}
