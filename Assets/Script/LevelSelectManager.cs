using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using rgame;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{

    public Button[] Buttons;//dýþarýdan verilecek butonlar
    public int Level;
    public Sprite LockedButtons;//dýþarýdan verilen sprite

    MemoryManagement _MemoryManagement= new MemoryManagement();
    void Start()
    {
        int presentlevel = _MemoryManagement.ReadData_int("LastLevel") - 4;//mevcut level bellek yönetimi int veri okuma þeklyle alýnýr
        int index = 1;//index deðiþkenine 1 den baþlar
        for (int i = 0; i < Buttons.Length; i++)//döngü 0 dan baþlar ve button dizisini uzunluðu kadar döner
        {
            if (index<=presentlevel)//mevcut level index ten eþit ve büyükse olmasý gereken iþlemler
            {
                Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();//butonlarýn çocuk kompanentlerine eriþir ve text kýsmýna indexi yazdýrýr
                int sceneindex = index + 4;//sceneindex deðiþkenine index+4 iþlemi(düz index +4 yazýnca skýntý oluþuyor onun içn ayrý bi atama iþlemi yaptýk)
                Buttons[i].onClick.AddListener(delegate {UploadScene(sceneindex);});//butonlara týklama iþlemi atamasý yapýlýr ve sahne yükleme iþlemine index gönderilir
            }
            else//mevcut level indexten BÜYÜK ise olmasý gerekenler
            {
                Buttons[i].GetComponentInChildren<Image>().sprite = LockedButtons;//butonlara sprite atama iþlemleri
                Buttons[i].enabled = false;//butonlarýn týklamalarýný kapatma iþlemi
            }
            index++; //index deðeri 1 arttýrma iþlemi
        }
    }
    public void UploadScene(int index) //sahne yükleme iþlemleri
    {
        SceneManager.LoadScene(index);
    }

    public void BackToMainMenu()//ana menü sahnesinin yüklenme iþlemleri
    {
        SceneManager.LoadScene(0);
    }
}
