using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rgame;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static int LiveCharacterNum = 1;// anlýk yaþayan karakter sayýsý (ana karakterden dolayý 1 e eþittir)
    [Header("AltKarakterler")]
    public List<GameObject> LowerCharacters; //dýþarýdan karakterlerin verileceði liste
    public List<GameObject> FormationEffect; // dýþarýdan oluþma efektlerinin verileceði liste
    public List<GameObject> ExtinctionnEffect; // dýþarýdan yok olma efektlerinin verileceði liste
    public List<GameObject> ManStainsEffect; //// dýþarýdan ezilme efektlerinin verileceði liste
    public GameObject MainCharacter; //dýþarýdan ana karakterimizin verileceði obje
    HelperProcessLibrary _HelperLibrary = new HelperProcessLibrary();// sýnýf tanýmlamalarý
    MemoryManagement _MemoryManagement = new MemoryManagement();// sýnýf tanýmlamalarý

    [Header("Level Data")]//kolay yönetimi için oluþturulan baþlýk
    public List<GameObject> EnemyCharacter; // dýþarýdan düþman karakterlerin verileceði liste
    public int HowMuchEnemy; // kaç adet düþman var
    public bool IsGameOver; // oyun bitti mi
    bool ComeToEnd; // Sona geldik mi 
    
    [Header("Sapkalar")]
    public GameObject[] Hats;
    
    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi oluþturuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri oluþturuluyor
    public Material DefaultManColorMaterial;

    [Header("Karakter Ýþlemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    [Header("Alt Karakter Ýþlemleri")]
    public bool LowerCharacterItem=false;
    
    [Header("PAUSE ISLEMLERI")]

    [Header("Pause Panel")]
    public GameObject[] Panels;

    [Header("Pause Settings")]
    public Slider _MenuAudioSlider;
    public Slider _MenuFxAudioSlider;
    public Slider _GameAudioSlider;




    ////

    Scene _Scene;
    void Start()
    {
        EnemyCreate();
        _Scene=SceneManager.GetActiveScene();
        _MenuAudioSlider.value = _MemoryManagement.ReadData_float("MenuAudio");
        _MenuFxAudioSlider.value = _MemoryManagement.ReadData_float("MenuFxAudio");
        _GameAudioSlider.value = _MemoryManagement.ReadData_float("GameAudio");
    }
    private void Awake()
    {
      ItemCheck();
    }
    public void EnemyCreate()
    {
        for (int i = 0; i < HowMuchEnemy; i++)
        {
            EnemyCharacter[i].SetActive(true);
        }
    }

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
                    //kaybettin canvasý hazýrlanacak
                    Debug.Log("lose");
                    _MemoryManagement.SaveData_int("Point",99);
                }
                else
                {
                    //kazandýn canvasý hazýrlanacak
                    Debug.Log("Win");
                    _MemoryManagement.SaveData_int("Point", _MemoryManagement.ReadData_int("Point") +799);
                    if (_Scene.buildIndex== _MemoryManagement.ReadData_int("LastLevel"))
                    {
                        _MemoryManagement.SaveData_int("LastLevel",_MemoryManagement.ReadData_int("LastLevel")+1);
                    }
                    
                }
            }
        }
    }

    public void ExtinctionnEffectRun(Vector3 effect_position,bool hammer=false,bool Status=false)
    {
        foreach (var item in ExtinctionnEffect)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                item.transform.position = effect_position;
                item.GetComponent<ParticleSystem>().Play();
                item.GetComponent<AudioSource>().Play();
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

    public void EnemyTrigger()
    {
        foreach (var item in EnemyCharacter)
        {
            if (item.activeInHierarchy)
            {
                item.GetComponent<EnemyChracter>().AnimationRun();
            }
        }
        ComeToEnd = true;
        WarSituation();
    }
   
    public void ItemColorCheck()
    {
        Color NewColor;
        if (_MemoryManagement.ReadData_int("ActiveHatColor") !=-1)
        {
            if (ColorUtility.TryParseHtmlString(HatColorName[_MemoryManagement.ReadData_int("ActiveHatColor")], out NewColor))
            {
                HatColorMaterial.color = NewColor;
            }
        }
        else
        {
            HatColorMaterial.color = DefaultHatColorMaterial.color;
        }

        if (_MemoryManagement.ReadData_int("ActiveStickColor") != -1)
        {
            StickColorMaterial.color = DefaultStickColorMaterial.color;
        }
        else
        {
            if (ColorUtility.TryParseHtmlString(StickColorName[_MemoryManagement.ReadData_int("ActiveStickColor")], out NewColor))
            {
                StickColorMaterial.color = NewColor;
            }
        }
    }
    public void ItemCheck()
    {
        if (_MemoryManagement.ReadData_int("ActiveHat")!=-1)
        {
            Hats[_MemoryManagement.ReadData_int("ActiveHat")].SetActive(true);
        }

        if (_MemoryManagement.ReadData_int("ActiveStick") != -1)
        {
            Sticks[_MemoryManagement.ReadData_int("ActiveStick")].SetActive(true);
        }

        if (_MemoryManagement.ReadData_int("ActiveManColor") != -1)
        {
            Material[] mats=_SkinnedMeshRenderer.materials;
            mats[0]=ManColorMaterials[_MemoryManagement.ReadData_int("ActiveManColor")];
            _SkinnedMeshRenderer.materials = mats;
        }
        else
        {
            Material[] mats = _SkinnedMeshRenderer.materials;
            mats[0] = DefaultManColorMaterial;
            _SkinnedMeshRenderer.materials = mats;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Panels[0].gameObject.SetActive(true);
    }

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
                SceneManager.LoadScene(0);//Ana menü sahnesinin yüklenme iþlemi
                Time.timeScale = 1;
                break;
            case 4:
                foreach (var item in Panels)
                {
                    item.gameObject.SetActive(false);
                }
                Panels[2].gameObject.SetActive(true);
                break;
        }
    }

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

    public void BackToPauseMenu()
    {
        Panels[1].gameObject.SetActive(false);
        Panels[0].gameObject.SetActive(true);
    }

    public void AudioChange(int process)//process=0 MenuAudioChange|| process=1 MenuFxAudioChange || process=2 GameAudioChange
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
