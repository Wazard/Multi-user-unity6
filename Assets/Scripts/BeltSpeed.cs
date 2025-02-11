using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BeltSpeed : MonoBehaviour
{
    [Header("Buttons"), Space(5)]
    [SerializeField] private Button increaseSpeedButton;
    [SerializeField] private Button reduceSpeedButton;

    [Space(10), Header("Conveyor belt settings"), Space(5)]
    public GameObject ConveyorSpeed;
    public GameObject parentObject;
    public float beltSpeedOffset = .01f;

    [Space(10), Header("UI"), Space(5)]
    public TMP_Text speedText;
    
    // Start is called before the first frame update
    void Start()
    {
        speedText.text = "Speed: " + ConveyorBelt.m_BeltSpeed;
        increaseSpeedButton.onClick.AddListener(increaseVelocity);
        reduceSpeedButton.onClick.AddListener(reduceVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        //speedText.text = "Speed: " + speed;
        

    if(Input.GetKeyDown(KeyCode.P))
        {
            increaseVelocity();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            reduceVelocity();
        }
    }

    private void reduceVelocity(){
        ConveyorBelt.m_BeltSpeed -= beltSpeedOffset;
        UpdateText();
    }    
    private void increaseVelocity(){
        ConveyorBelt.m_BeltSpeed += beltSpeedOffset;
        UpdateText();
    }
    private void UpdateText()
    {
        speedText.text = $"Speed: {ConveyorBelt.m_BeltSpeed:0.00}";
        Debug.Log("belt speed set to: " + ConveyorBelt.m_BeltSpeed.ToString("0.00") );
    }


}
