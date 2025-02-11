using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[SelectionBase]
public class ConveyorBelt : MonoBehaviour
{
    //#region PHOTON_EVENTS
    //public const byte BELT_TOGGLE_EVENT = 4;
    //public const byte OBJECT_DESTRUCTION = 3;
    //#endregion
    [Header("ObjectMovement")]
    public static float m_BeltSpeed = .01f;
    [SerializeField] public bool m_BeltIsActive;
    //The parts to be assembled : elica, sostegno etc
    [SerializeField] private List<GameObject> m_ObjectsOnTheBelt = new List<GameObject>();
    
    //private bool spawningAlready = false;
    
    RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    private bool m_IsBeltActive;
    [SerializeField] private SpawnManager m_SpawnManager;
    
    [SerializeField] private GameObject m_startOfBelt;
    [SerializeField] private GameObject m_endOfBelt;
    

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
        if (obj.Code == PhotonEvents.BELT_TOGGLE_EVENT)
        {
            object[] evData = (object[])obj.CustomData;
            m_BeltIsActive = (bool)evData[0];
        }
        if (obj.Code == PhotonEvents.OBJECT_DESTRUCTION)
        {
            object[] evData = (object[])obj.CustomData;
            GameObject objectToDestroy = m_SpawnManager?.FindObjectToDestroy((int)evData[0], m_ObjectsOnTheBelt);
            if (objectToDestroy != null)
            {
                if(m_ObjectsOnTheBelt.Contains(objectToDestroy))
                    m_ObjectsOnTheBelt.Remove(objectToDestroy);
            }
            else
            {
                Debug.LogWarning("[BELT]L'oggetto non è stato trovato.");
            }
        }
    }

    //These three add an object to the list of objects to move if it is on the belt
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AssemblableObject") || other.gameObject.CompareTag("Movable"))
        {
            if (!m_ObjectsOnTheBelt.Contains(other.gameObject))
                m_ObjectsOnTheBelt.Add(other.gameObject);

            if (!m_SpawnManager.m_ObjectsOnTheBelts.Contains(other.gameObject))
                m_SpawnManager.m_ObjectsOnTheBelts.Add(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AssemblableObject") || collision.gameObject.CompareTag("Movable"))
        {
            if (!m_ObjectsOnTheBelt.Contains(collision.gameObject))
                m_ObjectsOnTheBelt.Add(collision.gameObject);

            if (!m_SpawnManager.m_ObjectsOnTheBelts.Contains(collision.gameObject))
                m_SpawnManager.m_ObjectsOnTheBelts.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("AssemblableObject") || collision.gameObject.CompareTag("Movable"))
        {
            if (m_ObjectsOnTheBelt.Contains(collision.gameObject))
                m_ObjectsOnTheBelt.Remove(collision.gameObject);
        }
    }
    //Moving objects on the line and removing null(despawned) objects
    //Movement works by taking the start and end points of this piece of conveyor belt, calculating the vector between them, normalizing it, and
    //adding it to the transform of each object to be moved, multiplied by a beltspeed variable.
    private void Update()
    {
        if (m_BeltIsActive)
        {
            try
            {
                foreach (GameObject Object in m_ObjectsOnTheBelt)
                {

                    if (Object == null)
                    {
                        m_ObjectsOnTheBelt.Remove(Object);
                    }

                    bool ObjectIsAligned = false;
                    ////Bringing the object to the center of the piece of belt before changing direction
                    if (Math.Abs(Object.transform.position.x - GetClosestPointOnFiniteLine(Object.transform.position, m_endOfBelt.transform.position, m_startOfBelt.transform.position).x) > 0.01f || Math.Abs(Object.transform.position.z - GetClosestPointOnFiniteLine(Object.transform.position, m_endOfBelt.transform.position, m_startOfBelt.transform.position).z) > 0.01f)
                    {
                        //Vector3 MT = Vector3.MoveTowards(Object.transform.position, GetClosestPointOnFiniteLine(Object.transform.position, m_endOfBelt.transform.position, m_startOfBelt.transform.position), m_BeltSpeed);
                        //Object.transform.position = new Vector3(MT.x, Object.transform.position.y, MT.y);
                        if(Object.GetComponent<PartToBeAssembled>().isAssembled == false)
                        {
                            Vector3 target = Object.transform.position + ((GetClosestPointOnFiniteLine(Object.transform.position, m_endOfBelt.transform.position, m_startOfBelt.transform.position) - Object.transform.position).normalized * m_BeltSpeed);
                            Object.transform.position = new Vector3(target.x, Object.transform.position.y, target.z);
                        }
                    }
                    else
                    {
                        ObjectIsAligned = true;
                    }

                    if (Object != null && m_BeltIsActive && ObjectIsAligned)
                    {
                        Object.transform.position = Object.transform.position + (m_endOfBelt.transform.position - m_startOfBelt.transform.position).normalized * m_BeltSpeed;
                        //Object.transform.position = new Vector3(Object.transform.position.x - m_BeltSpeed, Object.transform.position.y, Object.transform.position.z);
                    }

                }

            }
            catch (InvalidOperationException)
            {

            }
        }

    }



    public void StartConveyorBelt()
    {
        m_IsBeltActive = true;
        object[] eventData = new object[] { m_IsBeltActive };
        PhotonNetwork.RaiseEvent(PhotonEvents.BELT_TOGGLE_EVENT, eventData, options, SendOptions.SendReliable);
        Debug.Log("Start");
    }

    public void StopConveyorBelt()
    {
        m_IsBeltActive = false;
        object[] eventData = new object[] { m_IsBeltActive };
        PhotonNetwork.RaiseEvent(PhotonEvents.BELT_TOGGLE_EVENT, eventData, options, SendOptions.SendReliable);
        Debug.Log("Stop");
    }

    Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
    {
        Vector3 line_direction = line_end - line_start;
        float line_length = line_direction.magnitude;
        line_direction.Normalize();
        float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
        return line_start + line_direction * project_length;
    }

    /*
    public void IncreaseSpeed()
    {
        m_BeltSpeed = (float)(m_BeltSpeed + 0.001);
    }
    public void DecreaseSpeed()
    {
        m_BeltSpeed = (float)(m_BeltSpeed - 0.001);
    }
    */
}
