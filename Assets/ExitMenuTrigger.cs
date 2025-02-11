using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class ExitMenuTrigger : MonoBehaviour
{
    public GameObject exitMenu;  // Assign the exit menu GameObject in the Inspector
    public Button confirmButton;
    public Button cancelButton;

    private void Start()
    {
        // Ensure the menu is not active at the start
        exitMenu.SetActive(false);

        // Assign button listeners
        confirmButton.onClick.AddListener(ConfirmExit);
        cancelButton.onClick.AddListener(CancelExit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("sono entrato in collisione");
            ShowExitMenu();
        }
    }

    private void ShowExitMenu()
    {
        if (exitMenu != null)
        {
            exitMenu.SetActive(true);
            Debug.Log("Exit menu ON");
        }
    }

    private void ConfirmExit()
    {
        // Confirm exit logic
        Application.Quit();
        Debug.Log("Exiting Game");
    }

    private void CancelExit()
    {
        // Cancel exit logic
        exitMenu.SetActive(false);
        Debug.Log("Exit Canceled");
    }
}
