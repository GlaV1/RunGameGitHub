using rgame;
using rgamekeys;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsManager : MonoBehaviour
{
    [Header("Ses Sliderlari")]
    [Tooltip("Menu Audio Slider")]
    public Slider _MenuAudioSlider;
    [Tooltip("Menu Fx Audio Slider")]
    public Slider _MenuFxAudioSlider;
    [Tooltip("GAme Audio Slider")]
    public Slider _GameAudioSlider;

    [Header("Oyun Kalite DropDown")]
    [Tooltip("Oyun Kalite DropDown")]
    public TMP_Dropdown _GameQualityDropdown;

    [Header("Dil Secenekleri DwopDown")]
    [Tooltip("Dil secenekleri DropDown")]
    public TMP_Dropdown _GameLanguageDropdown;
    // AudioSource MenuAudioSource;
    // AudioSource MenuFxAudioSource;
    //  AudioSource GameAudioSource;
    MemoryManagement _MemoryManagement= new MemoryManagement();

    /// <summary>
    /// Oyun ba�lad���nda olmas� gerekenler.Kay�t dosyas�ndan ses seviyelerini okur ve verileir getirir
    /// </summary>
    void Start()
    {
        _MenuAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.MenuAudio);
        _MenuFxAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.MenuFxAudio);
        _GameAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.GameAudio);
        _GameLanguageDropdown.value = _MemoryManagement.ReadData_int(SaveKeys.SelectedLanguage);
    }

    /// <summary>
    /// Ana men� sahnesinin y�kleme i�lemini yapar
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);//men� sahnesinin y�klenme i�lemi
    }
    /// <summary>
    /// kullan�c�dan dropdown men�s� ile Oyun Kalitesini al�r ve De�i�iklikleri uygular
    /// </summary>
    /// <param name="SelectedQuality">Se�ilen kalite de�erini kullan�c�dan al�r</param>
    public void QualitySelection(int SelectedQuality)
    {
        PlayerPrefs.SetInt(SaveKeys.SelectedQuality,SelectedQuality);
        QualitySettings.SetQualityLevel(SelectedQuality);
    }

    public void LanguageSelection(int SelectionLanguage)
    {
        _MemoryManagement.SaveData_int(SaveKeys.SelectedLanguage, SelectionLanguage);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[SelectionLanguage];
    }

    /// <summary>
    /// Kullan�c�dan gelen veriye g�re ses seviyelerini kay�t eder
    /// </summary>
    /// <param name="process">Kullan�c�dan Hangi Ses seviyesini de�i�tirmek istedi�i al�n�r</param>
    public void AudioChange(int process)//process=0 MenuAudioChange|| process=1 MenuFxAudioChange || process=2 GameAudioChange
    {
        switch (process)
        {
            case 0:
                 _MemoryManagement.SaveData_float(SaveKeys.MenuAudio, _MenuAudioSlider.value);
                break;
            case 1:
                 _MemoryManagement.SaveData_float(SaveKeys.MenuFxAudio, _MenuFxAudioSlider.value);
                break;
            case 2:
                 _MemoryManagement.SaveData_float(SaveKeys.GameAudio, _GameAudioSlider.value);
                break;
        }
    }
}
