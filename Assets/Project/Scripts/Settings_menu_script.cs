using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace settings_menu
{
public class Settings_menu_script : MonoBehaviour
{
    public GameObject settings_menu;

        // Update is called once per frame
        public void Update()
    {
            if (Input.GetKeyDown(KeyCode.K))
                {
                ToggleSettingsMenu();
            }
        }
    public void ToggleSettingsMenu()
        {
            if (settings_menu != null)
            {
            Debug.Log("Settings menu attivato");
                // Inverti lo stato attivo/inattivo del menu
                settings_menu.SetActive(!settings_menu.activeSelf);
            }
            else
            {
                Debug.LogError("Menu non collegato nell'Editor di Unity!");
            }

        }
}

}
