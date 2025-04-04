using rgame;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class EmptyChracter : MonoBehaviour
{
    [Header("SkinMesh")]
    public SkinnedMeshRenderer _Renderer;
    [Header("NavMesh")]
    public NavMeshAgent _NavMesh; //boþ karaktere dýþarýdan verilecek ýolan nav mesh agent
    [Header("Animator")]
    public Animator _Animator; //dýþarýdan verilecek animatör
    [Header("Targer")]
    public GameObject Target;//dýþarýdan verilecek olan hedef
    bool isCharacterContact; //boþ karaktere temas varmý deðiþkeni
    [Header("GameManager")]
    public GameManager _GameManager;// gamemanergar scripti dýþarýdan verilir 

    [Header("Sapkalar")]
    public GameObject[] Hats;

    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi oluþturuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri oluþturuluyor
    public Material DefaultManColorMaterial;
    public Material _WillGivenMaterial;

    [Header("Karakter Ýþlemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;
    public double EmptyCharacterSpeed = 1.2;
    public Animator _EmptyCharacterAnimator;

    MemoryManagement _MemoryManagement = new MemoryManagement();
    //karaktere posizyon verme iþlemleri
    private void LateUpdate()
    {
        //eðer karakter ile temasa geçildi ise olmasý gerekenler
        if (isCharacterContact == true)
        {
            _NavMesh.SetDestination(Target.transform.position);//boþ karakterin nav mesh deðerler
        }
    }

    /// <summary>
    /// Boþ Karaktere Alt karakter veya Ana karakter çarptýðýnda Animasyonu çalýþtýrýr ve geri kalan iþlemleri yapar
    /// </summary>
    public void AnimationTrigger()
    {
        //karaktere dokunuldðunda materyal deðiþtirme iþlemleri
        Material[] material = _Renderer.materials;
        material[0] = _WillGivenMaterial;
        _Renderer.materials = material;
        _Animator.SetBool("Attack", true); //animatorde ki saldýr iþlemi aktif edilir
        gameObject.tag = "LowerCharacters";//boþ karakterin tagý alt karakter olarak deðiþtirilir
        GameManager.LiveCharacterNum++;//gamemanager scriptindeki anlýk karakter sayýsýnda +1 deðiþimi yapýlýr      
    }

    private void Awake()
    {
        _EmptyCharacterAnimator.speed = (float)(EmptyCharacterSpeed);
    }
    Vector3 GivePosition() // y eksen bileþeninde .23 deðer verir x ve z ekseninde deðiþklik yapýlmaz(efektler için)
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    /// <summary>
    /// Boþ karakter çarptýðýnda ve ya boþ karaktere çarpýldýðýnda  olmasý gereken iþlemler
    /// </summary>
    /// <param name="other">Boþ Karakter Collideri</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LowerCharacters")||other.CompareTag("Player"))//boþ karakterere alt karakter ve oyuncu karakterin çarptýðý zaman olacaklar
        {
            if (gameObject.CompareTag("EmptyCharacters"))//eðer çarpýlan objenin tagý boþ karaktere ise olacaklar
            {
                AnimationTrigger();//animasyon tetikleme methodunu çaðýrýr
                isCharacterContact = true;//karaktere temas var 
               // GetComponent<AudioSource>().Play();//ses dosyasý oynatma
            }
        }
        else if (other.CompareTag("EnemyCharacters"))//boþ karaktere düþman karakter çarparsa olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), false, false);//gamemanager scriptinden yok olma efektinin methodunu çaðýrýr ve gerekli deðerleri gönderir
            gameObject.SetActive(false);//objeyi aktifliðini kapatýr
        }
        else if (other.CompareTag("NeedleBox"))//boþ karakter iðneli kutuya çarparsa olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu çaðýrýr ve gerekli deðerleri gönderir
            gameObject.SetActive(false);//objeyi aktifliðini kapatýr
        }
        else if (other.CompareTag("Saw"))//boþ karakter testereye çarparsa olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu çaðýrýr ve gerekli deðerleri gönderir
            gameObject.SetActive(false);//objeyi aktifliðini kapatýr
        }
        else if (other.CompareTag("PropallerNeedle"))//boþ karakter pervane iðnesine çarparsa olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu çaðýrýr ve gerekli deðerleri gönderir
            gameObject.SetActive(false);//objeyi aktifliðini kapatýr
        }
        else if (other.CompareTag("Hammer"))//boþ karakter çekiç'e çarparsa olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), true);//gamemanager scriptinden yok olma efektinin methodunu çaðýrýr ve gerekli deðerleri gönderir
            gameObject.SetActive(false);//objeyi aktifliðini kapatýr
        }
    }

    private void ItemCheck()
    {
        if (_MemoryManagement.ReadData_int("ActiveHat")!=-1)
        {
            Hats[_MemoryManagement.ReadData_int("ActiveHat")].SetActive(true);
        }
        if (_MemoryManagement.ReadData_int("ActiveStick")!=-1)
        {
            Sticks[_MemoryManagement.ReadData_int("ActiveStick")].SetActive(true);
        }
        if (_MemoryManagement.ReadData_int("ActiveManColor") != -1)
        {
            Material[] mats = _SkinnedMeshRenderer.materials;
            mats[0] = ManColorMaterials[_MemoryManagement.ReadData_int("ActiveManColor")];
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
