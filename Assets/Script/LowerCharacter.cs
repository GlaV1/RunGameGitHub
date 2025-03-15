using rgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LowerCharacter : MonoBehaviour
{
    public GameObject FinishPoint;//d��ar�dan alt karakterlerin gidece�i son nokta yani ana karakterin arkas�
    NavMeshAgent _Navmesh;//d��ar�dan navmesgagent veriyoruz
    public GameManager _GameManager;//d��ar�dan gamemanager scriptini veriyoruz
    
    [Header("Sapkalar")]
    public GameObject[] Hats;
    
    [Header("Sopalar")]
    public GameObject[] Sticks;//sopa dizisi olu�turuluyor

    [Header("Temalar")]
    public Material[] ManColorMaterials;//karakter materyalleri olu�turuluyor
    public Material DefaultManColorMaterial;

    [Header("Karakter ��lemleri")]
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
        _Navmesh = GetComponent<NavMeshAgent>();//kompanentlerden nav mese eri�iyoruz
    }

    private void LateUpdate()
    {
        _Navmesh.SetDestination(FinishPoint.transform.position);//alt karakterin gitmesi gereken yeri veriyoruz
    }
    //her yerde tek tek posizyon vermektense vermek istedi�imiz posizyonu bir metoda at�yoruz
    Vector3 GivePosition()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);//x ve z eksenlerindeki posizyon ayn� y ekseninde .23f lik bir posizyon
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NeedleBox"))//alt karakter i�neli kutuya �arpt���nda olmas� gerekenler
        {
            
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini �a��r�p posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if (other.CompareTag("Saw"))//alt karakter testereye �arpt���nda olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini �a��r�p posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("PropallerNeedle"))//alt karakter pervanenin i�nelerine �arpt���nda olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition());//gamemanager scriptinden yok olma efektini �a��r�p posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("Hammer"))//alt karakter �eki�e �arpt���n da olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), true);//gamemanager scriptinden yok olma efektini �a��r�p posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if(other.CompareTag("EnemyCharacters"))//alt karakter d��man karakterlere �arpt���nda olmas� gerekenler
        {
            _GameManager.ExtinctionnEffectRun(GivePosition(), false,false);//gamemanager scriptinden yok olma efektini �a��r�p posizyon veriyoruz
            gameObject.SetActive(false);//obejeyi pasif hale getirir
        }
        else if (other.CompareTag("EmptyCharacters"))//alt karakter bo� karaktere �arpt���nda olmas� gerekenler
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//�arpt��� objeyi gamemanager scriptindeki karakterler dizisine ekler
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
