using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rgame;
using rgamekeys;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static int LiveCharacterNum = 1;// anlýk yaþayan karakter sayýsý (ana karakterden dolayý 1 e eþittir)
    
    [Header("AltKarakterler")]
    [Tooltip("LowerCharacterPool daki alt karakterler verilecek")]
    public List<GameObject> LowerCharacters; //dýþarýdan karakterlerin verileceði liste
   
    [Header("Olusma Efekti")]
    [Tooltip("EffectPool daki formationeffectleri verilecek")]
    public List<GameObject> FormationEffect; // dýþarýdan oluþma efektlerinin verileceði liste
   
    [Header("Yok Olma Efekti")]
    [Tooltip("EffectPool daki extinctioneffectleri verilecek")]
    public List<GameObject> ExtinctionnEffect; // dýþarýdan yok olma efektlerinin verileceði liste
    
    [Header("Ezilme efekti")]
    [Tooltip("EffectPool daki manstains efectleri verilecek")]
    public List<GameObject> ManStainsEffect; //// dýþarýdan ezilme efektlerinin verileceði liste
    
    [Header("Ana Karakter")]
    [Tooltip("Main character verilecek")]
    public GameObject MainCharacter; //dýþarýdan ana karakterimizin verileceði obje
    
    HelperProcessLibrary _HelperLibrary = new HelperProcessLibrary();// sýnýf tanýmlamalarý
    MemoryManagement _MemoryManagement = new MemoryManagement();// sýnýf tanýmlamalarý

    [Header("Level Data")]//kolay yönetimi için oluþturulan baþlýk
    
    public List<GameObject> EnemyCharacter; // dýþarýdan düþman karakterlerin verileceði liste
   
    public int HowMuchEnemy; // kaç adet düþman var
   
    public bool IsGameOver; // oyun bitti mi
    
    bool ComeToEnd; // Sona geldik mi 
    
    [Header("Karakter Ýþlemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    [Header("Alt Karakter Ýþlemleri")]
    public bool LowerCharacterItem=false;

    //

    //
    [Header("PAUSE ISLEMLERI")]

    [Header("Pause Panel")]
    public GameObject[] Panels;
    [Tooltip("Pause Panelleri")]

    [Header("Pause Settings")]

    public Slider _MenuAudioSlider;
    [Tooltip("Pause Menu,Menu Audio Slider")]
    public Slider _MenuFxAudioSlider;
    [Tooltip("Pause Menu,Menu Fx Audio Slider")]
    public Slider _GameAudioSlider;
    [Tooltip("Pause Menu,Game Audio Slider")]

    ///Taglar
    private const string Attack = "Attack";
    ///Taglar

    Scene _Scene;

    /// <summary>
    /// sahne baþladýðýndan olmasý gereken Ýþlemler Düþman Karakterlerini aktif eder sahneyi yükler. Ses ayalarýný kayýt dosyasýndan okur ve getirir
    /// </summary>
    void Start()
    {
        EnemyCreate();
        _Scene=SceneManager.GetActiveScene();
        _MenuAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.MenuAudio);
        _MenuFxAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.MenuFxAudio);
        _GameAudioSlider.value = _MemoryManagement.ReadData_float(SaveKeys.GameAudio);
    }
   
    /// <summary>
    /// Düþman Karakterleri ortaya çýkarma iþlemi
    /// </summary>
    public void EnemyCreate()
    {
        for (int i = 0; i < HowMuchEnemy; i++)
        {
            EnemyCharacter[i].SetActive(true);
        }
    }

    /// <summary>
    /// Ana karakterden çoðalacak veya yok olacak alt karakterlerin iþlemleri(çarpma,bölme,toplama,çýkarma)
    /// </summary>
    /// <param name="processtype">Karakterin Hangi Isleme uðrayacaðýný belirler</param>
    /// <param name="incomingnum">Karakterin hangi sayýya çarpýp iþleme gireceiðini belirler</param>
    /// <param name="newposition">karakterin çýkmasý gereken pozisyon</param>
    public void ManManager(string processtype,int incomingnum, Transform newposition)
    {
        switch (processtype)
        {

            case GameCharactersProcess.MultiplacationProcess:
                _HelperLibrary.MultiplacationClass(incomingnum, LowerCharacters, newposition,FormationEffect);
                break;

            case GameCharactersProcess.ExtractionProcess:
                _HelperLibrary.ExtractionClass(incomingnum, LowerCharacters, ExtinctionnEffect);
                break;

            case GameCharactersProcess.DivisionProcess:
                _HelperLibrary.DivisionClass(incomingnum, LowerCharacters, ExtinctionnEffect);
                break;

            case GameCharactersProcess.CollectionProcess:
                _HelperLibrary.CollectionClass(incomingnum, LowerCharacters, newposition,FormationEffect);
                break;
        }
    }
   
    /// <summary>
    /// Savaþ Durumu fonksiyonu eðer sona gelindiyse Karakterlerin Attack animasyonunu aktif eder
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
                        item.GetComponent<Animator>().SetBool(Attack, false);
                    }
                }
                foreach (var item in LowerCharacters)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool(Attack, false);
                    }
                }
                MainCharacter.GetComponent<Animator>().SetBool(Attack, false);
                if (LiveCharacterNum < HowMuchEnemy || LiveCharacterNum == HowMuchEnemy)
                {
                    Panels[4].SetActive(true);
                    Debug.Log("lose");
                    _MemoryManagement.SaveData_int(SaveKeys.Point,99);
                }
                else
                {
                    Debug.Log("Win");
                    _MemoryManagement.SaveData_int(SaveKeys.Point, _MemoryManagement.ReadData_int(SaveKeys.Point) +799);
                    if (_Scene.buildIndex== _MemoryManagement.ReadData_int(SaveKeys.LastLevel))
                    {
                        _MemoryManagement.SaveData_int(SaveKeys.LastLevel,_MemoryManagement.ReadData_int(SaveKeys.LastLevel)+1);
                    }
                    Panels[3].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="effect_position">Efektin çýkmasý gerekn pozinyonu alýr</param>
    /// <param name="hammer">Çekiç Tarafýndan ezilen karakterlere farklý evet uygulamak için kontrol yapar</param>
    /// <param name="Status">Savaþ sýrasýnda eðer düþman karakter çaptý ise True deðeri gelir ve canlý karakter sayýsýný azaltýr eðer alt karakter çarptý ise false deðeri gelir ve Düþman karakter sayýsý azaltýlýr</param>
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
    /// Düþman karakterlere Saldýrma komutunu verir ve efektrini etkinleþtirir.Sona gelindi deðiþkenini true deðer döndürür.ve SAvaþ durumu deðiþkenini çaðýrýr
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
    /// Oyun Durunca açýlan duraklama menüsündeki islemleri kontrol eder 0=play||1=replay|2=settings|3=gotomainmenu|4=exit|5=next level| 6=Oyun durdurma |7= Ayarlar panelini kapatýr ve duraklama menüsünü açar
    /// </summary>
    /// <param name="process">Kullanýcýnýn Hangi Ýþlemi Yaptýðýnýn Bilgisini alýr</param>
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
            case 5:
                SceneManager.LoadScene(_Scene.buildIndex + 1);
                break;
            case 6:
                Time.timeScale = 0;
                Panels[0].gameObject.SetActive(true);
                break;
                case 7:
                Panels[1].gameObject.SetActive(false);
                Panels[0].gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// kullanýcýdan gelen cevaba göre oyunu kapatýr veya kapatmaz
    /// </summary>
    /// <param name="Question"> kullanýcýdan evet yada hayýr cevabýný alýr</param>
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
    /// duraklama menüsündeki ayarlar kýsmýndaki ses ayarlarýný yapmaya izin verir ve kaydeder
    /// </summary>
    /// <param name="process"> Slider dan gelen iþleme göre ses seviyesini ayarlar </param>
    public void AudioChange(int process)
    {
        switch (process)
        {
            case 0:
                _MemoryManagement.SaveData_float(SaveKeys.MenuAudio, _MenuAudioSlider.value);
                break;
            case 1:
                _MemoryManagement.SaveData_float(SaveKeys.MenuFxAudio, _MenuFxAudioSlider.value);
                break;
            case 2:
                _MemoryManagement.SaveData_float(SaveKeys.GameAudio, _GameAudioSlider.value);
                break;
        }
    }

}
