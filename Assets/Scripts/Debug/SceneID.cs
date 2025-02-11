using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneID : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] int PhotonID;
    // Start is called before the first frame update
    void Start()
    {
        ID = gameObject.GetInstanceID();
        PhotonID = gameObject.GetComponent<PhotonView>().ViewID;
    }

}
