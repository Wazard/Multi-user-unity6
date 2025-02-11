using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    public void DestroyAssemblableObjects()
    {
        
        foreach(GameObject go in spawnManager.m_AssemblableObjects) {

            PhotonNetwork.Destroy(go);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DestroyAssemblableObjects();
        }
    }
}
