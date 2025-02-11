using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Data.SqlTypes;

[SelectionBase]
public class AttachementPoint : MonoBehaviour
{
    #region Events
    /*public delegate void AttachmentCompleted(GameObject attachmentPoint, GameObject assembledObject);
    public static event AttachmentCompleted hasBeenCompleted;
    public delegate void SeparationCompleted(GameObject attachmentPoint, GameObject separatedObject);
    public static event SeparationCompleted hasBeenSeparated;
    */
    #endregion
    //#region PhotonEvents
    //private const byte OBJECT_ASSEMBLED_EVENT = 1;
    //private const byte OBJECT_DISASSEMBLED_EVENT = 2;
    //#endregion

    [SerializeField]
    public List<GameObject> m_CompatibleObjects = new List<GameObject>();
    [SerializeField]
    private bool m_attach = false;
    [SerializeField]
    private Vector3 m_PositionOffset;
    [SerializeField]
    private Vector3 m_RotationOffset;
    /*[SerializeField]
    private bool notAttachedYet = true;
    */
    [SerializeField]
    private GameObject installationPoint;
    [SerializeField]
    private List<GameObject> snappingPoints;
    [SerializeField]
    private List<GameObject> containers;
    [SerializeField]
    private Material m_DefaultMaterial;
    [SerializeField]
    private Material m_VisibleMaterial;
    private GameObject m_AssembledObject;
    RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    //private bool otherHandIsGrabbingParent;
    UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor grabbingHand;

    private void Start()
    {
        m_DefaultMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
        StartCoroutine(PopulateReferencesAfterDelay());
    }

