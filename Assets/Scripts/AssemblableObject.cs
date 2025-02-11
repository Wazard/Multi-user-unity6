using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class AssemblableObject : MonoBehaviour
{
    RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    [SerializeField] public PartToBeAssembled m_FirstPropeller;
    [SerializeField] public PartToBeAssembled m_SecondPropeller;
    [SerializeField] public PartToBeAssembled m_ThirdPropeller;
    [SerializeField] public PartToBeAssembled m_Support;
    bool isFullyAssembled = false;

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
        //Checks each time 
        if (obj.Code == PhotonEvents.OBJECT_ASSEMBLED_EVENT)
        {
            if (CheckIfFullyAssembled())
            {
                isFullyAssembled = true;
            }
            else
            {
                isFullyAssembled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConveyorBelt"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void ChildDestroyed(string Owner)
    {
        Debug.Log("[DESTRUCTION]");
        object[] eventData = new object[] {gameObject.name, isFullyAssembled, Owner};
        Debug.Log($"am i the owner of the currently destroyed child ? {Owner}");
        //ASSEMBLABLE_OBJECT_DESTRUCTION is the event that spawn the box once FollowingSpawner.cs receives it
        //Objects that are not fully assembled will still disappear, its just that the box will not spawn in their place
        PhotonNetwork.RaiseEvent(PhotonEvents.ASSEMBLABLE_OBJECT_DESTRUCTION, eventData, options, SendOptions.SendReliable);
    }

    //Rotor is not checked because the other part get attached to it, but it does not become attached itself, for now
    public bool CheckIfFullyAssembled()
    {
        if (m_FirstPropeller.isAssembled && m_SecondPropeller.isAssembled && m_ThirdPropeller.isAssembled && m_Support.isAssembled)
            return true;
        else
            return false;
    }

}
