using rgame;
using rgamekeys;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    [Header("GameManager")]
    public GameManager _GameManager;//Oyun y�neticisi
    [Header("Kamera")]
    public CameraManagement _Camera;//kamera
    [Header("Sona Geldik mi")]
    public bool ComeToEnd;//sona geldik mi kontrol yapan de�i�ken
    [Header("Savas Alani")]
    public GameObject WillGoWarCharacter;//karakterlerin gidece�i yer
    [Header("Slider")]
    public Slider _GameMapSlider;//slider
    [Header("Bitis Cizgisi")]
    public GameObject EndGame;//finish line
    [Header("Ana Karakter H�z")]
    public double MainCharacterSpeed = 1.2;
    [Header("Ana Karakter Animator")]
    public Animator _MainCharacterAnimator;
    [Header("Karakter ��lemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    [Header("Sapkalar")]
    public GameObject[] Hats;

    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi olu�turuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri olu�turuluyor
    public Material DefaultManColorMaterial;

    [Header("ITEM ISLEMLERI")]

    [Header("Sapka renk islemleri")]
    public Material HatColorMaterial;
    public Material DefaultHatColorMaterial;
    [Header("Sopa renk islemleri")]
    public Material StickColorMaterial;
    public Material DefaultStickColorMaterial;

    MemoryManagement _MemoryManagement = new MemoryManagement();
    DataManager _DataManager = new DataManager();
    public List<ColorData> _HatColorName = new List<ColorData>();
    public List<ColorData> _StickColorName = new List<ColorData>();
    
    private void Start()
    {
        float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe de�i�kenine karaterin posizyponu ile biti� �izgisinin posizyonu aras�ndaki mesafeyi atar
        _GameMapSlider.maxValue = difference;//slider�n maksimum de�erine mesafe de�i�kenini atars
    }
    private void FixedUpdate()
    {
        //karkater sona gelmediyse olmas� gerekenler
        if (ComeToEnd==false)
        {
            //karakter zamanda .5f b�y�kl���nde ilerler
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);
        }
    }
    void Update()
    {
        if (Time.time != 0)
        {
            //karakter sona geldi ise olmas� gerekenler
            if (ComeToEnd == true)
            {
                //karakter biti� �izgisini ge�tikten sonra kullan�c�n�n konrol�nden ��kar ve sava� alan�n ortas�na otomatik gider
                transform.position = Vector3.Lerp(transform.position, WillGoWarCharacter.transform.position, .015f);
                //biti� �izgisini ge�tikten sonra slider da kalan bo�lu�u kapatmak i�in yap�lan i�lemler
                //e�er karakter biti� �igisini ge�tiyse ve slider�n de�eri 0 a e�it DE��LSE slider�n de�erini .005 azalt
                if (_GameMapSlider.value != 0)
                {
                    _GameMapSlider.value -= .005f;
                }
            }
            else//karakter sona gelmediyse olmas� gerekenler
            {
                float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe de�i�kenine biti� �izgisi ile karakterin aras�ndaki mesafeyi atar
                _GameMapSlider.value = difference;// kayd�r�c�n�n de�erine mesafayi atar
    
                if (Input.GetKey(KeyCode.Mouse0)) //mousenin sa� tu�una bas�ld���nda olmas� gerekenler
                {
                    //karakterin x bile�eninde sa�a ve sola haraket etmesini sa�layan ko�ullar
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z), .3f);
                    }
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z), .3f);
                    }
                }
            }
        }
    }

    private void Awake()
    {
        _DataManager.DataUpload();
        GameData loadedData = _DataManager.GetData();
        _HatColorName=loadedData._HatColorName;
        _StickColorName=loadedData._StickColorName;
        ItemCheck();
        //ItemColorCheck();
        _MainCharacterAnimator.speed = ((float)MainCharacterSpeed);
    }


    /// <summary>
    /// Karakter bir objeye �arpt���nda veya Herhangi bir obje karaktere �arpt���nda olmas� gereken i�lemler
    /// </summary>
    /// <param name="other">Karakterin Collideri</param>
    private void OnTriggerEnter(Collider other)
    {
        //taglar� b�lme,toplama,�arpma,��karma i�lemleri olan nesneleri tetikler ise olacaklar
        if (other.CompareTag(GameCharactersProcess.DivisionProcess) || other.CompareTag(GameCharactersProcess.CollectionProcess) || other.CompareTag(GameCharactersProcess.MultiplacationProcess) || other.CompareTag(GameCharactersProcess.ExtractionProcess))
        {
            //gamemaner scriptindeki manmanager metoduna verileri g�nderir
            int num = int.Parse(other.name);//�arpt��� nesnenin ad�ndaki say�y� al�r
            _GameManager.ManManager(other.tag,num,other.transform);//manmanagera �arpt��� nesnenin tag�n�, num de�i�kenini ve �arpt��� yerin pozisyonunu g�nderir
        }
        else if (other.CompareTag(GameObstacles.LastTrigger))//karakterin �arpt��� tetikledi nesnenin tag� son tetkleyici ise olacaklar
        {
            _Camera.ComeToEnd= true;//kameran�n sona geldik mi de�i�keni true olur
            _GameManager.EnemyTrigger();//gamemanger scriptindeki d��man tetikleme metodu �al���r
            ComeToEnd = true;//karakterin sona geldik mi de�i�keni true olur
        }
        else if (other.CompareTag(GameCharacters.EmptyCharacters))//karakterin �arpt���,tetikledi�i nesnenin tag� bo� karakterler ise olacaklar
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//gamemanager scriptinde  ki karakterler dizisine ekleme yapar
        }
    }

   /// <summary>
   /// Ana karakterin collideri ve pervanei�neleri,i�neli kutu ve dire�e �arpt���nda karakter tak�l� kalmas�n diye ufak bir d�zeltme(ekran�n s��na veya solunda bulunmas�na g�re)
   /// </summary>
   /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //direk,i�neli kutu,pervanenin i�ne tagl� objelere �arpt���nda yap�lmas� gereken i�lemler
        if (collision.gameObject.CompareTag(GameObstacles.Mast) || collision.gameObject.CompareTag(GameObstacles.NeedleBox) ||collision.gameObject.CompareTag(GameObstacles.PropallerNeedle))
        {
            //ekran�n sa� taraf�nda ise
            if (transform.position.x>0)
            {
                //bulundu�u posizyondan x bileieninde .2 birim kayd�rma yapar 
                transform.position = new Vector3(transform.position.x -.2f,transform.position.y,transform.position.z);
            }
            else //ekran�n sol taraf�nda ise
            {
                //bulundu�u posizyondan x bileieninde .2 birim kayd�rma yapar
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z);
            }
        }
    }

    /// <summary>
    /// Karakter �zelle�itirme sayfas�nda De�i�tirilen item renklerini kay�t dosyas�ndan okur ve karkterlere uygular
    /// </summary>
    //private void ItemColorCheck()
    //{
    //    Color NewColor;
    //    if (_MemoryManagement.ReadData_int(SaveKeys.ActiveHatColor) != -1)
    //    {
    //        if (ColorUtility.TryParseHtmlString(_HatColorName[_MemoryManagement.ReadData_int(SaveKeys.ActiveHatColor)].ColorName, out NewColor))
    //        {
    //            HatColorMaterial.color = NewColor;
    //        }
    //    }
    //    else
    //    {
    //        HatColorMaterial.color = DefaultHatColorMaterial.color;
    //    }

    //    if (_MemoryManagement.ReadData_int(SaveKeys.ActiveStickColor) != -1)
    //    {
    //        StickColorMaterial.color = DefaultStickColorMaterial.color;
    //    }
    //    else
    //    {
    //        if (ColorUtility.TryParseHtmlString(_StickColorName[_MemoryManagement.ReadData_int(SaveKeys.ActiveStickColor)].ColorName, out NewColor))
    //        {
    //            StickColorMaterial.color = NewColor;
    //        }
    //    }
    //}  

    /// <summary>
    /// Customize Sayfas�nda �zelle�tirilen karakterdek itemleri kay�t dosyas�ndan okur ve karaktere �zelle�tirmeyi uygular
    /// </summary>
    private void ItemCheck()
    {
        if (_MemoryManagement.ReadData_int(SaveKeys.ActiveHat) != -1)
        {
            Hats[_MemoryManagement.ReadData_int(SaveKeys.ActiveHat)].SetActive(true);
        }

        if (_MemoryManagement.ReadData_int(SaveKeys.ActiveStick) != -1)
        {
            Sticks[_MemoryManagement.ReadData_int(SaveKeys.ActiveStick)].SetActive(true);
        }

        if (_MemoryManagement.ReadData_int(SaveKeys.ActiveManColor) != -1)
        {
            Material[] mats = _SkinnedMeshRenderer.materials;
            mats[0] = ManColorMaterials[_MemoryManagement.ReadData_int(SaveKeys.ActiveManColor)];
            _SkinnedMeshRenderer.materials = mats;
        }
        else
        {
            Material[] mats = _SkinnedMeshRenderer.materials;
            mats[0] = DefaultManColorMaterial;
            _SkinnedMeshRenderer.materials = mats;
        }
    }
}
