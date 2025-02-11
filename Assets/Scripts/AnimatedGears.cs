using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]private List<GameObject> Ingranaggi = new List<GameObject>();
    private bool isRotating = false;
    //[SerializeField]private LineManager lineManager;
    [SerializeField] private ConveyorBelt conveyorBelt;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isRotating = !isRotating;
        }

        if (conveyorBelt.m_BeltIsActive)
        {
            for (int i = 0; i < Ingranaggi.Count; i++)
            {
                float rotation = (i % 2 == 0) ? 0.4f : -0.4f;
                Ingranaggi[i].transform.Rotate(0, 0, rotation);
            }
        }
    }
}
