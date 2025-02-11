using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[SelectionBase]
public class SpawnManager : MonoBehaviour
{

    //#region PHOTON_EVENTS
    //public const byte OBJECT_DESTRUCTION = 3;
    //#endregion
    [Header("Object Spawn")]
    public float timeBetweenSpawns;
    public bool continueSpawning;
    //[SerializeField] private GameObject m_objectToSpawn;
    [SerializeField] private List<GameObject> m_objectsToSpawn = new List<GameObject>();
    [SerializeField] private int m_SpawnLimit;
    [SerializeField] private int m_Counter = 0;
    [SerializeField] public int m_BoxLimit;
    [SerializeField] public int m_BoxCounter = 0;
    [SerializeField] public int nameCounter = 1;
    //There is a Gameobject instead of a Vector3 and Quaternion so that if we move the assembly line around, and the reference object is a child of the line, it won't be
    //necessary to reset them. Also, the pivot of the line is in kind of an awkward position to use that as a reference.
    [SerializeField] private GameObject m_spawnReferencePoint;
    //the parent objects : motore assemblabile 
    [SerializeField] public List<GameObject> m_AssemblableObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> m_ConveyorBelts = new List<GameObject>();
    [SerializeField] public List<GameObject> m_ObjectsOnTheBelts = new List<GameObject>();
    private bool spawningAlready = false;
    RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    //These three methods handle toggling the belt movement upon event reception
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
        if (obj.Code == PhotonEvents.OBJECT_DESTRUCTION)
        {
            object[] evData = (object[])obj.CustomData;
            GameObject objectToDestroy = FindObjectToDestroy((int)evData[0], m_ObjectsOnTheBelts);
            if (objectToDestroy != null)
            {
                if (m_ObjectsOnTheBelts.Contains(objectToDestroy))
                {
                    m_ObjectsOnTheBelts.Remove(objectToDestroy);
                    Destroy(objectToDestroy);
                }

            }
            else 
            {
                objectToDestroy = FindObjectToDestroy((int)evData[0], m_AssemblableObjects);
                if (objectToDestroy != null)
                {
                    m_AssemblableObjects.Remove(objectToDestroy);
                    //PhotonNetwork.Destroy(objectToDestroy);
                    m_Counter--;
                }
                else
                {
                    Debug.LogError("[SPAWNMANAGER]L'oggetto non è stato correttamente rimosso.");
                    Debug.Log("[DESPAWN]Gli id che ho ricevuto sono : \n");
                    foreach (GameObject go in m_ObjectsOnTheBelts)
                    {
                        Debug.Log($"[DESPAWN] {go.GetInstanceID()}\n");
                    }
                }
            }

        }
        else if (obj.Code == PhotonEvents.ASSEMBLABLE_OBJECT_CREATION)
        {
            GameObject newEngine = new GameObject();
            object[] evData = (object[])obj.CustomData;
            newEngine.name = $"{evData[0].ToString()}";
            newEngine.AddComponent<AssemblableObject>();
            StartCoroutine(OperateOnChildrenAfterDelay(4f, newEngine, evData));

        }
    }

    public IEnumerator OperateOnChildrenAfterDelay(float v, GameObject parentObject, object[] data)
    {
        yield return new WaitForSeconds(v);
        GetChildrenOfAssemblable(parentObject, data);
        PopulateAssemblableObject(parentObject);
    }

    private void GetChildrenOfAssemblable(GameObject newEngine, object[] childrenID)
    {
        int ID1 = (int)childrenID[1];
        int ID2 = (int)childrenID[2];
        int ID3 = (int)childrenID[3];
        int ID4 = (int)childrenID[4];
        int ID5 = (int)childrenID[5];

        Debug.Log($"Sto cercando le parti corrette per gli ID");
        foreach (PartToBeAssembled part in GameObject.FindObjectsByType<PartToBeAssembled>(FindObjectsSortMode.None))
        {
            if (part.GetComponent<PhotonView>())
            {
                if (part.GetComponent<PhotonView>().ViewID == ID1 || part.GetComponent<PhotonView>().ViewID == ID2 || part.GetComponent<PhotonView>().ViewID == ID3 || part.GetComponent<PhotonView>().ViewID == ID4 || part.GetComponent<PhotonView>().ViewID == ID5)
                    part.transform.parent = newEngine.transform;
            }
        }
    }

    public GameObject FindObjectToDestroy(int id, List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (obj.GetInstanceID() == id)
            {
                return obj;
            }
        }
        Debug.Log("[DESTRUCTION]Oggetto non trovato");
        return null;
    }

    public IEnumerator SpawnObjects()
    {
        while (continueSpawning)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            //Currently if the limit is reached the spawn will be interrupted and boxes will stop spawning no matter what, 
            //but currently spawned parts will not instantly disappear and still be assembleble and destructible
            if (m_Counter < m_SpawnLimit && continueSpawning && m_BoxCounter < m_BoxLimit)
            {
                //GameObject newObject = (GameObject)Instantiate(m_objectToSpawn, m_spawnReferencePoint.transform.position, m_spawnReferencePoint.transform.rotation);
                //m_AssemblableObjects.Add(newObject);
                GameObject newEngine = new GameObject();
                newEngine.name = $"Parent Object {nameCounter}";
                nameCounter++;
                newEngine.AddComponent<AssemblableObject>();
                object[] eventData = new object[] { newEngine.name, 0, 0, 0, 0, 0 };
                int c = 0;
                
                
                foreach (GameObject m_objectToSpawn in m_objectsToSpawn)
                {
                    c++;
                    yield return new WaitForSeconds(0.8f);
                    GameObject newObject = WrapPhotonInstantiation(m_objectToSpawn);
                    newObject.transform.parent = newEngine.transform;
                    eventData[c] = newObject.GetComponent<PhotonView>().ViewID;
                }
                yield return new WaitForSeconds(0.5f);
                PhotonNetwork.RaiseEvent(PhotonEvents.ASSEMBLABLE_OBJECT_CREATION, eventData, RaiseEventOptions.Default, SendOptions.SendReliable);

                PopulateAssemblableObject(newEngine);
                m_AssemblableObjects.Add(newEngine);
                m_Counter++;

                for(int i = 0; i < newEngine.transform.childCount; i++)
                {
                    m_ObjectsOnTheBelts.Add(newEngine.transform.GetChild(i).gameObject);
                }
            }

        }
    }

    private GameObject WrapPhotonInstantiation(GameObject m_objectToSpawn)
    {
        GameObject newObject = PhotonNetwork.Instantiate($"{m_objectToSpawn.name}", m_spawnReferencePoint.transform.position, m_spawnReferencePoint.transform.rotation);
        if (newObject.GetComponent<PhotonView>().AmOwner)
        {
            Debug.Log($"I am the owner of this object {newObject.GetComponent<PhotonView>().Owner}, the local player is {PhotonNetwork.LocalPlayer}");
        }
        return newObject;
    }

    private void PopulateAssemblableObject(GameObject objectToPopulate)
    {
        AssemblableObject assemblableObject = objectToPopulate.GetComponent<AssemblableObject>();
        assemblableObject.m_FirstPropeller = objectToPopulate.transform.Find("Elica1(Clone)").GetComponent<PartToBeAssembled>();
        assemblableObject.m_SecondPropeller = objectToPopulate.transform.Find("Elica2(Clone)").GetComponent<PartToBeAssembled>();
        assemblableObject.m_ThirdPropeller = objectToPopulate.transform.Find("Elica3(Clone)").GetComponent<PartToBeAssembled>();
        assemblableObject.m_Support = objectToPopulate.transform.Find("Sostegno(Clone)").GetComponent<PartToBeAssembled>();
    }

    public void StartSpawning()
    {
        continueSpawning = true;
        if (!spawningAlready)
            StartCoroutine(SpawnObjects());
        spawningAlready = true;
        Debug.Log("Spawn start");
    }

    public void StopSpawning()
    {
        continueSpawning = false;
        spawningAlready = false;
        Debug.Log("[SPWAN MANAGER]Stop");
    }

    private void Update()
    {
        try
        {
            foreach (GameObject Object in m_AssemblableObjects)
            {
                if (Object == null)
                {
                    m_AssemblableObjects.Remove(Object);
                    m_Counter--;
                }
            }
            foreach (GameObject Object in m_ObjectsOnTheBelts)
            {
                if (Object == null)
                {
                    m_ObjectsOnTheBelts.Remove(Object);
                }
            }
        }
        catch(InvalidOperationException)
        {
            Debug.Log("trying to remove ad unremovable object");
        }

    }
}
