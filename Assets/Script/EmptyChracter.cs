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
    public NavMeshAgent _NavMesh; //bo� karaktere d��ar�dan verilecek �olan nav mesh agent
    [Header("Animator")]
    public Animator _Animator; //d��ar�dan verilecek animat�r
    [Header("Targer")]
    public GameObject Target;//d��ar�dan verilecek olan hedef
    bool isCharacterContact; //bo� karaktere temas varm� de�i�keni
    [Header("GameManager")]
    public GameManager _GameManager;// gamemanergar scripti d��ar�dan verilir 

    [Header("Sapkalar")]
    public GameObject[] Hats;

    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi olu�turuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri olu�turuluyor
    public Material DefaultManColorMaterial;
    public Material _WillGivenMaterial;

    [Header("Karakter ��lemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;
    public double EmptyCharacterSpeed = 1.2;
    public Animator _EmptyCharacterAnimator;

    MemoryManagement _MemoryManagement = new MemoryManagement();
    //karaktere posizyon verme i�lemleri
    private void LateUpdate()
    {
        //e�er karakter ile temasa ge�ildi ise olmas� gerekenler
        if (isCharacterContact == true)
        {
            _NavMesh.SetDestination(Target.transform.position);//bo� karakterin nav mesh de�erler
        }
    }

    /// <summary>
    /// Bo� Karaktere Alt karakter veya Ana karakter �arpt���nda Animasyonu �al��t�r�r ve geri kalan i�lemleri yapar
    /// </summary>
    public void AnimationTrigger()
    {
        //karaktere dokunuld�unda materyal de�i�tirme i�lemleri
        Material[] material = _Renderer.materials;
        material[0] = _WillGivenMaterial;
        _Renderer.materials = material;
        _Animator.SetBool("Attack", true); //animatorde ki sald�r i�lemi aktif edilir
        gameObject.tag = "LowerCharacters";//bo� karakterin tag� alt karakter olarak de�i�tirilir
        GameManager.LiveCharacterNum++;//gamemanager scriptindeki anl�k karakter say�s�nda +1 de�i�imi yap�l�r      
    }

    private void Awake()
    {
        _EmptyCharacterAnimator.speed = (float)(EmptyCharacterSpeed);
    }
    Vector3 GivePosition() // y eksen bile�eninde .23 de�er verir x ve z ekseninde de�i�klik yap�lmaz(efektler i�in)
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    /// <summary>
    /// Bo� karakter �arpt���nda ve ya bo� karaktere �arp�ld���nda  olmas� gereken i�lemler
    /// </summary>
    /// <param name="other">Bo� Karakter Collideri</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LowerCharacters")||other.CompareTag("Player"))//bo� karakterere alt karakter ve oyuncu karakterin �arpt��� zaman olacaklar
        {
            if (gameObject.CompareTag("EmptyCharacters"))//e�er �arp�lan objenin tag� bo� karaktere ise olacaklar
            {
                AnimationTrigger();//animasyon tetikleme methodunu �a��r�r
                isCharacterContact = true;//karaktere temas var 
               // GetComponent<AudioSource>().Play();//ses dosyas� oynatma
            }
        }
        else if (other.CompareTag("EnemyCharacters"))//bo� karaktere d��man karakter �arparsa olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), false, false);//gamemanager scriptinden yok olma efektinin methodunu �a��r�r ve gerekli de�erleri g�nderir
            gameObject.SetActive(false);//objeyi aktifli�ini kapat�r
        }
        else if (other.CompareTag("NeedleBox"))//bo� karakter i�neli kutuya �arparsa olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu �a��r�r ve gerekli de�erleri g�nderir
            gameObject.SetActive(false);//objeyi aktifli�ini kapat�r
        }
        else if (other.CompareTag("Saw"))//bo� karakter testereye �arparsa olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu �a��r�r ve gerekli de�erleri g�nderir
            gameObject.SetActive(false);//objeyi aktifli�ini kapat�r
        }
        else if (other.CompareTag("PropallerNeedle"))//bo� karakter pervane i�nesine �arparsa olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektinin methodunu �a��r�r ve gerekli de�erleri g�nderir
            gameObject.SetActive(false);//objeyi aktifli�ini kapat�r
        }
        else if (other.CompareTag("Hammer"))//bo� karakter �eki�'e �arparsa olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), true);//gamemanager scriptinden yok olma efektinin methodunu �a��r�r ve gerekli de�erleri g�nderir
            gameObject.SetActive(false);//objeyi aktifli�ini kapat�r
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
