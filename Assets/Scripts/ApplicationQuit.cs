using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("sono uscito");
    }
    
}
