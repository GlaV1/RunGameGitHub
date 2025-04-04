using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : MonoBehaviour
{
    [Header("Saldiri Hedefi")]
    public GameObject Attack_Target;//dýþarýdan saldýrý hedefi verilir
    [Header("NavMesh")]
    public NavMeshAgent _NavMesh;//dýþarýdan nav mash agent verilir
    [Header("Animator")]
    public Animator _Animator;//dýþarýdan animatör verilir
    [Header("GameManager")]
    public GameManager _GameManager; // dýþarýdan gamemanager scripti çaðýrýlýr
    bool AttackStarted;//saldýrý baþladý mý
    
    /// <summary>
    /// Düþman Karakterin Saldýrma efektini aktif eder
    /// </summary>
    public void AnimationRun()//animasyon çalýþtýrs
    {
        _Animator.SetBool("Attack",true);//düþman karakter animatöründeki saldýr deðiþkenine true deðeri verilie
        AttackStarted = true;//atak baþladý mý evet
    }


    private void LateUpdate()
    {
        if (AttackStarted==true)//eðer saldýrý baþladý ise olmasý gerekenler
        {
            _NavMesh.SetDestination(Attack_Target.transform.position);//düþman karaktere saldýrý hedefi verilir
        }
    }

    /// <summary>
    /// Düþman Karaktere, Alt Karater çaptýysa olmasý gerekenler
    /// </summary>
    /// <param name="other">Düþman karakterin collideri</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LowerCharacters"))//düþman karakter  alt karaktere çarptýðýnda olmasý gerekenler
        {
            Vector3 newpos = new Vector3(transform.position.x, .23f, transform.position.z);//x ve z ekseni sabit olmak üzere y ekseninde deðiþiklik olmakla beraber yeni bir posizyon belirtilir
            _GameManager.ExtinctionnEffectRun(newpos,false,true);//gamemanager scriptinden yok olma efekti çalýþtýr fonksiyonu çaðýrýlýr
            gameObject.SetActive(false);//objenin aktifliði kapatýlýr
        }
    }
}
