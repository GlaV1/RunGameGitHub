using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class Quality_DropDown : MonoBehaviour
{
    public TMP_Dropdown _GameQualityDropdown;
    public LocalizedString[] localizedOptions;
    void Start()
    {
        StartCoroutine(LoadLocalization());
        LocalizationSettings.SelectedLocaleChanged += OnLocalChanged;
    }
    IEnumerator LoadLocalization()
    {
        yield return LocalizationSettings.InitializationOperation;
        UpdateDropDown();
    }
    private void OnLocalChanged(UnityEngine.Localization.Locale locale)
    {
        UpdateDropDown();
    }
    private void UpdateDropDown()
    {
        _GameQualityDropdown.ClearOptions();
        foreach (var item in localizedOptions)
        {
            string translatedText = item.GetLocalizedString();
            _GameQualityDropdown.options.Add(new TMP_Dropdown.OptionData(translatedText));
        }
        _GameQualityDropdown.RefreshShownValue();
    }
}
