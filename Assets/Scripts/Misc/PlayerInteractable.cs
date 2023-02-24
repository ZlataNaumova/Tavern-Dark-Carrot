using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Outline))]

public abstract class PlayerInteractable : MonoBehaviour
{
    protected PlayerController player;
    protected Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
        outline.OutlineColor = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            player.SetTarget(gameObject);
            outline.OutlineWidth = 2.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetTarget(player.gameObject);
            outline.OutlineWidth = 0;
        }
    }

    public abstract void PlayerInteraction();
}
