using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public abstract class PlayerInterractible : MonoBehaviour
{
    protected PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            player.SetTarget(gameObject);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetTarget(player.gameObject);
        }
          }


    public abstract void PlayerInterraction();
}
