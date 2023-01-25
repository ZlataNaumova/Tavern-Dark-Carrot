using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterractionalCollider : MonoBehaviour
{
    [SerializeField] private GameObject defenderOnHirePointPanel;
    [SerializeField] private GameObject defenderOnTablePanel;

    private bool isDefenderHired = false;
    

    public void SetIsDefenderHired(bool value)
    {
        isDefenderHired = value;
    }
    public void SetAllPanelsInactive()
    {
        defenderOnHirePointPanel.SetActive(false);
        defenderOnTablePanel.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isDefenderHired)
            {
                defenderOnHirePointPanel.SetActive(true);
            }
            else
            {
                defenderOnTablePanel.SetActive(true);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isDefenderHired)
            {
                defenderOnHirePointPanel.SetActive(false);
            }
            else
            {
                defenderOnTablePanel.SetActive(false);
            }
        }
    }
}
