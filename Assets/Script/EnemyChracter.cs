using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChracter : MonoBehaviour
{
    public GameObject Attack_Target;//d��ar�dan sald�r� hedefi verilir
    public NavMeshAgent _NavMesh;//d��ar�dan nav mash agent verilir
    public Animator _Animator;//d��ar�dan animat�r verilir
    public GameManager _GameManager; // d��ar�dan gamemanager scripti �a��r�l�r
    bool AttackStarted;//sald�r� ba�lad� m�
    public void AnimationRun()//animasyon �al��t�rs
    {
        _Animator.SetBool("Attack",true);//d��man karakter animat�r�ndeki sald�r de�i�kenine true de�eri verilie
        AttackStarted = true;//atak ba�lad� m� evet
    }

    private void LateUpdate()
    {
        if (AttackStarted==true)//e�er sald�r� ba�lad� ise olmas� gerekenler
        {
            _NavMesh.SetDestination(Attack_Target.transform.position);//d��man karaktere sald�r� hedefi verilir
        }
    }

    //d��man karakter �arpt���nda �arp��p ge�ti�inde(tetikleme) yapt���nda olmas� gerekenler
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LowerCharacters"))//d��man karakter  alt karaktere �arpt���nda olmas� gerekenler
        {
            Vector3 newpos = new Vector3(transform.position.x, .23f, transform.position.z);//x ve z ekseni sabit olmak �zere y ekseninde de�i�iklik olmakla beraber yeni bir posizyon belirtilir
            _GameManager.ExtinctionnEffectRun(newpos,false,true);//gamemanager scriptinden yok olma efekti �al��t�r fonksiyonu �a��r�l�r
            gameObject.SetActive(false);//objenin aktifli�i kapat�l�r
        }
    }
}
