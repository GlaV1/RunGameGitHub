using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketManager : MonoBehaviour
{
    /// <summary>
    /// Ana menuye donme islemleri
    /// </summary>
    public void BackToMainMenu()//ana menü sahnesinin yüklenme iþlemleri
    {
        SceneManager.LoadScene(0);
    }
}
