using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirePoint : MonoBehaviour
{
    public bool isEmpty = true;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            other.GetComponent<Defender>().DefenderIsOnHiringPoint();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            isEmpty = true;
        }
    }
}
