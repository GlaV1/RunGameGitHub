using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using rgame;

public class MainMenuManager : MonoBehaviour
{

    
    [Header("Item Islemleri")]
    public List<ItemInformations> _ItemInformations= new List<ItemInformations>();

    DataManager _DataManager = new DataManager();
    MemoryManagement _MemoryManagement = new MemoryManagement();
    public SceneLoading sceneLoading;  

    void Start()
    {
        _MemoryManagement.Check();
        _DataManager.FirstSave(_ItemInformations);
       
    }


    public void UploadScene(int index) //sahne yükleme isþlemleri
    {
        // SceneManager.LoadScene(index);
        sceneLoading.LoadScene(index);
    }

    public void Play() //oyna butonu iþlemleri
    {
       // StartCoroutine(LoadAsync(_MemoryManagement.ReadData_int("LastLevel")));
    }

   
}
