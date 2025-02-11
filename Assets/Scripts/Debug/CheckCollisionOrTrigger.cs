using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionOrTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("AssemblableObject"))
            Debug.Log("[DEBUG]Sono in OnCollisionEnter");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("AssemblableObject"))
            Debug.Log($"[DEBUG]Sono in OnTriggerEnter con {other.gameObject.name}");
    }
}
