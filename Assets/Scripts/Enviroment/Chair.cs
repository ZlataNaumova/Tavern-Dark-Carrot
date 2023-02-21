using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool isEmpty = true;
    private VisitorAI visitor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            visitor = other.GetComponent<VisitorAI>();
            if (!visitor.isLeaving)
            {
                visitor.VisitorSits();
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Visitor"))
    //    {
    //        if (visitor.isLeaving)
    //        {
    //            isEmpty = true;
    //        }
               
    //    }
    //}
}
