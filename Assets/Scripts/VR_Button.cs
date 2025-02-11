using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_Button : MonoBehaviour
{
    public bool isDeadTimeActive;

    public UnityEvent onPressed, onReleased;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button") && !isDeadTimeActive)
        {
            isDeadTimeActive = true;
            onPressed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            onReleased?.Invoke();
            isDeadTimeActive = false;
        }
    }

}
