using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.GraphicsBuffer;
public class MenuManager : MonoBehaviour

{
    public GameObject MUI;
    public GameObject target;
    public GameObject l_teleport;
   
    public InputActionProperty showButton;
    public InputActionProperty showButtonOculus;
    public GameObject menu;
    public GameObject smenu;
    public GameObject amenu;
    public GameObject imenu;
    
    public bool menuPressed;
    private bool menuTogglePressed;
    public bool activemenu = false;
    [SerializeField] Input_menu inputMenu;

    
    private DetectVR.VRController controllerName;
    public void Start()
    {
        controllerName = DetectVR.GetControllerTypeToEnum();
        UIUtilities.AdjustHeight(MUI);
    }


    // Update is called once per frame
   


        // Update is called once per frame
    public void Update()
    {


            if (controllerName == DetectVR.VRController.oculus_touch)
            {
                if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyUp(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3) /*&& !menuPressed*/)
                {
                    ToggleMenu();
                    Debug.Log("Pause menu ON");
                    menuPressed = true; // Mark as pressed
                    menuTogglePressed = true;
                }
                else if (menuTogglePressed && (Input.GetKeyUp(KeyCode.Menu) || Input.GetKeyUp(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3)))
                {
                    menuPressed = false; // Reset the flag when the key is released
                }
            }

                else if (showButton.action.WasPerformedThisFrame() || Input.GetKeyUp(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3) /*&& !menuPressed*/)
                {
            ToggleMenu();
            Debug.Log("Pause menu ON");
            menuPressed = true; // Mark as pressed
            menuTogglePressed = true;
                }
                else if (menuTogglePressed && (Input.GetKeyUp(KeyCode.Menu) || Input.GetKeyUp(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3)))
                {
            menuPressed = false; // Reset the flag when the key is released
                 }

        
    }
    public void ToggleMenu()
    {
        if (menu != null)
        {
            if (menu.activeSelf || smenu.activeSelf || amenu.activeSelf || imenu.activeSelf) 
            {
                menu.SetActive(false);
                smenu.SetActive(false);
                amenu.SetActive(false);
                imenu.SetActive(false);
                if (inputMenu.teleport)
                    ToggleTeleport(true);
            }
            else 
            {
                menu.SetActive(true);
                ToggleTeleport(false);
            }

        // Inverti lo stato attivo/inattivo del menu
        Debug.Log("TOGGLE MENU");
        }
        
    }

    public void ToggleTeleport(bool shouldTeleportBeActive)
    {
        if (inputMenu.teleport == false)
        {
            shouldTeleportBeActive = false;
        }
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider tp = target.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
        tp.enabled = shouldTeleportBeActive;
        l_teleport.SetActive(shouldTeleportBeActive);
        
        
    }
   
}
