using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] Image soundOnIcon;  // private but able to be shown in the editor
    [SerializeField] Image soundOffIcon;
    private bool muted = false;

    
    void Start()
    {
        // if no save data from previous game session
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        // if there is data from previous session
        else
        {
            Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = muted; 
           
    }

    public void OnButtonPress() // if sound is on then button is pressed, mute music and vice versa
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }
    
    private void UpdateButtonIcon() // update the sound button icon
    {
        if (muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }

        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }

    //
    private void Load() // if muted == 1, muted == true and if muted == 0, muted == false
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save() // if muted is true, use 1. if false, use 0
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);


    }

}
