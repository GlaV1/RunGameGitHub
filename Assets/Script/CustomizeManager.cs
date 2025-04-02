using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using rgame;
using TMPro;
using System.IO;
using ProtoBuf;
using System;
using System.Collections.ObjectModel;
using static UnityEditor.Progress;
using System.Threading;
using TMPro.EditorUtilities;
using System.Linq;
using OpenCover.Framework.Model;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
//using System.Diagnostics;


public class CustomizeManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject[] PanelProcess;
    int ActiveCustomizePanelIndex = 0;//aktif panel

    [Header("Canvaslar")]   
    public GameObject[] CanvasProcess;

    [Header("Text")]
    public TextMeshProUGUI PointText;//d��ar�dan puan verilmesi
    public TextMeshProUGUI BuyingText;//d��ar�dan puan verilmesi
    public TextMeshProUGUI AlertText;//d��ar�dan puan verilmesi//
    public Text HatText;//d��ar�dan �apka texti verilmesi
    public Text StickText;//d��ar�dan sopa texti verilmesi
    public Text ManColorText;//d��ar�dan tema rengi texti verilmesi
   
    [Header("Buton")]
    public Button BuyButton;
    public Button CustomizeSaveButton;
   

    [Header("�apka ��lemleri")]
    public Button[] HatButtons;
    public GameObject[] Hats;//�arpka dizisi olu�turuluyor
    int hatindex=-1;//hatindex -1 den ba�lar

    [Header("Sopa ��lemleri")]
    public Button[] StickButtons;
    public GameObject[] Sticks;//sopa dizisi olu�turuluyor
    static int stickindex = -1;

    [Header("Karakter Color ��lemleri")]
    public Material[] ManColorMaterials;//karakter materyalleri olu�turuluyor
    public Material DefaultManColorMaterial;//karakter materyalleri olu�turuluyor
    public Button[] ManColorButtons;
    
    [Header("Karakter ��lemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;
    int mancolorindex = -1;

    [Header("ITEM RENK ISLEMLERI")]

    [Header("Sopalar")]
    public Button[] StickColorButtons;
    public Material StickColorMaterial;
    public Material DefaultStickColorMaterial;
    public RawImage StickColorRawImage;
    int stickcolorindex=-1;

    [Header("Sapkalar")]
    public Button[] HatColorButtons;
    public Material HatColorMaterial;
    public Material DefaultHatColorMaterial;
    public RawImage HatColorRawImage;
    int hatcolorindex = -1;
   
    [Header("Item Renk Adlar�")]
    public List<string> HatColorName;
    public List<string> StickColorName;



    /////////
    MemoryManagement _MemoryManagement= new MemoryManagement();
    DataManager _DataManager = new DataManager();
    LanguageManager _LanguageManager = new LanguageManager();   
    [Header("�tem Bilgi ��lemleri")]
    public List<ItemInformations> _ItemInformations = new List<ItemInformations>();

    /////////


    ////////Dil de�i�iklik ��lem�eri  
    private string LocalizationCutomizeItemTableName = "Customize_Table"; //�tem textlerinin tutuldu�u tablo
    private string LocalizationCustomizeTextTableName="Game_Text"; //oyun textlerinin tutuldu�u tablo
    public void UpdateLocalizedItemNames()
    {
        foreach (var item in _ItemInformations)
        {
            item.ItemName = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationCutomizeItemTableName, item.LocalizationKey);
        }

    }
    private void OnEnable()
    {
        // Dil de�i�ti�inde isimleri tekrar g�ncelle
        LocalizationSettings.SelectedLocaleChanged += UpdateNamesOnLanguageChange;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= UpdateNamesOnLanguageChange;
    }

    private void UpdateNamesOnLanguageChange(Locale locale)
    {
        UpdateLocalizedItemNames();
    }
    ////////Dil de�i�iklik ��lem�eri

    void Start()
    {
        _LanguageManager.LanguageSelection(_MemoryManagement.ReadData_int("SelectedLanguage"));
        UpdateLocalizedItemNames();
        MakeControl(0,true);
        MakeControl(1,true);
        MakeControl(2,true);
        _MemoryManagement.SaveData_int("Point", 5000);
        PointText.text = _MemoryManagement.ReadData_int("Point").ToString();
        _DataManager.DataSave(_ItemInformations);
        _DataManager.DataUpload();
        _ItemInformations = _DataManager.TransferList();
    }

    /// <summary>
    /// Ana mEn�ye d�nme i�lemleri
    /// </summary>
    public void BackToMainMenu()
    {
        _DataManager.DataSave(_ItemInformations);
        SceneManager.LoadScene(0);//men� sahnesinin y�klenme i�lemi
    }
    /// <summary>
    /// �apka �temlerinde gezinme
    /// </summary>
    /// <param name="direction">Y�n belirtme</param>
    public void HatDirection(string direction)
    {
        PublicDirectionButtons(0, direction);      
    }
    /// <summary>
    /// Sopa itemlerinde gezinme
    /// </summary>
    /// <param name="direction">Y�n belirtme</param>
    public void StickDirection(string direction)
    {
        PublicDirectionButtons(1, direction);
    }
   /// <summary>
   /// karakter temalar�nda gezinme
   /// </summary>
   /// <param name="direction">y�n belirtme</param>
    public void ManColorDirection(string direction)
    {
        PublicDirectionButtons(2, direction);
    }

    /// <summary>
    /// �apka item renklerinde gezinme
    /// </summary>
    /// <param name="direction">Y�n Belirtme</param>
    public void HatColorDirection(string direction)
    {
        PublicItemColorDirection(0,direction);
    }
    /// <summary>
    /// Sopa item renklerinde gezinme
    /// </summary>
    /// <param name="direction">Y�n Belirtme</param>
    public void StickColorDirection(string direction)
    {
        PublicItemColorDirection(1,direction);
    }


    /// <summary>
    ///  Karakter �zelle�tirme itemlerinin RENKleri  aras�nda gezinme i�lemlerini indexlere g�re yapar
    /// </summary>
    /// <param name="process">Hangi �temlerin renklerinin aras�nda gezilece�ini belirler</param>
    /// <param name="direction">>�leri mi geri gidilece�ini belirler</param>
    private void PublicItemColorDirection(int process,string direction)
    {
        Color NewColor;
        switch (process)
        {
            case 0:
                if (direction=="Foward")
                {
                    if (hatcolorindex==-1)
                    {
                        hatcolorindex = 0;
                        if (ColorUtility.TryParseHtmlString(HatColorName[hatcolorindex], out NewColor))
                        {
                            HatColorMaterial.color = NewColor;
                            HatColorRawImage.color = NewColor;
                        }
                    }
                    else
                    {
                        hatcolorindex++;
                        if (ColorUtility.TryParseHtmlString(HatColorName[hatcolorindex], out NewColor))
                        {
                            HatColorMaterial.color = NewColor;
                            HatColorRawImage.color = NewColor;
                        }
                    }
                    if (hatcolorindex == HatColorName.Count- 1)
                    {
                        HatColorButtons[1].interactable = false;
                    }
                    else if (hatcolorindex != -1)
                    {
                        HatColorButtons[0].interactable = true;
                    }
                    else
                    {
                        HatColorButtons[1].interactable = true;
                    }
                }
                else
                {
                    if (hatcolorindex!=-1)
                    {
                        hatcolorindex--;
                        if (hatcolorindex!=-1)
                        {
                            if (ColorUtility.TryParseHtmlString(HatColorName[hatcolorindex], out NewColor))
                            {
                                HatColorMaterial.color = NewColor;
                                HatColorRawImage.color = NewColor;
                            }
                        }
                        else
                        {
                            HatColorMaterial.color=DefaultHatColorMaterial.color;
                            HatColorRawImage.color=Color.white;
                            HatColorButtons[0].interactable = false;
                        }
                    }
                    else
                    {
                        HatColorButtons[0].interactable = false;
                    }
                    if (hatcolorindex != HatColorName.Count - 1)
                    {
                        HatColorButtons[1].interactable = true;
                    }
                }
            break;
            case 1:
                if(direction=="Foward")
                {
                    if (stickcolorindex == -1)
                    {
                        stickcolorindex = 0;
                        if (ColorUtility.TryParseHtmlString(StickColorName[stickcolorindex], out NewColor))
                        {
                            StickColorMaterial.color = NewColor;
                            StickColorRawImage.color = NewColor;
                        }
                    }
                    else
                    {
                        stickcolorindex++;
                        if (ColorUtility.TryParseHtmlString(StickColorName[stickcolorindex], out NewColor))
                        {
                            StickColorMaterial.color = NewColor;
                            StickColorRawImage.color = NewColor;
                        }
                    }
                    if (stickcolorindex == StickColorName.Count - 1)
                    {
                        StickColorButtons[1].interactable = false;
                    }
                    else if (stickcolorindex != -1)
                    {
                        StickColorButtons[0].interactable = true;
                    }
                    else
                    {
                        StickColorButtons[1].interactable = true;
                    }
                }
                else
                {
                    if (stickcolorindex != -1)
                    {
                        stickcolorindex--;
                        if (stickcolorindex != -1)
                        {
                            if (ColorUtility.TryParseHtmlString(StickColorName[stickcolorindex], out NewColor))
                            {
                                StickColorMaterial.color = NewColor;
                                StickColorRawImage.color = NewColor;
                            }
                        }
                        else
                        {
                            StickColorMaterial.color = DefaultStickColorMaterial.color;
                            StickColorRawImage.color = Color.white;
                            StickColorButtons[0].interactable = false;
                        }
                    }
                    else
                    {
                        StickColorButtons[0].interactable = false;
                    }
                    if (stickcolorindex != StickColorName.Count - 1)
                    {
                        StickColorButtons[1].interactable = true;
                    }
                }
                break;
        }
    }
    
    /// <summary>
    /// Karakter �zelle�tirme itemleri aras�nda gezinme i�lemlerini indexlere g�re yapar
    /// </summary>
    /// <param name="process">Hangi �temlerin aras�nda gezilece�ini belirler</param>
    /// <param name="direction">�leri mi geri gidilece�ini belirler </param>
    private void PublicDirectionButtons(int process, string direction)
    {
        switch (process)
        {
            case 0:
                if (direction == "Foward")
                {
                    if (hatindex == -1)
                    {
                        hatindex = 0;
                        Hats[hatindex].SetActive(true);
                        HatText.text = _ItemInformations[hatindex].ItemName;
                        PurchaseControl(hatindex);
                    }
                    else
                    {
                        Hats[hatindex].SetActive(false);
                        hatindex++;
                        Hats[hatindex].SetActive(true);
                        HatText.text = _ItemInformations[hatindex].ItemName;
                        PurchaseControl(hatindex);
                    }

                    if (hatindex == Hats.Length - 1)
                    {
                        HatButtons[1].interactable = false;
                    }
                    else if (hatindex != -1)
                    {
                        HatButtons[0].interactable = true;
                    }
                    else
                    {
                        HatButtons[1].interactable = true;
                    }
                }
                else
                {
                    if (hatindex != -1)
                    {
                        Hats[hatindex].SetActive(false);
                        hatindex--;
                        if (hatindex != -1)
                        {
                            Hats[hatindex].SetActive(true);
                            HatButtons[0].interactable = true;
                            HatText.text = _ItemInformations[hatindex].ItemName;
                            PurchaseControl(hatindex);
                        }
                        else
                        {
                            HatButtons[0].interactable = false;
                            HatText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_HatPanel_txt_NoHatText");
                            BuyingText.text = "-";
                            BuyButton.interactable = false;
                        }
                    }
                    else
                    {
                        HatButtons[0].interactable = false;
                        HatText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_HatPanel_txt_NoHatText");
                        BuyingText.text = "-";
                        BuyButton.interactable = false;
                    }
                    if (hatindex != Hats.Length - 1)
                    {
                        HatButtons[1].interactable = true;
                    }
                }
                ItemColorPanelControl(0,hatindex);
                break;
            case 1:
                int StickArrayCalculation = (Hats.Length) + stickindex;
                if (direction == "Foward")
                {
                    if (stickindex == -1)
                    {
                        stickindex = 0;
                        Sticks[stickindex].SetActive(true);
                        StickText.text = _ItemInformations[StickArrayCalculation + 1].ItemName;
                        PurchaseControl(StickArrayCalculation + 1);
                    }
                    else
                    {
                        Sticks[stickindex].SetActive(false);
                        stickindex++;
                        Sticks[stickindex].SetActive(true);
                        StickText.text = _ItemInformations[StickArrayCalculation + 1].ItemName;
                        PurchaseControl(StickArrayCalculation + 1);
                    }

                    if (stickindex == Sticks.Length - 1)
                    {
                        StickButtons[1].interactable = false;
                    }
                    if (stickindex != -1)
                    {
                        StickButtons[0].interactable = true;
                    }
                    else
                    {
                        StickButtons[1].interactable = true;
                    }
                }
                else
                {
                    if (stickindex != -1)
                    {
                        Sticks[stickindex].SetActive(false);
                        stickindex--;
                        if (stickindex != -1)
                        {
                            Sticks[stickindex].SetActive(true);
                            StickButtons[0].interactable = true;
                            StickText.text = _ItemInformations[StickArrayCalculation - 1].ItemName;
                            PurchaseControl(StickArrayCalculation - 1);
                        }
                        else
                        {
                            StickButtons[0].interactable = false;
                            StickText.text =_LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_SticksPanel_txt_NoStickText");
                            BuyingText.text = "-";
                            BuyButton.interactable = false;
                        }
                    }
                    else
                    {
                        StickButtons[0].interactable = false;
                        StickText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_SticksPanel_txt_NoStickText"); 
                        BuyingText.text = "-";
                        BuyButton.interactable = false;
                    }
                    if (stickindex != Sticks.Length - 1)
                    {
                        StickButtons[1].interactable = true;
                    }
                }
                ItemColorPanelControl(1,StickArrayCalculation+1);
                break;
            case 2:
                int ManColorArrayCalculation = (mancolorindex) + (Hats.Length) + (Sticks.Length);
                if (direction == "Foward")
                {
                    if (mancolorindex == -1)
                    {
                        mancolorindex = 0;
                        ManColorText.text = _ItemInformations[ManColorArrayCalculation+1].ItemName;
                        PurchaseControl(ManColorArrayCalculation+1);
                    }
                    else
                    {
                        mancolorindex++;
                        Material[] mats = _SkinnedMeshRenderer.materials;
                        mats[0] = ManColorMaterials[mancolorindex];
                        _SkinnedMeshRenderer.materials = mats;
                        ManColorText.text = _ItemInformations[ManColorArrayCalculation + 1].ItemName;
                        PurchaseControl(ManColorArrayCalculation + 1);
                    }

                    if (mancolorindex == ManColorMaterials.Length - 1)
                    {
                        ManColorButtons[1].interactable = false;
                    }
                    if (mancolorindex != -1)
                    {
                        ManColorButtons[0].interactable = true;
                    }
                    else
                    {
                        ManColorButtons[1].interactable = true;
                    }
                }
                else
                {
                    if (mancolorindex != -1)
                    {
                        mancolorindex--;
                        if (mancolorindex != -1)
                        {
                            Material[] mats = _SkinnedMeshRenderer.materials;
                            mats[0] = ManColorMaterials[mancolorindex];
                            _SkinnedMeshRenderer.materials = mats;
                            ManColorButtons[0].interactable = true;
                            ManColorText.text = _ItemInformations[ManColorArrayCalculation-1].ItemName;
                            PurchaseControl(ManColorArrayCalculation-1);
                        }
                        else
                        {
                            ManColorButtons[0].interactable = false;
                            BuyingText.text = "-";
                            BuyButton.interactable = false;
                        }
                    }
                    else
                    {
                        Material[] mats = _SkinnedMeshRenderer.materials;
                        mats[0] = DefaultManColorMaterial;
                        _SkinnedMeshRenderer.materials = mats;
                        ManColorButtons[0].interactable = false;
                        BuyingText.text = "-";
                        BuyButton.interactable = false;
                    }
                    if (mancolorindex != ManColorMaterials.Length - 1)
                    {
                        ManColorButtons[1].interactable = true;
                    }
                }
                break;
        }
    }

    //panelleri a�ma ve kapatma i�lemleri
    public void ProcessPanelActive(int Index)
    {
        PanelProcess[3].SetActive(false);
        PanelProcess[ActiveCustomizePanelIndex].SetActive(false);
        MakeControl(ActiveCustomizePanelIndex, true);
        ActiveCustomizePanelIndex = -1;
        PanelProcess[Index].SetActive(true);
        PanelProcess[3].SetActive(true);  
        MakeControl(Index, false);
        ActiveCustomizePanelIndex = Index;
    }

    /// <summary>
    /// aktif panele g�re sat�n almalar�n kontrol i�lemlerini yapar yapar 
    /// </summary>
    public void CustomizeBuying()
    {
        //index hesaplama
        int StickArrayCalculation = (stickindex) + (Hats.Length) ;
        int ManColorArrayCalculation = (mancolorindex) + (Hats.Length) + (Sticks.Length);
        //index hesaplama
        if (ActiveCustomizePanelIndex > -1)
        {
            switch (ActiveCustomizePanelIndex)
            {
                case 0:
                    if (_ItemInformations[hatindex].Point <= _MemoryManagement.ReadData_int("Point") )
                    {
                        PurchasingHelper(hatindex);
                    }
                    else
                    {
                        StartCoroutine(ShowAlert(2));
                    }
                    break;
                case 1:
                    if (_ItemInformations[StickArrayCalculation].Point<= _MemoryManagement.ReadData_int("Point"))
                    {
                        PurchasingHelper(StickArrayCalculation);
                    }
                    else
                    {
                        StartCoroutine(ShowAlert(2));
                    }
                    break;
                case 2:
                    if (_MemoryManagement.ReadData_int("Point") <= _ItemInformations[ManColorArrayCalculation].Point)
                    {
                        PurchasingHelper(ManColorArrayCalculation);
                    }
                    else
                    {
                        StartCoroutine(ShowAlert(2));
                    }
                    break;
            }
        }      
    }
    
    /// <summary>
    /// Aktif Panele g�re item kaydetme ve item indexi kaydetme i�lemlerini yapar
    /// </summary>
    public void CustomizeSave()
    {
        if (ActiveCustomizePanelIndex>-1)
        {
            switch (ActiveCustomizePanelIndex)
            {
                case 0:
                    _MemoryManagement.SaveData_int("ActiveHat",hatindex);
                    _MemoryManagement.SaveData_int("ActiveHatColor",hatcolorindex);
                    StartCoroutine(ShowAlert(1));
                    break;
                case 1:
                    _MemoryManagement.SaveData_int("ActiveStick",stickindex);
                    _MemoryManagement.SaveData_int("ActiveStickColor",stickcolorindex);
                    StartCoroutine(ShowAlert(1));
                    break;
                case 2:
                    _MemoryManagement.SaveData_int("ActiveManColor",mancolorindex);
                    StartCoroutine(ShowAlert(1));
                    break;
            }
        }
    }

    /// <summary>
    ///�zelle�tirme sayfas�a��ld��� zaman Item indexlerini kay�t dosyas�ndan okur okunan verilere g�re Karaktere uygular 
    /// </summary>
    /// <param name="Index">A��lan PAnel indexi</param>
    /// <param name="process"></param>
    public void MakeControl(int Index,bool process =false)
    {
        int StickArrayCalculation = (stickindex) + (Hats.Length);
        int ManColorArrayCalculation = (mancolorindex) + (Hats.Length) + (Sticks.Length);
        if (Index == 0)
        {
            if (_MemoryManagement.ReadData_int("ActiveHat") == -1)//-1 varsay�lan �apka de�eri olarak tan�ml�
            {
                foreach (var item in Hats)//a��k bir �apka modeli olmas�n diye diziyi tek tek gezer ve �apkalar�n aktifli�ini kapat�r
                {
                    item.SetActive(false);
                }
                BuyButton.interactable = false;
                BuyingText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeItemPurchaseControlText");
                if (process == true)
                {
                    hatindex = -1;
                    HatText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_HatPanel_txt_NoHatText");
                }
                if (hatindex == Hats.Length - 1)
                {
                    HatButtons[1].interactable = false;
                }
                else if (hatindex != -1)
                {
                    HatButtons[0].interactable = true;
                }
                else if (hatindex != Hats.Length - 1)
                {
                    HatButtons[1].interactable = true;
                }
            }
            else
            {
                foreach (var item in Hats)
                {
                    item.SetActive(false);
                }           
                hatindex = _MemoryManagement.ReadData_int("ActiveHat");
                HatText.text = _ItemInformations[hatindex].ItemName;
                Hats[hatindex].SetActive(true);
                BuyButton.interactable = false;
                CustomizeSaveButton.interactable = true;
                BuyingText.text = "-";
                if (hatindex!=Hats.Length-1)
                {
                    HatButtons[1].interactable = true;
                }
                if (hatindex!=-1)
                {
                    HatButtons[0].interactable= true;
                }
                if (hatindex==Hats.Length-1)
                {
                    HatButtons[1].interactable = false;
                }
            }
            ItemColorControl(Index);
            ItemColorPanelControl(Index, hatindex);
        }
        else if (Index == 1)
        {
            ItemColorControl(Index);
            if (_MemoryManagement.ReadData_int("ActiveStick") == -1)
            {
                foreach (var item in Sticks)
                {
                    item.SetActive(false);
                }
                BuyButton.interactable = false;
                BuyingText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeItemPurchaseControlText");
                if (process == true)
                {
                    stickindex = -1;
                    StickText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_SticksPanel_txt_NoStickText");
                    BuyButton.interactable = false;
                }

                if (stickindex == Sticks.Length - 1)
                {
                    StickButtons[1].interactable = false;
                }
                else if (stickindex != -1)
                {
                    StickButtons[0].interactable = true;
                }
                else
                {
                    StickButtons[1].interactable = true;
                }
            }
            else
            {
                foreach (var item in Sticks)
                {
                    item.SetActive(false);
                }    
                stickindex = _MemoryManagement.ReadData_int("ActiveStick");
                StickText.text = _ItemInformations[StickArrayCalculation].ItemName;
                Sticks[stickindex].SetActive(true);
                BuyButton.interactable = false;
                CustomizeSaveButton.interactable = true;
                BuyingText.text = "-";

                if (stickindex != Sticks.Length - 1)
                {
                    StickButtons[1].interactable = true;
                }
                if (stickindex != -1)
                {
                    StickButtons[0].interactable = true;
                }
                if (stickindex == Sticks.Length - 1)
                {
                    StickButtons[1].interactable = false;
                }
            }
            ItemColorControl(Index);
            ItemColorPanelControl(Index, StickArrayCalculation);
        }
        else if (Index == 2)
        {
            if (_MemoryManagement.ReadData_int("ActiveManColor") == -1)
            {
                BuyingText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeItemPurchaseControlText");
                if (process == true)
                {
                    mancolorindex = -1;
                    ManColorText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "Customize_MidItemPanel_ManColorPanel_txt_NoManColorText");

                    BuyButton.interactable = false; ;
                }
                else
                {
                    Material[] mats = _SkinnedMeshRenderer.materials;
                    mats[0] = DefaultManColorMaterial;
                    _SkinnedMeshRenderer.materials = mats;
                }

            }
            else
            {
                mancolorindex = _MemoryManagement.ReadData_int("ActiveManColor");
                ManColorText.text = _ItemInformations[ManColorArrayCalculation].ItemName;
                Material[] mats = _SkinnedMeshRenderer.materials;
                mats[0] = ManColorMaterials[mancolorindex];
                _SkinnedMeshRenderer.materials = mats;
                BuyButton.interactable = false;
                CustomizeSaveButton.interactable = true;
                BuyingText.text = "-";
            }
        }
    }


    /// <summary>
    /// E�er �tem Sat�n al�nd�ysa renk paletini a�ar ve aras�nda gezmeye izin verir e�er Item sat�n al�nmad�ysa renk paletlerini kapat�r
    /// </summary>
    /// <param name="PanelIndex">�apka veya Sopa panellerini a�mak i�in gerekli index</param>
    /// <param name="ItemArrayIndex">Gelen item indexiyle sat�n al�nm�� m� diye kontrol eder </param>
    private void ItemColorPanelControl(int PanelIndex,int ItemArrayIndex)
    {
        if (PanelIndex == 0)
        {
            if (ItemArrayIndex > -1)
            {
                if (_ItemInformations[ItemArrayIndex].BuyingStatus==true)
                {
                    PanelProcess[4].gameObject.SetActive(true);
                    PanelProcess[5].gameObject.SetActive(false);
                }
                else
                {
                    PanelProcess[4].gameObject.SetActive(false);
                    PanelProcess[5].gameObject.SetActive(false);
                }
            }
            else
            {
                PanelProcess[4].gameObject.SetActive(false);
                PanelProcess[5].gameObject.SetActive(false);
            }
        }
        else if (PanelIndex == 1)
        {
            if (ItemArrayIndex > -1)
            {
                if (_ItemInformations[ItemArrayIndex].BuyingStatus==true)
                {
                    PanelProcess[4].gameObject.SetActive(false);
                    PanelProcess[5].gameObject.SetActive(true);
                }
                else
                {
                    PanelProcess[4].gameObject.SetActive(false);
                    PanelProcess[5].gameObject.SetActive(false);
                }
            }
            else
            {
                PanelProcess[4].gameObject.SetActive(false);
                PanelProcess[5].gameObject.SetActive(false);
            }
        }
        else
        {
            PanelProcess[4].gameObject.SetActive (false);
            PanelProcess[5].gameObject.SetActive (false);
        }
    }
   
    /// <summary>
    /// Item renk indexlerini kay�t dosyas�ndan okur ve default index ise ona g�re renk verir default halde de�il ise okunan indexe g�re renk verme i�lemlerini yapar
    /// </summary>
    /// <param name="Index">Gelen indexe g�re kontrol i�lem yapar</param>
    private void ItemColorControl(int Index)
    {
        Color NewColor;
        if (Index==0)
        {
            if (_MemoryManagement.ReadData_int("ActiveHatColor")==-1)
            {
                HatColorRawImage.color = DefaultHatColorMaterial.color;
                HatColorMaterial.color = DefaultHatColorMaterial.color;
                if (hatcolorindex == HatColorName.Count - 1)
                {
                    HatColorButtons[1].interactable = false;
                }
                else if (hatcolorindex != -1)
                {
                    HatColorButtons[0].interactable = true;
                }
                else
                {
                    HatColorButtons[1].interactable = true;
                }
            }
            else
            {
                hatcolorindex = _MemoryManagement.ReadData_int("ActiveHatColor");
                if (ColorUtility.TryParseHtmlString(HatColorName[hatcolorindex], out NewColor))
                {
                    HatColorMaterial.color = NewColor;
                    HatColorRawImage.color = NewColor;
                }
            }
        }
        else
        {
            if (_MemoryManagement.ReadData_int("ActiveStickColor")==-1)
            {
                StickColorRawImage.color = DefaultStickColorMaterial.color;
                StickColorMaterial.color = DefaultStickColorMaterial.color;
                if (stickcolorindex == StickColorName.Count - 1)
                {
                    StickColorButtons[1].interactable = false;
                }
                else if (stickcolorindex != -1)
                {
                    StickColorButtons[0].interactable = true;
                }
                else
                {
                    StickColorButtons[1].interactable = true;
                }
            }
            else
            {
                stickcolorindex = _MemoryManagement.ReadData_int("ActiveStickColor");
                if (ColorUtility.TryParseHtmlString(StickColorName[stickcolorindex],out NewColor))
                {
                    StickColorRawImage.color= NewColor;
                    StickColorMaterial.color= NewColor;
                }
            }
        }
    }

    /// <summary>
    /// Gelen indexe g�re �tem say�n al�nm��m� al�nmam�� diye kontrol eder
    /// </summary>
    /// <param name="purchasecontrolindex">Hangi item kotnrol edilmek isteniyosa onun indexi verilir</param>
    private void PurchaseControl(int purchasecontrolindex)
    {
        if (!_ItemInformations[purchasecontrolindex].BuyingStatus)
        {
            if (_ItemInformations[purchasecontrolindex].Point>0)
            {
                //
                BuyingText.text = _ItemInformations[purchasecontrolindex].Point + _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeItemPurchaseControlText");
                BuyButton.interactable = true;
                CustomizeSaveButton.interactable = false;
            }           
        }
        else
        {
            BuyingText.text = "-";
            BuyButton.interactable = false;
            CustomizeSaveButton.interactable = true;
        }

    }

    /// <summary>
    /// Gelen i�leme g�re kullan�c�ya mesaj verir
    /// </summary>
    /// <param name="process">(0) sat�n alma mesaj�|(1) kaydetme mesaj�|(2) yetersizbakiye mesaj�</param>
    /// <returns></returns>
    IEnumerator ShowAlert(int process)
    {
        switch (process)
        {
            case 0:
                AlertText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeShowAlertPurchase");
                AlertText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                AlertText.gameObject.SetActive(false);
                break;
            case 1:
                AlertText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeShowAlertRecorded");
                AlertText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                AlertText.gameObject.SetActive(false);
                break;
            case 2:
                AlertText.text = _LanguageManager.BringText(LocalizationCustomizeTextTableName, "CustomizeShowAlertInsufficientFunds");
                AlertText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                AlertText.gameObject.SetActive(false);
                break;
        }
    }


    /// <summary>
    ///Gelen indexe G�re Sat�n alma i�lemini yapar puan d�zenlemesini yapar butonlar� kapat�r  puan textini g�nceller ve kullan�c�ya mesaj verir
    /// </summary>
    /// <param name="ArrayIndex">Dizi indexleri al�n�r</param>
    private void PurchasingHelper(int ArrayIndex)
    {
        _ItemInformations[ArrayIndex].BuyingStatus = true;
        _MemoryManagement.SaveData_int("Point", _MemoryManagement.ReadData_int("Point") - _ItemInformations[ArrayIndex].Point);
        BuyButton.interactable = false;
        BuyingText.text = "-";
        CustomizeSaveButton.interactable = true;
        PointText.text = _MemoryManagement.ReadData_int("Point").ToString();
        StartCoroutine(ShowAlert(0));
    }  
}