using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

namespace audio_menu
{


public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer MyMixer;

    public TMP_Text volumeText;
    public TMP_Text buttonText;
    public GameObject audio_menu;
        public GameObject amenu;
        public GameObject Audio_Source;

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        MyMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        if (volumeText != null)
        {
            volumeText.text = "Volume: " + (volume * 100).ToString("F0") + "%";
        }
    }
    void Start()
    {
        float volume = volumeSlider.value;
        if (volumeText != null)
        {
            volumeText.text = "Volume: " + (volume * 100).ToString("F0") + "%";
        }
        if (volume != 0)
        {
            buttonText.text = "Mute";
        }
        /*if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume * 100f;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
        else
        {
            Debug.LogError("Slider non collegato nell'Editor di Unity!");
        }*/
    }
    private void Update()
    {
            //if (amenu.activeSelf == true)
            //{
            //    audio_menu.SetActive(false);
            //}
            if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Input Mute");
            ToggleAudio();
            //Mute_Audio();
        }
            if (Input.GetKeyDown(KeyCode.Menu))
            {
                audio_menu.SetActive(false);
                //if (audio_menu != null && audio_menu.activeSelf)
                //{
                //    audio_menu.SetActive(false);
                //    Debug.Log("Oggetto disattivato.");
                //}
                //else
                //{
                //    Debug.Log("L'oggetto è già disattivato o è null.");
                //}
            }
        }
    public void ToggleAudioMenu()
    {
        if (audio_menu != null)
        {
            // Inverti lo stato attivo/inattivo del menu
            audio_menu.SetActive(!audio_menu.activeSelf);
        }
        else
        {
            Debug.LogError("Menu non collegato nell'Editor di Unity!");
        }
    }

    public void ToggleAudio()
    {
        if (Audio_Source != null)
        {
            // Inverti lo stato attivo/inattivo dell'audio
            Audio_Source.SetActive(!Audio_Source.activeSelf);
        }
        else
        {
            Debug.LogError("Menu non collegato nell'Editor di Unity!");
        }
        if (Audio_Source.activeSelf is true)
        {
            float volume = volumeSlider.value;
            Debug.Log("Audio ON");
            volumeText.text = "Volume: " + (volume * 100).ToString("F0") + "%";
            buttonText.text = "Mute";
            
        }
        else
        {
            Debug.Log("Audio OFF");
            volumeText.text = "AUDIO MUTED";
            buttonText.text = "Unmute";
        }
    }
    /*public void Mute_Audio()
    {
        Debug.Log("Muted");
        float volume = volumeSlider.value;
        if(volumeSlider.value != 0)
        {
        MyMixer.SetFloat("volume", Mathf.Log10(volume) * 0);
        }
        else
        {
            SetVolume();
        }
        
    }*/



}
}
