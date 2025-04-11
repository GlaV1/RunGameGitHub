using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public Animator _Animator;//d��ar�dan animat�r verilir
    public float WaitTime;//bekleme s�resi
    public Collider _PropallerWind;//d��ar�dan r�zgar�n geldi�i b�l�m�n collideri verilir(r�zgar alan�)
    ///Taglar
    private const string Run = "Run";//animatodeki run de�i�keni
    ///Taglar


    /// <summary>
    /// Fan durumunu kontrol eder ve Fan ile ilgili i�lemleri yapar
    /// </summary>
    /// <param name="fanstatus">Fan �al���yor mu �al��m�yor mu</param>
    public void AnimationStatus(string fanstatus)//animasyon durumu
    {
        if (fanstatus=="true")//e�er animasyon �al���yor ise
        {
            _Animator.SetBool(Run, true);//animatordeki Run fonksiyonuna true de�erinin g�nderir
            _PropallerWind.enabled = true;//r�zgar alan�n� aktif yapar
        }
        else
        {
            _Animator.SetBool(Run, false);//animatordeki Run fonksiyonuna false de�erinin g�nderir
            StartCoroutine(FanAnimationTrigger());//fan animasyonu tetikle
            _PropallerWind.enabled = false;//r�zgar alan�n� pasif yapar
        }
    }

    /// <summary>
    /// Fan animasyonunu Gelen istenilen bekleme s�resine g�re oynat�r
    /// </summary>
    /// <returns></returns>
    IEnumerator FanAnimationTrigger()//fan animasyonu tetikle
    {
        yield return new WaitForSeconds(WaitTime);//
        AnimationStatus("true");
    }
}
