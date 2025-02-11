using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SnappingMethods
{
    public static void SnapTwoObjects(GameObject objectToSnapTo, GameObject ObjectToSnap)
    {
        if (objectToSnapTo.CompareTag("RefPoint") && ObjectToSnap.CompareTag("RefPoint"))
        {
            Debug.Log($"SNAP!");
            Vector3 Difference = objectToSnapTo.transform.position - ObjectToSnap.transform.position;
            ObjectToSnap.transform.parent.transform.position += Difference;
            //GameObject destinationSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //destinationSphere.GetComponent<SphereCollider>().enabled = false;
            //Vector3 objPos = ObjectToSnap.transform.parent.transform.position;
            //destinationSphere.transform.position = objPos + Difference;
            //destinationSphere.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            //Debug.DrawLine(objPos, destinationSphere.transform.position);
            //GameObject startSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //startSphere.GetComponent<SphereCollider>().enabled = false;
            //startSphere.transform.position = objPos;
            //startSphere.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        }
        else
        {
            Debug.LogError("Gli oggetti selezionati non sono dei punti di riferimento");
        }

    }
}
