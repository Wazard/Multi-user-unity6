using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public class PartToBeAssembled : MonoBehaviour
{
    #region PHOTON_EVENTS
    public const byte OBJECT_DESTRUCTION = 3;
    #endregion
    //[SerializeField] AttachementPoint attachmentPoint;
    XRGrabNetworkInteractable interactable;
    public bool isAssembled;
    [HideInInspector] public GameObject startingParent;
    public UnityEvent onDestruction;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ConveyorBelt"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("ConveyorBelt"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndOfLine"))
        {
            //PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            //Destroy(gameObject);
            int ID = gameObject.GetInstanceID();
            //Debug.Log($"[DESPAWN] Sto triggerando {other.gameObject.name}");
            //Debug.Log($"[DESPAWN] l'id che sto inviando è : {gameObject.GetInstanceID()}");
            object[] eventData = new object[] {ID};
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvents.OBJECT_DESTRUCTION, eventData, options, SendOptions.SendReliable);
        }
    }

    private void Awake()
    {
        StartCoroutine(AddParentAfterDelay());
    }

    public IEnumerator AddParentAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
        if(gameObject.transform.parent is not null)
            startingParent = gameObject.transform.parent.gameObject;
    }

    private void OnDestroy()
    {
        Debug.Log($"The owner is {gameObject.GetComponent<PhotonView>().AmOwner}");

        if (gameObject.transform.parent is not null && gameObject.transform.parent.name.Contains("Rotore"))
            transform.parent.parent.gameObject.GetComponent<AssemblableObject>().ChildDestroyed(PhotonNetwork.LocalPlayer.UserId);
        else if(gameObject.transform.parent.name.Contains("Parent"))
            transform.parent.gameObject.GetComponent<AssemblableObject>().ChildDestroyed(PhotonNetwork.LocalPlayer.UserId);
    }
}
