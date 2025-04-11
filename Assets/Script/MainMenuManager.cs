using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using rgame;
using rgamekeys;

public class MainMenuManager : MonoBehaviour
{

    
    [Header("Item Islemleri")]
    public List<ItemInformations> _ItemInformations= new List<ItemInformations>();
    [Header("Sapka Renk Islemleri")]
    public List<ColorData> _HatColorName = new List<ColorData>();
    [Header("Sopa Renk Islemleri")]
    public List<ColorData> _StickColorName = new List<ColorData>();

    DataManager _DataManager = new DataManager();
    GameData _GameData = new GameData();
    MemoryManagement _MemoryManagement = new MemoryManagement();
    public SceneLoading sceneLoading;  

    void Start()
    {
        _GameData._ItemInformation = _ItemInformations;
        _GameData._StickColorName = _StickColorName;
        _GameData._HatColorName = _HatColorName;
        _MemoryManagement.Check();
        _DataManager.FirstSave(_GameData);
    }


    public void UploadScene(int index) //sahne yükleme isþlemleri
    {
        sceneLoading.LoadScene(index);
    }

    public void Play() //oyna butonu iþlemleri
    {
        sceneLoading.LoadScene(_MemoryManagement.ReadData_int(SaveKeys.LastLevel));  
    }

}
