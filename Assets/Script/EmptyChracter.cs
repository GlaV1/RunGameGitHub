using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class EmptyChracter : MonoBehaviour
{
    public SkinnedMeshRenderer _Renderer;
    public Material _WillGivenMaterial; // bo� karaktere d��ar�dan verilcecek materyal
    public NavMeshAgent _NavMesh; //bo� karaktere d��ar�dan verilecek �olan nav mesh agent
    public Animator _Animator; //d��ar�dan verilecek animat�r
    public GameObject Target;//d��ar�dan verilecek olan hedef
    bool CharacterContact; //bo� karaktere temas varm� de�i�keni
    public GameManager _GameManager;// gamemanergar scripti d��ar�dan verilir 

    //karaktere posizyon verme i�lemleri
    private void LateUpdate()
    {
        //e�er karakter ile temasa ge�ildi ise olmas� gerekenler
        if (CharacterContact==true)
        {
            _NavMesh.SetDestination(Target.transform.position);//bo� karakterin nav mesh de�erler
        }
    }

    //renk de�i�imi ve animasyon tetikleme i�lemleri
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

    Vector3 GivePosition() // y eksen bile�eninde .23 de�er verir x ve z ekseninde de�i�klik yap�lmaz
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    //�arpma �arp�p tetikleme i�lemleri
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LowerCharacters")||other.CompareTag("Player"))//bo� karakterere alt karakter ve oyuncu karakterin �arpt��� zaman olacaklar
        {
            if (gameObject.CompareTag("EmptyCharacters"))//e�er �arp�lan objenin tag� bo� karaktere ise olacaklar
            {
                AnimationTrigger();//animasyon tetikleme methodunu �a��r�r
                CharacterContact = true;//karaktere temas var 
                GetComponent<AudioSource>().Play();//ses dosyas� oynatma
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
}
