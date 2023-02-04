using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDoor : MonoBehaviour
{
    private VisitorAI visitor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            visitor = other.GetComponent<VisitorAI>();
            if (visitor.isLeaving)
            {
                visitor.ReachedDoor();
            }
            
        }
    }

    
}
