using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BackToMainMenu()//ana men� sahnesinin y�klenme i�lemleri
    {
        SceneManager.LoadScene(0);
    }
}
