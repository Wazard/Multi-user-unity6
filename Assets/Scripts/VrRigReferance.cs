using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrRigReferance : MonoBehaviour
{
    public static VrRigReferance Singleton;
    
    public Transform root;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    private void Awake()
    {
        Singleton ??= this;
    }
    
}
