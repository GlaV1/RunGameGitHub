using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rgame;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static int LiveCharacterNum = 1;// anl�k ya�ayan karakter say�s� (ana karakterden dolay� 1 e e�ittir)
    [Header("AltKarakterler")]
    public List<GameObject> LowerCharacters; //d��ar�dan karakterlerin verilece�i liste
    [Header("Olusma Efekti")]
    public List<GameObject> FormationEffect; // d��ar�dan olu�ma efektlerinin verilece�i liste
    [Header("Yok Olma Efekti")]
    public List<GameObject> ExtinctionnEffect; // d��ar�dan yok olma efektlerinin verilece�i liste
    [Header("Ezilme efekti")]
    public List<GameObject> ManStainsEffect; //// d��ar�dan ezilme efektlerinin verilece�i liste
    [Header("Ana Karakter")]
    public GameObject MainCharacter; //d��ar�dan ana karakterimizin verilece�i obje
    HelperProcessLibrary _HelperLibrary = new HelperProcessLibrary();// s�n�f tan�mlamalar�
    MemoryManagement _MemoryManagement = new MemoryManagement();// s�n�f tan�mlamalar�

    [Header("Level Data")]//kolay y�netimi i�in olu�turulan ba�l�k
    public List<GameObject> EnemyCharacter; // d��ar�dan d��man karakterlerin verilece�i liste
    public int HowMuchEnemy; // ka� adet d��man var
    public bool IsGameOver; // oyun bitti mi
    bool ComeToEnd; // Sona geldik mi 
    
    [Header("Karakter ��lemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    [Header("Alt Karakter ��lemleri")]
    public bool LowerCharacterItem=false;

    //
    
    //
    [Header("PAUSE ISLEMLERI")]

    [Header("Pause Panel")]
    public GameObject[] Panels;

    [Header("Pause Settings")]
    public Slider _MenuAudioSlider;
    public Slider _MenuFxAudioSlider;
    public Slider _GameAudioSlider;

    ////

    Scene _Scene;

    /// <summary>
    /// sahne ba�lad���ndan olmas� gereken ��lemler D��man Karakterlerini aktif eder sahneyi y�kler. Ses ayalar�n� kay�t dosyas�ndan okur ve getirir
    /// </summary>
    void Start()
    {
        EnemyCreate();
        _Scene=SceneManager.GetActiveScene();
        _MenuAudioSlider.value = _MemoryManagement.ReadData_float("MenuAudio");
        _MenuFxAudioSlider.value = _MemoryManagement.ReadData_float("MenuFxAudio");
        _GameAudioSlider.value = _MemoryManagement.ReadData_float("GameAudio");
    }
   
    /// <summary>
    /// Sahne y�klenmeden �nce olmas� gerekenler
    /// </summary>
    private void Awake()
    {
    }
    /// <summary>
    /// D��man Karakterleri ortaya ��karma i�lemi
    /// </summary>
    public void EnemyCreate()
    {
        for (int i = 0; i < HowMuchEnemy; i++)
        {
            EnemyCharacter[i].SetActive(true);
        }
    }

    /// <summary>
    /// Ana karakterden �o�alacak veya yok olacak alt karakterlerin i�lemleri(�arpma,b�lme,toplama,��karma)
    /// </summary>
    /// <param name="processtype">Karakterin Hangi Isleme u�rayaca��n� belirler</param>
    /// <param name="incomingnum">Karakterin hangi say�ya �arp�p i�leme girecei�ini belirler</param>
    /// <param name="newposition">karakterin ��kmas� gereken pozisyon</param>
    public void ManManager(string processtype,int incomingnum, Transform newposition)
    {
        switch (processtype)
        {

            case "MultiplacationProcess":
                _HelperLibrary.MultiplacationClass(incomingnum, LowerCharacters, newposition,FormationEffect);
                break;

            case "ExtractionProcess":
                _HelperLibrary.ExtractionClass(incomingnum, LowerCharacters, ExtinctionnEffect);
                break;

            case "DivisionProcess":
                _HelperLibrary.DivisionClass(incomingnum, LowerCharacters, ExtinctionnEffect);
                break;

            case "CollectionProcess":
                _HelperLibrary.CollectionClass(incomingnum, LowerCharacters, newposition,FormationEffect);
                break;
        }
    }
   
    /// <summary>
    /// Sava� Durumu fonksiyonu e�er sona gelindiyse Karakterlerin Attack animasyonunu aktif eder
    /// </summary>
    void WarSituation()
    {
        if (ComeToEnd==true)
        {
            if (LiveCharacterNum == 1 || HowMuchEnemy == 0)
            {
                IsGameOver = true;
                foreach (var item in EnemyCharacter)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Attack", false);
                    }
                }
                foreach (var item in LowerCharacters)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Attack", false);
                    }
                }
                MainCharacter.GetComponent<Animator>().SetBool("Attack", false);
                if (LiveCharacterNum < HowMuchEnemy || LiveCharacterNum == HowMuchEnemy)
                {
                    Panels[4].SetActive(true);
                    Debug.Log("lose");
                    _MemoryManagement.SaveData_int("Point",99);
                }
                else
                {
                    Debug.Log("Win");
                    _MemoryManagement.SaveData_int("Point", _MemoryManagement.ReadData_int("Point") +799);
                    if (_Scene.buildIndex== _MemoryManagement.ReadData_int("LastLevel"))
                    {
                        _MemoryManagement.SaveData_int("LastLevel",_MemoryManagement.ReadData_int("LastLevel")+1);
                    }
                    Panels[3].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="effect_position">Efektin ��kmas� gerekn pozinyonu al�r</param>
    /// <param name="hammer">�eki� Taraf�ndan ezilen karakterlere farkl� evet uygulamak i�in kontrol yapar</param>
    /// <param name="Status">Sava� s�ras�nda e�er d��man karakter �apt� ise True de�eri gelir ve canl� karakter say�s�n� azalt�r e�er alt karakter �arpt� ise false de�eri gelir ve D��man karakter say�s� azalt�l�r</param>
    public void ExtinctionnEffectRun(Vector3 effect_position,bool hammer=false,bool Status=false)
    {
        foreach (var item in ExtinctionnEffect)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                item.transform.position = effect_position;
                item.GetComponent<ParticleSystem>().Play();
               // item.GetComponent<AudioSource>().Play();
                if (Status==false)
                {
                    LiveCharacterNum--;
                }
                else
                {
                    HowMuchEnemy--;
                }
                break;
            }
        }
        
        if (hammer==true) 
        {
            Vector3 newpos=new Vector3(effect_position.x,.0005f,effect_position.z);
            foreach (var item in ManStainsEffect)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                    item.transform.position = newpos;
                    break;
                }
            }
        }

        if (IsGameOver==false)
        {
            WarSituation();
        }
    }

    /// <summary>
    /// D��man karakterlere Sald�rma komutunu verir ve efektrini etkinle�tirir.Sona gelindi de�i�kenini true de�er d�nd�r�r.ve SAva� durumu de�i�kenini �a��r�r
    /// </summary>
    public void EnemyTrigger()
    {
        foreach (var item in EnemyCharacter)
        {
            if (item.activeInHierarchy)
            {
                item.GetComponent<EnemyCharacter>().AnimationRun();
            }
        }
        ComeToEnd = true;
        WarSituation();
    }
   

    /// <summary>
    /// Pause d��meseine bas�l�nca Oyunu Durdurur
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        Panels[0].gameObject.SetActive(true);
    }

    /// <summary>
    /// Oyun Durunca a��lan duraklama men�s�ndeki islemleri kontrol eder 0=play||1=replay|2=settings|3=gotomainmenu|4=exit|5=next level
    /// </summary>
    /// <param name="process">Kullan�c�n�n Hangi ��lemi Yapt���n�n Bilgisini al�r</param>
    public void PauseProcess(int process)
    {
        switch (process)
        {
            case 0:
                Time.timeScale = 1;
                foreach (var item in Panels)
                {
                    item.gameObject.SetActive(false);
                }
                break;
            case 1:
                SceneManager.LoadScene(_Scene.buildIndex);
                Time.timeScale = 1;
                break;
            case 2:
                foreach (var item in Panels)
                {
                    item.gameObject.SetActive(false);
                }
                Panels[1].gameObject.SetActive(true);
                break;
            case 3:
                SceneManager.LoadScene(0);//Ana men� sahnesinin y�klenme i�lemi
                Time.timeScale = 1;
                break;
            case 4:
                foreach (var item in Panels)
                {
                    item.gameObject.SetActive(false);
                }
                Panels[2].gameObject.SetActive(true);
                break;
            case 5:
                SceneManager.LoadScene(_Scene.buildIndex + 1);
                break;
        }
    }

    /// <summary>
    /// kullan�c�dan gelen cevaba g�re oyunu kapat�r veya kapatmaz
    /// </summary>
    /// <param name="Question"> kullan�c�dan evet yada hay�r cevab�n� al�r</param>
    public void ExitQuestion(string Question)
    {
        if (Question=="Yes")
        {
            Application.Quit();
        }
        else
        {
            Panels[2].gameObject.SetActive(false);
            Panels[0].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Ayarlar panelini kapat�r ve duraklama men�s�n� a�ar
    /// </summary>
    public void BackToPauseMenu()
    {
        Panels[1].gameObject.SetActive(false);
        Panels[0].gameObject.SetActive(true);
    }

    /// <summary>
    /// duraklama men�s�ndeki ayarlar k�sm�ndaki ses ayarlar�n� yapmaya izin verir ve kaydeder
    /// </summary>
    /// <param name="process"> Slider dan gelen i�leme g�re ses seviyesini ayarlar </param>
    public void AudioChange(int process)
    {
        switch (process)
        {
            case 0:
                _MemoryManagement.SaveData_float("MenuAudio", _MenuAudioSlider.value);
                break;
            case 1:
                _MemoryManagement.SaveData_float("MenuFxAudio", _MenuFxAudioSlider.value);
                break;
            case 2:
                _MemoryManagement.SaveData_float("GameAudio", _GameAudioSlider.value);
                break;
        }
    }

}