    public IEnumerator PopulateReferencesAfterDelay()
    {
        yield return new WaitForSeconds(8f);
        if (gameObject.name.Contains("Elica"))
        {
            int counter;
            for(counter = 0; counter < transform.parent.parent.childCount; counter++)
            {
                if (transform.parent.parent.GetChild(counter).gameObject.name.Contains("Elica"))
                {
                    m_CompatibleObjects.Add(transform.parent.parent.GetChild(counter).gameObject);
                    containers.Add(transform.parent.parent.GetChild(counter).gameObject);
                    snappingPoints.Add(transform.parent.parent.GetChild(counter).GetChild(0).gameObject);
                }
            }
        }
        else
        {
            int counter;
            for (counter = 0; counter < transform.parent.parent.childCount; counter++)
            {
                if (transform.parent.parent.GetChild(counter).gameObject.name.Contains("Sostegno"))
                {
                    m_CompatibleObjects.Add(transform.parent.parent.GetChild(counter).gameObject);
                    containers.Add(transform.parent.parent.GetChild(counter).gameObject);
                    snappingPoints.Add(transform.parent.parent.GetChild(counter).GetChild(0).gameObject);
                }
            }
        }
    }

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
        if (obj.Code == PhotonEvents.OBJECT_ASSEMBLED_EVENT)
        {
            //Debug.Log("[WOBBLING] Evento ricevuto");
            object[] evData = (object[])obj.CustomData;
            
            GameObject attachmentPoint = FindCorrectAttachmentPoint((int)evData[0], (string)evData[1]);
            GameObject objectToAssemble = FindCorrectPart((int)evData[2]);
            Debug.Log($"[WOBBLING] attachment point = {attachmentPoint.name}");
            Debug.Log($"[WOBBLING] part = {objectToAssemble.name}");
            if (attachmentPoint == gameObject && objectToAssemble.GetComponent<PartToBeAssembled>().isAssembled == false)
            {
                Debug.Log("[WOBBLING] Funzione invocata");
                AssembleToPoint(attachmentPoint, objectToAssemble);
            }
            m_attach = false;
        }
        //else if (obj.Code == PhotonEvents.OBJECT_DISASSEMBLED_EVENT)
        //{
        //    object[] evData = (object[])obj.CustomData;
        //    GameObject attachmentPoint = FindCorrectAttachmentPoint((int)evData[0]);
        //    GameObject objectToDisassemble = FindCorrectPart((int)evData[1]);
        //    if (attachmentPoint == gameObject)
        //    {
        //        DisassembleFromPoint(attachmentPoint, objectToDisassemble);
        //    }
        //}
        else if (obj.Code == PhotonEvents.OBJECT_TAKEN_IN_HAND)
        {
            object[] evData = (object[])obj.CustomData;
            //GameObject interactor = GameObject.Find($"{evData[2]}");
            GameObject objectTaken = FindCorrectPart((int)evData[0]);
            //Debug.Log($"L'oggetto che voglio montare � : {objectTaken}");
            //objectTaken.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //objectTaken.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //objectTaken.GetComponent<Rigidbody>().isKinematic = true;
            //objectTaken.GetComponent<Rigidbody>().useGravity = false;
            //objectTaken.transform.parent = interactor.transform;
            //objectTaken.transform.localPosition = Vector3.zero;
            TryChangeMaterials(objectTaken, m_VisibleMaterial);
        }
    }

    private GameObject FindCorrectPart(int instanceID)
    {
        Debug.Log($"[WOBBLING] Sto cercando la parte corretta per la parte con ID {instanceID}");
        foreach (PartToBeAssembled part in GameObject.FindObjectsByType<PartToBeAssembled>(FindObjectsSortMode.None))
        {
            if (part.GetComponent<PhotonView>().ViewID == instanceID)
            {
                Debug.Log("[WOBBLING] Parte trovata");
                return part.gameObject;
            }
            else
            {
                Debug.Log($"L'ID non era {instanceID} ma {part.GetComponent<PhotonView>().ViewID}");
            }
        }

        Debug.Log("[WOBBLING] Parte non trovata");
        return null;
    }

    private GameObject FindCorrectAttachmentPoint(int instanceId, string name)
    {
        Debug.Log($"[WOBBLING] Sto cercando l'attachment point corretto per l' ID {instanceId}");
        foreach (AttachementPoint attachmentPoint in GameObject.FindObjectsByType<AttachementPoint>(FindObjectsSortMode.None))
        {
            if (attachmentPoint.transform.parent.gameObject.GetComponent<PhotonView>().ViewID == instanceId && attachmentPoint.gameObject.name == name)
            {
                Debug.Log("[WOBBLING] Attachment point trovato");
                return attachmentPoint.gameObject;
            }
                
        }
        Debug.Log("[WOBBLING] Attachment point non trovato");
        return null;
    }

    private void AssembleToPoint(GameObject attachmentPoint, GameObject assembledObject)
    {
        //Debug.Log("Puoi Provare a rilasciare l'oggetto");
        Debug.Log("[WOBBLING] Funzione iniziata");
        attachmentPoint.transform.parent.GetComponent<PhotonView>().RequestOwnership();
        assembledObject.GetComponent<PhotonView>().RequestOwnership();
        assembledObject.GetComponent<Rigidbody>().useGravity = false;
        assembledObject.GetComponent<Rigidbody>().isKinematic = true;
        assembledObject.transform.SetParent(gameObject.transform.parent, true);
        //other.transform.localPosition = Vector3.zero + m_PositionOffset;
        GameObject snappingPoint = DetermineSnappingPoint(snappingPoints, assembledObject);
        GameObject container = DetermineContainer(assembledObject);
        container.transform.localRotation = Quaternion.Euler(m_RotationOffset);
        if (snappingPoint != null)
            SnappingMethods.SnapTwoObjects(installationPoint, snappingPoint);
        else
            Debug.LogError("Non sono riuscito a trovare uno snapping point compatibile");

        assembledObject.GetComponent<PartToBeAssembled>().isAssembled = true;
        m_AssembledObject = assembledObject;
        //Debug.Log("L'oggetto � stato attaccato");
        if (assembledObject.GetComponent<CapsuleCollider>())
        {
            assembledObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        if (assembledObject.GetComponent<BoxCollider>())
        {
            assembledObject.GetComponent<BoxCollider>().enabled = false;
        }
        TryChangeMaterials(assembledObject.transform.gameObject, m_DefaultMaterial);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
        assembledObject.GetComponent<XRGrabNetworkInteractable>().enabled = false;
        
        //if (assembledObject.GetComponent<CapsuleCollider>())
        //{
        //    assembledObject.GetComponent<CapsuleCollider>().enabled = false;
        //}
        //if (assembledObject.GetComponent<BoxCollider>())
        //{
        //    assembledObject.GetComponent<BoxCollider>().enabled = false;
        //}
        //Destroy(assembledObject.GetComponent<PhotonRigidbodyView>());
        
        Debug.Log("[WOBBLING] Funzione conclusa");
    }

    private GameObject DetermineContainer(GameObject assembledObject)
    {
        foreach (GameObject container in containers)
        {
            if (assembledObject == container)
            {
                Debug.Log("[WOBBLING] Container determinato con successo");
                return container;
            }
        }
        Debug.Log("[WOBBLING] Container non determinato");
        return null;
    }

    private GameObject DetermineSnappingPoint(List<GameObject> snappingPoints, GameObject assembledObject)
    {
        foreach(GameObject snapPoint in snappingPoints)
        {
            if(snapPoint == assembledObject.transform.GetChild(0).gameObject)
            {
                Debug.Log("[WOBBLING] Snapping Point determinato con successo");
                return snapPoint;
            }
        }
        Debug.Log("[WOBBLING] Snapping Point non determinato");
        return null;
    }

    private void DisassembleFromPoint(GameObject attachmentPoint, GameObject disassembledObject)
    {
        //disassembledObject.GetComponent<Rigidbody>().useGravity = true;
        //disassembledObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        //disassembledObject.transform.SetParent(disassembledObject.GetComponent<PartToBeAssembled>().startingParent.transform, true);
    }


    private void TryChangeMaterials(GameObject grabbedObject, Material newMat)
    {
        Debug.Log($"Sono il metodo TryMakeYourselfMoreVisible dell'oggetto {gameObject}");

        if (m_CompatibleObjects.Contains(grabbedObject))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = newMat;
        }
    }
        private void OnTriggerStay(Collider other)
        {
        //Debug.Log($"Sono in Ontriggerstay notAttachedYet � :  {notAttachedYet} m_attach � {m_attach}");
            if (m_attach)
            {
                Debug.Log($"primo controllo = {m_CompatibleObjects.Contains(other.gameObject)}, secondo controllo = {other.gameObject.GetComponent<Rigidbody>() != null}");
                if (m_CompatibleObjects.Contains(other.gameObject) && other.gameObject.GetComponent<Rigidbody>() != null)
                {
                    Debug.Log("Sto per inviare l'evento");
                    //other.gameObject.GetComponent<PartToBeAssembled>().isAssembled = true;
                    object[] eventData = new object[]{gameObject.transform.parent.GetComponent<PhotonView>().ViewID, gameObject.name, other.GetComponent<PhotonView>().ViewID, other.gameObject.name};
                    PhotonNetwork.RaiseEvent(PhotonEvents.OBJECT_ASSEMBLED_EVENT, eventData, options, SendOptions.SendReliable);
                    Debug.Log("[WOBBLING] Evento inviato");
                }
            }
        }
    public void NewObjectReleased(SelectExitEventArgs args)
    {
        Debug.Log("[EVENTI] OGGETTO RILASCIATO");   
        if(!m_attach)
            TryAttach();
    }

    public void NewObjectTakenBack(SelectEnterEventArgs args)
    {
        Debug.Log("[EVENTI] OGGETTO PRESO");
        object[] eventData = new object[] {args.interactableObject.transform.gameObject.GetComponent<PartToBeAssembled>().startingParent.GetComponent<PhotonView>().ViewID, args.interactableObject.transform.gameObject.name, args.interactorObject.transform.name};
        PhotonNetwork.RaiseEvent(PhotonEvents.OBJECT_TAKEN_IN_HAND, eventData, options, SendOptions.SendReliable);

        //if (args.interactableObject.transform.gameObject == gameObject)
        //{
        //    TrySeparate(args.interactableObject.transform.gameObject);
        //}
    }
    public void TryAttach()
    {
        m_attach = true;
    }


    public void TrySeparate(GameObject objectToSeparate)
    {
        //if(m_AssembledObject != null)
        //{
        //    if (m_CompatibleObjects.Contains(objectToSeparate) && objectToSeparate == m_AssembledObject && objectToSeparate.GetComponent<PartToBeAssembled>().isAssembled)
        //    {
        //        objectToSeparate.GetComponent<PartToBeAssembled>().isAssembled = false;
        //        objectToSeparate.transform.SetParent(objectToSeparate.GetComponent<PartToBeAssembled>().startingParent.transform);
        //        object[] eventData = new object[] {gameObject.GetInstanceID(), objectToSeparate.GetInstanceID()};
        //        PhotonNetwork.RaiseEvent(PhotonEvents.OBJECT_DISASSEMBLED_EVENT, eventData, options, SendOptions.SendReliable);
        //        //hasBeenSeparated(gameObject, lastAssembledObject);
        //    }
        //}
    }

}
