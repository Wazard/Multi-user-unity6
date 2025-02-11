using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Realtime;
public class XRGrabNetworkInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    private PhotonView photonView;
    [SerializeField]PartToBeAssembled partToBeAssembled;
    public List<AttachementPoint> Parts = new List<AttachementPoint>();

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(interactionManager == null)
        {
            interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        }
        if(gameObject.GetComponent<PartToBeAssembled>())
            StartCoroutine(AddListenersAfterDelay());
    }

    public IEnumerator AddListenersAfterDelay()
    {
        yield return new WaitForSeconds(8f);
        List<AttachementPoint> attachementPoints = new List<AttachementPoint>();
        GameObject rotore = gameObject.transform.parent.Find("Rotore(Clone)").gameObject;
        AttachementPoint point = new AttachementPoint();
        if (gameObject.name.Contains("Elica"))
        {
            for(int j = 0; j < rotore.transform.childCount; j++)
            {
                if (rotore.transform.GetChild(j).gameObject.name.Contains("Elica") && !rotore.transform.GetChild(j).gameObject.name.Contains("To"))
                {
                    point = rotore.transform.GetChild(j).gameObject.GetComponent<AttachementPoint>();
                    //selectEntered.AddListener(point.NewObjectTakenBack);
                    //selectExited.AddListener(point.NewObjectReleased);
                    Parts.Add(point);
                }
            }
        }
        else
        {
            for (int j = 0; j < rotore.transform.childCount; j++)
            {
                if (rotore.transform.GetChild(j).gameObject.name.Contains("Sostegno") && !rotore.transform.GetChild(j).gameObject.name.Contains("To"))
                {
                    point = rotore.transform.GetChild(j).gameObject.GetComponent<AttachementPoint>();
                    //selectEntered.AddListener(point.NewObjectTakenBack);
                    //selectExited.AddListener(point.NewObjectReleased);
                    Parts.Add(point);
                }
            }
        }
    }
    
    [System.Obsolete]
    protected override void OnSelectEntered(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
    {
        if(!photonView.IsMine)
         photonView.RequestOwnership();

        base.OnSelectEntered(interactor);
    }
    [System.Obsolete]
    //I layer vengono cambiati per un piccolo quantitativo di tempo al fine di evitare la collisione tra mano e oggetto quando viene rilasciato l'oggetto
    protected override void OnSelectExited(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        int oldLayer=0;
        if(gameObject.layer != 10)
        {
            oldLayer = gameObject.layer;
        }
        gameObject.layer = 10;
        StartCoroutine(GiveOldLayerCountdown(0.25f, oldLayer));
        if(partToBeAssembled != null && gameObject.transform.parent.gameObject.GetComponent<AttachementPoint>() != null)
        {
            gameObject.transform.SetParent(partToBeAssembled.startingParent.transform, true);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
        
    }

    public IEnumerator GiveOldLayerCountdown(float delay, int oldLayer)
    {
        yield return new WaitForSeconds(delay);
        gameObject.layer = oldLayer;
    }
    
    //adding a listener to the appropriate unity event doesn't seem to work, no time to understand why for today
    //So i will call these two methods using references filled at runtime in the first second of existence of the attached object
    public void CallObjectTakenBack(SelectEnterEventArgs args)
    {
        if (gameObject.GetComponent<PartToBeAssembled>())
        {
            foreach (AttachementPoint part in Parts)
            {
                part.NewObjectTakenBack(args);
            }
        }
    }

    public void CallObjectReleased(SelectExitEventArgs args)
    {
        if (gameObject.GetComponent<PartToBeAssembled>())
        {
            foreach (AttachementPoint part in Parts)
            {
                part.NewObjectReleased(args);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("hand") || collision.gameObject.name.Contains("Hand"))
        {
            if (!photonView.IsMine)
                photonView.RequestOwnership();
        }
    }
}
