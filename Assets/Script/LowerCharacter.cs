using rgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LowerCharacter : MonoBehaviour
{
    public GameObject FinishPoint;//dýþarýdan alt karakterlerin gideceði son nokta yani ana karakterin arkasý
    NavMeshAgent _Navmesh;//dýþarýdan navmesgagent veriyoruz
    public GameManager _GameManager;//dýþarýdan gamemanager scriptini veriyoruz
    
    [Header("Sapkalar")]
    public GameObject[] Hats;
    
    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi oluþturuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri oluþturuluyor
    public Material DefaultManColorMaterial;

    [Header("Karakter Ýþlemleri")]
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    MemoryManagement _MemoryManagement= new MemoryManagement();

    private void Awake()
    {
        if (_GameManager.LowerCharacterItem==true)
        {
            ItemCheck();
        }
        
    }
    void Start()
    {
        _Navmesh = GetComponent<NavMeshAgent>();//kompanentlerden nav mese eriþiyoruz
    }

    private void LateUpdate()
    {
        _Navmesh.SetDestination(FinishPoint.transform.position);//alt karakterin gitmesi gereken yeri veriyoruz
    }
    //her yerde tek tek posizyon vermektense vermek istediðimiz posizyonu bir metoda atýyoruz
    Vector3 GivePosition()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);//x ve z eksenlerindeki posizyon ayný y ekseninde .23f lik bir posizyon
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NeedleBox"))//alt karakter iðneli kutuya çarptýðýnda olmasý gerekenler
        {
            
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini çaðýrýp posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if (other.CompareTag("Saw"))//alt karakter testereye çarptýðýnda olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini çaðýrýp posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("PropallerNeedle"))//alt karakter pervanenin iðnelerine çarptýðýnda olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini çaðýrýp posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("Hammer"))//alt karakter çekiçe çarptýðýn da olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), true);//gamemanager scriptinden yok olma efektini çaðýrýp posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("EnemyCharacters"))//alt karakter düþman karakterlere çarptýðýnda olmasý gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), false,false);//gamemanager scriptinden yok olma efektini çaðýrýp posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if (other.CompareTag("EmptyCharacters"))//alt karakter boþ karaktere çarptýðýnda olmasý gerekenler
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//çarptýðý objeyi gamemanager scriptindeki karakterler dizisine ekler
        }
    }

    public void ItemCheck()
    {
        if (_MemoryManagement.ReadData_int("ActiveHat") != -1)
        {
            Hats[_MemoryManagement.ReadData_int("ActiveHat")].SetActive(true);
        }

        if (_MemoryManagement.ReadData_int("ActiveStick") != -1)
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
