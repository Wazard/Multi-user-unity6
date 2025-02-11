using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
public class NetworkPlayer : MonoBehaviour
{

    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    public Animator lefthandAnimator;
    public Animator righthandAnimator;

    [SerializeField] private PhotonView photonView;

    private Transform headrig;
    private Transform lefthandrig;
    private Transform righthandrig;


    // Start is called before the first frame update
    /// <summary>
    //[System.Obsolete]
    /// </summary>
    void Start()
    {
        photonView??= GetComponent<PhotonView>();
        GameObject rig = FindFirstObjectByType<XROrigin>().transform.GetChild(0).gameObject;
        headrig = rig.transform.Find("Main Camera");
        lefthandrig = rig.transform.Find("Left Controller");
        righthandrig = rig.transform.Find("Right Controller");

        if (!photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            righthand.gameObject.SetActive(false);
            lefthand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);

            MapPosition(head, headrig);
            MapPosition(lefthand, lefthandrig);
            MapPosition(righthand, righthandrig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), lefthandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), righthandAnimator);
        }


    }
    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    void MapPosition(Transform target, Transform rigTransform)
    {


        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

}