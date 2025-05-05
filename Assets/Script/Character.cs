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
    public GameManager _GameManager;//Oyun yöneticisi
    [Header("Kamera")]
    public CameraManagement _Camera;//kamera
    [Header("Sona Geldik mi")]
    public bool ComeToEnd;//sona geldik mi kontrol yapan deðiþken
    [Header("Savas Alani")]
    public GameObject WillGoWarCharacter;//karakterlerin gideceði yer
    [Header("Slider")]
    public Slider _GameMapSlider;//slider
    [Header("Bitis Cizgisi")]
    public GameObject EndGame;//finish line
    [Header("Ana Karakter Hýz")]
    public double MainCharacterSpeed = 1.2;
    [Header("Ana Karakter Animator")]
    public Animator _MainCharacterAnimator;
    [Header("Karakter Ýþlemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    [Header("Sapkalar")]
    public GameObject[] Hats;

    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi oluþturuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri oluþturuluyor
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
        float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe deðiþkenine karaterin posizyponu ile bitiþ çizgisinin posizyonu arasýndaki mesafeyi atar
        _GameMapSlider.maxValue = difference;//sliderýn maksimum deðerine mesafe deðiþkenini atars
    }
    private void FixedUpdate()
    {
        //karkater sona gelmediyse olmasý gerekenler
        if (ComeToEnd==false)
        {
            //karakter zamanda .5f büyüklüðünde ilerler
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);
        }
    }
    void Update()
    {
        if (Time.time != 0)
        {
            //karakter sona geldi ise olmasý gerekenler
            if (ComeToEnd == true)
            {
                //karakter bitiþ çizgisini geçtikten sonra kullanýcýnýn konrolünden çýkar ve savaþ alanýn ortasýna otomatik gider
                transform.position = Vector3.Lerp(transform.position, WillGoWarCharacter.transform.position, .015f);
                //bitiþ çizgisini geçtikten sonra slider da kalan boþluðu kapatmak için yapýlan iþlemler
                //eðer karakter bitiþ çigisini geçtiyse ve sliderýn deðeri 0 a eþit DEÐÝLSE sliderýn deðerini .005 azalt
                if (_GameMapSlider.value != 0)
                {
                    _GameMapSlider.value -= .005f;
                }
            }
            else//karakter sona gelmediyse olmasý gerekenler
            {
                float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe deðiþkenine bitiþ çizgisi ile karakterin arasýndaki mesafeyi atar
                _GameMapSlider.value = difference;// kaydýrýcýnýn deðerine mesafayi atar
    
                if (Input.GetKey(KeyCode.Mouse0)) //mousenin sað tuþuna basýldýðýnda olmasý gerekenler
                {
                    //karakterin x bileþeninde saða ve sola haraket etmesini saðlayan koþullar
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
    /// Karakter bir objeye çarptýðýnda veya Herhangi bir obje karaktere çarptýðýnda olmasý gereken iþlemler
    /// </summary>
    /// <param name="other">Karakterin Collideri</param>
    private void OnTriggerEnter(Collider other)
    {
        //taglarý bölme,toplama,çarpma,çýkarma iþlemleri olan nesneleri tetikler ise olacaklar
        if (other.CompareTag(GameCharactersProcess.DivisionProcess) || other.CompareTag(GameCharactersProcess.CollectionProcess) || other.CompareTag(GameCharactersProcess.MultiplacationProcess) || other.CompareTag(GameCharactersProcess.ExtractionProcess))
        {
            //gamemaner scriptindeki manmanager metoduna verileri gönderir
            int num = int.Parse(other.name);//çarptýðý nesnenin adýndaki sayýyý alýr
            _GameManager.ManManager(other.tag,num,other.transform);//manmanagera çarptýðý nesnenin tagýný, num deðiþkenini ve çarptýðý yerin pozisyonunu gönderir
        }
        else if (other.CompareTag(GameObstacles.LastTrigger))//karakterin çarptýðý tetikledi nesnenin tagý son tetkleyici ise olacaklar
        {
            _Camera.ComeToEnd= true;//kameranýn sona geldik mi deðiþkeni true olur
            _GameManager.EnemyTrigger();//gamemanger scriptindeki düþman tetikleme metodu çalýþýr
            ComeToEnd = true;//karakterin sona geldik mi deðiþkeni true olur
        }
        else if (other.CompareTag(GameCharacters.EmptyCharacters))//karakterin çarptýðý,tetiklediði nesnenin tagý boþ karakterler ise olacaklar
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//gamemanager scriptinde  ki karakterler dizisine ekleme yapar
        }
    }

   /// <summary>
   /// Ana karakterin collideri ve pervaneiðneleri,iðneli kutu ve direðe çarptýðýnda karakter takýlý kalmasýn diye ufak bir düzeltme(ekranýn sðýna veya solunda bulunmasýna göre)
   /// </summary>
   /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //direk,iðneli kutu,pervanenin iðne taglý objelere çarptýðýnda yapýlmasý gereken iþlemler
        if (collision.gameObject.CompareTag(GameObstacles.Mast) || collision.gameObject.CompareTag(GameObstacles.NeedleBox) ||collision.gameObject.CompareTag(GameObstacles.PropallerNeedle))
        {
            //ekranýn sað tarafýnda ise
            if (transform.position.x>0)
            {
                //bulunduðu posizyondan x bileieninde .2 birim kaydýrma yapar 
                transform.position = new Vector3(transform.position.x -.2f,transform.position.y,transform.position.z);
            }
            else //ekranýn sol tarafýnda ise
            {
                //bulunduðu posizyondan x bileieninde .2 birim kaydýrma yapar
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z);
            }
        }
    }

    /// <summary>
    /// Karakter özelleþitirme sayfasýnda Deðiþtirilen item renklerini kayýt dosyasýndan okur ve karkterlere uygular
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
    /// Customize Sayfasýnda özelleþtirilen karakterdek itemleri kayýt dosyasýndan okur ve karaktere özelleþtirmeyi uygular
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
