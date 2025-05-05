using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using rgame;
using rgamekeys;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine.Localization.SmartFormat.Utilities;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Butonlar")]
    [Tooltip("Level Butonlarý")]
    public Button[] Buttons;//dýþarýdan verilecek butonlar

    [Header("Istenen Yere Kadar level Açma islemi(5-20)")]
    public int Level;
    [Header("Kilit resmi")]
    [Tooltip("Kilit Sprite Verilecek")]
    public Sprite LockedButtons;//dýþarýdan verilen sprite

    [Header("Ara Sahne Manager")]
    [Tooltip("SceneLoadingImage itemi verilecek")]
    public SceneLoading sceneLoading;

    MemoryManagement _MemoryManagement= new MemoryManagement();
    void Start()
    {
        //_MemoryManagement.SaveData_int("LastLevel",Level);
        int presentlevel = _MemoryManagement.ReadData_int(SaveKeys.LastLevel) - 4;//mevcut level bellek yönetimi int veri okuma þeklyle alýnýr
        int index = 1;//index deðiþkenine 1 den baþlar
        for (int i = 0; i < Buttons.Length; i++)//döngü 0 dan baþlar ve button dizisini uzunluðu kadar döner
        {
            if (index<=presentlevel)//mevcut level index ten eþit ve büyükse olmasý gereken iþlemler
            {
                Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();//butonlarýn çocuk kompanentlerine eriþir ve text kýsmýna indexi yazdýrýr
                int sceneindex = index + 4;//sceneindex deðiþkenine index+4 iþlemi(düz index +4 yazýnca skýntý oluþuyor onun içn ayrý bi atama iþlemi yaptýk)
                Buttons[i].onClick.AddListener(delegate {UploadScene(sceneindex);});//butonlara týklama iþlemi atamasý yapýlýr ve sahne yükleme iþlemine index gönderilir
            }
            else//mevcut level indexten KÜÇÜK ise olmasý gerekenler
            {
                Buttons[i].GetComponentInChildren<Image>().sprite = LockedButtons;//butonlara sprite atama iþlemleri
                Buttons[i].enabled = false;//butonlarýn týklamalarýný kapatma iþlemi
            }
            index++; //index deðeri 1 arttýrma iþlemi
        }
    }
    /// <summary>
    /// Gelen index verisine göre sahne yükleme iþlemini yapar
    /// </summary>
    /// <param name="index">Ýndex verisi alýnýr</param>
    public void UploadScene(int index) //sahne yükleme iþlemleri
    {
        sceneLoading.LoadScene(index);
    }
    /// <summary>
    /// Ana Menüye dönme iþlemi
    /// </summary>
    public void BackToMainMenu()//ana menü sahnesinin yüklenme iþlemleri
    {
        SceneManager.LoadScene(0);
    }
}
