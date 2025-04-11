using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Animator _Animator;//dýþarýdan animatör verilir
    public float WaitTime;//bekleme süresi
    public Collider _PropallerWind;//dýþarýdan rüzgarýn geldiði bölümün collideri verilir(rüzgar alaný)
    ///Taglar
    private const string Run = "Run";//animatodeki run deðiþkeni
    ///Taglar


    /// <summary>
    /// Fan durumunu kontrol eder ve Fan ile ilgili iþlemleri yapar
    /// </summary>
    /// <param name="fanstatus">Fan çalýþýyor mu çalýþmýyor mu</param>
    public void AnimationStatus(string fanstatus)//animasyon durumu
    {
        if (fanstatus=="true")//eðer animasyon çalýþýyor ise
        {
            _Animator.SetBool(Run, true);//animatordeki Run fonksiyonuna true deðerinin gönderir
            _PropallerWind.enabled = true;//rüzgar alanýný aktif yapar
        }
        else
        {
            _Animator.SetBool(Run, false);//animatordeki Run fonksiyonuna false deðerinin gönderir
            StartCoroutine(FanAnimationTrigger());//fan animasyonu tetikle
            _PropallerWind.enabled = false;//rüzgar alanýný pasif yapar
        }
    }

    /// <summary>
    /// Fan animasyonunu Gelen istenilen bekleme süresine göre oynatýr
    /// </summary>
    /// <returns></returns>
    IEnumerator FanAnimationTrigger()//fan animasyonu tetikle
    {
        yield return new WaitForSeconds(WaitTime);//
        AnimationStatus("true");
    }
}
