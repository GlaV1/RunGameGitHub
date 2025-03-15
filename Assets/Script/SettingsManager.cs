using rgame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider _MenuAudioSlider;
    public Slider _MenuFxAudioSlider;
    public Slider _GameAudioSlider;
    public TMP_Dropdown _GameQualityDropdown;
    public TMP_Dropdown _GameLanguageDropdown;
    AudioSource MenuAudioSource;
    AudioSource MenuFxAudioSource;
    AudioSource GameAudioSource;
    
    MemoryManagement _MemoryManagement= new MemoryManagement();
    void Start()
    {
        _MenuAudioSlider.value = _MemoryManagement.ReadData_float("MenuAudio");
        _MenuFxAudioSlider.value = _MemoryManagement.ReadData_float("MenuFxAudio");
        _GameAudioSlider.value = _MemoryManagement.ReadData_float("GameAudio");


      //  _GameQualityDropdown.value = PlayerPrefs.GetInt("SelectedQuality"); 
    }

    // Update is called once per frame
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);//menü sahnesinin yüklenme iþlemi
    }
    public void QualitySelection(int SelectedQuality)
    {
        PlayerPrefs.SetInt("SelectedQuality",SelectedQuality);
        QualitySettings.SetQualityLevel(SelectedQuality);
    }

    /*public void LanguageSelection(int SelectedLanguage)
    {
        PlayerPrefs.SetInt("SelectedLanguage",SelectedLanguage);

    }*/


    public void AudioChange(int process)//process=0 MenuAudioChange|| process=1 MenuFxAudioChange || process=2 GameAudioChange
    {
        switch (process)
        {
            case 0:
                 _MemoryManagement.SaveData_float("MenuAudio", _MenuAudioSlider.value);
                break;
            case 1:
                 _MemoryManagement.SaveData_float("MenuFxAudio", _MenuFxAudioSlider.value);
                break;
            case 2:
                 _MemoryManagement.SaveData_float("GameAudio", _GameAudioSlider.value);
                break;
        }
    }
}
