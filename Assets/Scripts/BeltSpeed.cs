using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BeltSpeed : MonoBehaviour
{
    [Header("Buttons"), Space(5)]
    [SerializeField] private Button increaseSpeedButton;
    [SerializeField] private Button decreaseSpeedButton;

    [Space(10), Header("Conveyor belt settings"), Space(5)]
    public GameObject ConveyorSpeed;
    public GameObject parentObject;
    public double speed = 0.1;

    [Space(10), Header("UI"), Space(5)]
    public TMP_Text speedText;
    public Action<double> onBeltSpeedChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        
        speedText.text = "Speed: " + speed;
    }

    // Update is called once per frame
    void Update()
    {
        //speedText.text = "Speed: " + speed;
        

    if(Input.GetKeyDown(KeyCode.P))
            {
            IncreaseSpeedText();
            }
        if (Input.GetKeyDown(KeyCode.O))
            {
            DecreaseSpeedText();
            }
    }

    private void DecreaseSpeedText()
    {
        speed -=  0.01;
        speedText.text = "Speed: " + speed;
        Debug.Log("Velocità diminuita a: " + speed );
    }
    private void IncreaseSpeedText()
    {
        speed += 0.01;
        speedText.text = "Speed: " + speed;
        Debug.Log("Velocità aumentata a: " + speed);
    }


}
