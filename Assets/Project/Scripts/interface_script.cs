using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using settings_menu;

namespace menu

{


public class interface_script : MonoBehaviour
{


    public GameObject menu; // Assicurati di collegare il menu nell'Editor di Unity

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ToggleMenu();
        //    //PauseMenu();
        //}
        ////if (Input.GetKeyDown(KeyCode.Menu))
        ////{
        ////        ToggleMenu();
        ////        Debug.Log("Pause menu ON");
        ////}
    }
    
    public void ToggleMenu()
    {
        if (menu != null)
        {
            // Inverti lo stato attivo/inattivo del menu
            menu.SetActive(!menu.activeSelf);
        }
        else
        {
            Debug.LogError("Menu non collegato nell'Editor di Unity!");
        }
     
    }
    public void PauseMenu()
    {
        //if (Input.GetKeyDown(KeyCode.Menu))
        //{

        //    ToggleMenu();
        //    Debug.Log("Pause menu ON");
        //    //Time.timeScale = (menu.activeSelf) ? 0 : 1;
        //}
        //else
        //{
        //    ToggleMenu();
        //    Debug.Log("Pause menu OFF");
        //    //Time.timeScale = (menu.activeSelf) ? 0 : 1;
        //}
    }
    public void ResumeButton()
    {
        ToggleMenu();
        PauseMenu();
    }

    //public void Application_Quit()
    //{
    //    UnityEditor.EditorApplication.isPlaying = false;
    //    Application.Quit();
    //}
}

}

