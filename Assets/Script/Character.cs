using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    

    public GameManager _GameManager;//Oyun y�neticisi
    public CameraManagement _Camera;//kamera
    public bool ComeToEnd;//sona geldik mi kontrol yapan de�i�ken
    public GameObject WillGoWarCharacter;//karakterlerin gidece�i yer
    public Slider _Slider;//slider
    public GameObject EndGame;//finish line

    private void Start()
    {
        float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe de�i�kenine karaterin posizyponu ile biti� �izgisinin posizyonu aras�ndaki mesafeyi atar
        _Slider.maxValue = difference;//slider�n maksimum de�erine mesafe de�i�kenini atars
    }
    private void FixedUpdate()
    {
        //karkater sona gelmediyse olmas� gerekenler
        if (ComeToEnd==false)
        {
            //karakter zamanda .5f b�y�kl���nde ilerler
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);
        }
    }
    void Update()
    {
        if (Time.time != 0)
        {
            //karakter sona geldi ise olmas� gerekenler
            if (ComeToEnd == true)
            {
                //karakter biti� �izgisini ge�tikten sonra kullan�c�n�n konrol�nden ��kar ve sava� alan�n ortas�na otomatik gider
                transform.position = Vector3.Lerp(transform.position, WillGoWarCharacter.transform.position, .015f);
                //biti� �izgisini ge�tikten sonra slider da kalan bo�lu�u kapatmak i�in yap�lan i�lemler
                //e�er karakter biti� �igisini ge�tiyse ve slider�n de�eri 0 a e�it DE��LSE slider�n de�erini .005 azalt
                if (_Slider.value != 0)
                {
                    _Slider.value -= .005f;
                }
            }
            else//karakter sona gelmediyse olmas� gerekenler
            {
                float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe de�i�kenine biti� �izgisi ile karakterin aras�ndaki mesafeyi atar
                _Slider.value = difference;// kayd�r�c�n�n de�erine mesafayi atar

                if (Input.GetKey(KeyCode.Mouse0)) //mousenin sa� tu�una bas�ld���nda olmas� gerekenler
                {
                    //karakterin x bile�eninde sa�a ve sola haraket etmesini sa�layan ko�ullar
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z), .3f);
                    }
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z), .3f);
                    }
                }
            }
        }
    }


    //karakterin �arp�p ge�me tetikleme olaylar�
    private void OnTriggerEnter(Collider other)
    {
        //taglar� b�lme,toplama,�arpma,��karma i�lemleri olan nesneleri tetikler ise olacaklar
        if (other.CompareTag("DivisionProcess")|| other.CompareTag("CollectionProcess") || other.CompareTag("MultiplacationProcess") || other.CompareTag("ExtractionProcess"))
        {
            //gamemaner scriptindeki manmanager metoduna verileri g�nderir
            int num = int.Parse(other.name);
            _GameManager.ManManager(other.tag,num,other.transform);
        }
        else if (other.CompareTag("LastTrigger"))//karakterin �arpt��� tetikledi nesnenin tag� son tetkleyici ise olacaklar
        {
            _Camera.ComeToEnd= true;//kameran�n sona geldik mi de�i�keni true olur
            _GameManager.EnemyTrigger();//gamemanger scriptindeki d��man tetikleme metodu �al���r
            ComeToEnd = true;//karakterin sona geldik mi de�i�keni true olur
        }
        else if (other.CompareTag("EmptyCharacters"))//karakterin �arpt���,tetikledi�i nesnenin tag� bo� karakterler ise olacaklar
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//gamemanager scriptinde  ki karakterler dizisine ekleme yapar
        }
    }

    //�arpma ve kalma olaylar�
    private void OnCollisionEnter(Collision collision)
    {
        //direk,i�neli kutu,pervanenin i�ne tagl� objelere �arpt���nda yap�lmas� gereken i�lemler
        if (collision.gameObject.CompareTag("Mast")|| collision.gameObject.CompareTag("NeedleBox")||collision.gameObject.CompareTag("PropallerNeedle"))
        {
            //ekran�n sa� taraf�nda ise
            if (transform.position.x>0)
            {
                //bulundu�u posizyondan x bileieninde .2 birim kayd�rma yapar 
                transform.position = new Vector3(transform.position.x -.2f,transform.position.y,transform.position.z);
            }
            else //ekran�n sol taraf�nda ise
            {
                //bulundu�u posizyondan x bileieninde .2 birim kayd�rma yapar
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z);
            }
        }
    }

}
