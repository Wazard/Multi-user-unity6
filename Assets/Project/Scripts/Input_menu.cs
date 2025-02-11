using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using ContinuousMoveProvider = UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider;
using TeleportationProvider = UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider;
public class Input_menu : MonoBehaviour
{
    public GameObject target;
    public TMP_Text movement_text;
    
    public GameObject teleport_l;
    public GameObject input_menu;
    public bool teleport=true;
        
        private void Start()
    {
        MonoBehaviour[] scripts = target.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script is ContinuousMoveProvider)
            {
                script.enabled = false;
            }
            if (script is TeleportationProvider)
            {
                //teleport_r.SetActive(true);
                //teleport_l.SetActive(true);
                //script.enabled = true;
                teleport = true;
            }
        }
        movement_text.text = "Current Movement : Teleport";
    }
    private void Update()
    {
        
        
            if (Input.GetKeyDown(KeyCode.Menu) && input_menu != null)
            {
                input_menu.SetActive(false);
              
            }


        }

    public void ToggleInputMenu()
    {
        if (input_menu != null)
        {
            // Inverti lo stato attivo/inattivo del menu
            input_menu.SetActive(!input_menu.activeSelf);
        }
        else
        {
            Debug.LogError("Menu non collegato nell'Editor di Unity!");
        }

    }
   
    public void ChangeMovementToAnalog()
    {
        
        MonoBehaviour[] scripts = target.GetComponents<MonoBehaviour>();
        {
            foreach (MonoBehaviour script in scripts)
            {
                if (script is ContinuousMoveProvider)
                {
                    script.enabled = true;
                    Debug.Log("Movement method changed to analog");
                }
                if (script is TeleportationProvider)
                {
                    
                    teleport_l.SetActive(false);
                    script.enabled = false;
                    teleport = false;
                }
            }
            movement_text.text = "Current Movement : Analog";
        }
    }
        public void ChangeMovementToTeleport()
        {
            


            MonoBehaviour[] scripts = target.GetComponents<MonoBehaviour>();
            {
                foreach (MonoBehaviour script in scripts)
                {
                    if (script is ContinuousMoveProvider)
                    {
                        script.enabled = false;
                    }
                    if (script is TeleportationProvider)
                    {
                    //teleport_r.SetActive(true);
                    //teleport_l.SetActive(true);
                    //script.enabled = true;
                    teleport = true;
                    Debug.Log("Movement method changed to Teleport");
                    }
                }
                movement_text.text = "Current Movement : Teleport";


            }
        }
}
