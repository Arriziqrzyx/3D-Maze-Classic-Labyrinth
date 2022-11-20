using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider BGM;
    [SerializeField] Slider SFX;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle toggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    public static AudioManager Instance { get; set; }


    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;
        
        for (int i = 0; i < resolutions.Length; i++ )
        {
            if(resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetFullscreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
        if (toggle.isOn == true)
            Debug.Log("Fullscreen");
        else
            Debug.Log("WindowS");
    }
    private void Awake() {
        float db;
        if(audioMixer.GetFloat("MasterVol", out db))
            masterVolume.value = (db+80)/80;

        if(audioMixer.GetFloat("BGMVol", out db))
            BGM.value = (db+80)/80;

        if(audioMixer.GetFloat("SFXVol", out db))
            SFX.value = (db+80)/80;
    }

    public void SetMasterVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("MasterVol", volume);
        PlayerPrefs.SetFloat("MasterVol", volume);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVol", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
        PlayerPrefs.Save();
    }

    public void Keluar()
    {
        Debug.Log ("KAMU TELAH KELUAR!");
        Application.Quit();
    }



}
