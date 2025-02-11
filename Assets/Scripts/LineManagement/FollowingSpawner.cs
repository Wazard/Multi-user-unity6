using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class FollowingSpawner : MonoBehaviour
{
    //[SerializeField] SpawnManager spawnManager;
    [SerializeField] GameObject m_SpawnReference;
    [SerializeField] GameObject m_ObjectToSpawn;
    [SerializeField] SpawnManager m_SpawnManager;
    [SerializeField] List<string> alreadyDestroyedNames = new List<string>();
    

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == PhotonEvents.ASSEMBLABLE_OBJECT_DESTRUCTION)
        {
            object[] evData = (object[])obj.CustomData;
            string name = (string)evData[0];
            GameObject objectToDestroy = FindPhotonObjectToDestroy(name, m_SpawnManager.m_AssemblableObjects);
            if (!alreadyDestroyedNames.Contains(name))
            {
                //Destroys parent object
                Debug.Log($"[BOX SPAWN] ho appena spawnato la scatola per l'oggetto di nome : {(string)evData[0]}");
                
                bool isOwner = (string)evData[2] == PhotonNetwork.LocalPlayer.UserId;
                Debug.Log($"The Owner is {(string)evData[2]}, the local player is {PhotonNetwork.LocalPlayer.UserId}");
                Destroy(objectToDestroy);

                //Checks if object was fully assembled when destroyed, if it was, it instantiates a box
                //Currently if the limit is reached the spawn will be interrupted and boxes will stop spawning no matter what, 
                //but currently spawned parts will not instantly disappear and still be assembleble and destructible
                bool isFullyAssembled = (bool)evData[1];
                Debug.Log($"[BOX SPAWN] isOwner = {isOwner}, isFullyAssembled = {isFullyAssembled}, m_SpawnManager.m_BoxCounter = {m_SpawnManager.m_BoxCounter}, m_SpawnManager.m_BoxLimit = {m_SpawnManager.m_BoxLimit} ");
                if(isOwner && isFullyAssembled && m_SpawnManager.m_BoxCounter < m_SpawnManager.m_BoxLimit)
                {
                    PhotonNetwork.Instantiate($"{m_ObjectToSpawn.name}", m_SpawnReference.transform.position, m_SpawnReference.transform.rotation);
                    Debug.Log($"[BOX SPAWN] SPAWNNNNNNNN");
                    m_SpawnManager.m_BoxCounter++;
                }
                
                alreadyDestroyedNames.Add((string)evData[0]);
            }
            
        }
    }

    public GameObject FindPhotonObjectToDestroy(string name, List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }
        return null;
    }
}
