using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Animator _Animator;//dýþarýdan animatör verilir
    public float WaitTime;//bekleme süresi
    public Collider _PropallerWind;//dýþarýdan rüzgarýn geldiði bölümün collideri verilir(rüzgar alaný)

    //animatörde baþlangýçta Run fonksiyonun deðeri TRUE dur
    public void AnimationStatus(string fanstatus)//animasyon durumu
    {
        if (fanstatus=="true")//eðer animasyon çalýþýyor ise
        {
            _Animator.SetBool("Run", true);//animatordeki Run fonksiyonuna true deðerinin gönderir
            _PropallerWind.enabled = true;//rüzgar alanýný aktif yapar
        }
        else
        {
            _Animator.SetBool("Run",false);//animatordeki Run fonksiyonuna false deðerinin gönderir
            StartCoroutine(FanAnimationTrigger());//fan animasyonu tetikle
            _PropallerWind.enabled = false;//rüzgar alanýný pasif yapar
        }
    }

    IEnumerator FanAnimationTrigger()//fan animasyonu tetikle
    {
        yield return new WaitForSeconds(WaitTime);//
        AnimationStatus("true");
    }
}
