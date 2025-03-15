using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using rgame;

public class MainMenuManager : MonoBehaviour
{

    MemoryManagement _MemoryManagement = new MemoryManagement();
    public List<ItemInformations> _ItemInformations= new List<ItemInformations>();
    DataManager _DataManager = new DataManager();
    void Start()
    {
        _MemoryManagement.Check();
        _DataManager.FirstSave(_ItemInformations);
    }

    void Update()
    {
        
    }

    public void UploadScene(int index) //sahne yükleme isþlemleri
    {
        SceneManager.LoadScene(index);
    }

    public void Play() //oyna butonu iþlemleri
    {
        SceneManager.LoadScene(_MemoryManagement.ReadData_int("LastLevel"));//sahne yüklemesinin yapýldýðý iþlem
    }
}
