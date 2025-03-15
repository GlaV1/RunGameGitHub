using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using rgame;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{

    public Button[] Buttons;//d��ar�dan verilecek butonlar
    public int Level;
    public Sprite LockedButtons;//d��ar�dan verilen sprite

    MemoryManagement _MemoryManagement= new MemoryManagement();
    void Start()
    {
        int presentlevel = _MemoryManagement.ReadData_int("LastLevel") - 4;//mevcut level bellek y�netimi int veri okuma �eklyle al�n�r
        int index = 1;//index de�i�kenine 1 den ba�lar
        for (int i = 0; i < Buttons.Length; i++)//d�ng� 0 dan ba�lar ve button dizisini uzunlu�u kadar d�ner
        {
            if (index<=presentlevel)//mevcut level index ten e�it ve b�y�kse olmas� gereken i�lemler
            {
                Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();//butonlar�n �ocuk kompanentlerine eri�ir ve text k�sm�na indexi yazd�r�r
                int sceneindex = index + 4;//sceneindex de�i�kenine index+4 i�lemi(d�z index +4 yaz�nca sk�nt� olu�uyor onun i�n ayr� bi atama i�lemi yapt�k)
                Buttons[i].onClick.AddListener(delegate {UploadScene(sceneindex);});//butonlara t�klama i�lemi atamas� yap�l�r ve sahne y�kleme i�lemine index g�nderilir
            }
            else//mevcut level indexten B�Y�K ise olmas� gerekenler
            {
                Buttons[i].GetComponentInChildren<Image>().sprite = LockedButtons;//butonlara sprite atama i�lemleri
                Buttons[i].enabled = false;//butonlar�n t�klamalar�n� kapatma i�lemi
            }
            index++; //index de�eri 1 artt�rma i�lemi
        }
    }
    public void UploadScene(int index) //sahne y�kleme i�lemleri
    {
        SceneManager.LoadScene(index);
    }

    public void BackToMainMenu()//ana men� sahnesinin y�klenme i�lemleri
    {
        SceneManager.LoadScene(0);
    }
}
