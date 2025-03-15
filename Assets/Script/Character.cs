using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    

    public GameManager _GameManager;//Oyun yöneticisi
    public CameraManagement _Camera;//kamera
    public bool ComeToEnd;//sona geldik mi kontrol yapan deðiþken
    public GameObject WillGoWarCharacter;//karakterlerin gideceði yer
    public Slider _Slider;//slider
    public GameObject EndGame;//finish line

    private void Start()
    {
        float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe deðiþkenine karaterin posizyponu ile bitiþ çizgisinin posizyonu arasýndaki mesafeyi atar
        _Slider.maxValue = difference;//sliderýn maksimum deðerine mesafe deðiþkenini atars
    }
    private void FixedUpdate()
    {
        //karkater sona gelmediyse olmasý gerekenler
        if (ComeToEnd==false)
        {
            //karakter zamanda .5f büyüklüðünde ilerler
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);
        }
    }
    void Update()
    {
        if (Time.time != 0)
        {
            //karakter sona geldi ise olmasý gerekenler
            if (ComeToEnd == true)
            {
                //karakter bitiþ çizgisini geçtikten sonra kullanýcýnýn konrolünden çýkar ve savaþ alanýn ortasýna otomatik gider
                transform.position = Vector3.Lerp(transform.position, WillGoWarCharacter.transform.position, .015f);
                //bitiþ çizgisini geçtikten sonra slider da kalan boþluðu kapatmak için yapýlan iþlemler
                //eðer karakter bitiþ çigisini geçtiyse ve sliderýn deðeri 0 a eþit DEÐÝLSE sliderýn deðerini .005 azalt
                if (_Slider.value != 0)
                {
                    _Slider.value -= .005f;
                }
            }
            else//karakter sona gelmediyse olmasý gerekenler
            {
                float difference = Vector3.Distance(transform.position, EndGame.transform.position);//mesafe deðiþkenine bitiþ çizgisi ile karakterin arasýndaki mesafeyi atar
                _Slider.value = difference;// kaydýrýcýnýn deðerine mesafayi atar

                if (Input.GetKey(KeyCode.Mouse0)) //mousenin sað tuþuna basýldýðýnda olmasý gerekenler
                {
                    //karakterin x bileþeninde saða ve sola haraket etmesini saðlayan koþullar
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


    //karakterin çarpýp geçme tetikleme olaylarý
    private void OnTriggerEnter(Collider other)
    {
        //taglarý bölme,toplama,çarpma,çýkarma iþlemleri olan nesneleri tetikler ise olacaklar
        if (other.CompareTag("DivisionProcess")|| other.CompareTag("CollectionProcess") || other.CompareTag("MultiplacationProcess") || other.CompareTag("ExtractionProcess"))
        {
            //gamemaner scriptindeki manmanager metoduna verileri gönderir
            int num = int.Parse(other.name);
            _GameManager.ManManager(other.tag,num,other.transform);
        }
        else if (other.CompareTag("LastTrigger"))//karakterin çarptýðý tetikledi nesnenin tagý son tetkleyici ise olacaklar
        {
            _Camera.ComeToEnd= true;//kameranýn sona geldik mi deðiþkeni true olur
            _GameManager.EnemyTrigger();//gamemanger scriptindeki düþman tetikleme metodu çalýþýr
            ComeToEnd = true;//karakterin sona geldik mi deðiþkeni true olur
        }
        else if (other.CompareTag("EmptyCharacters"))//karakterin çarptýðý,tetiklediði nesnenin tagý boþ karakterler ise olacaklar
        {
            _GameManager.LowerCharacters.Add(other.gameObject);//gamemanager scriptinde  ki karakterler dizisine ekleme yapar
        }
    }

    //çarpma ve kalma olaylarý
    private void OnCollisionEnter(Collision collision)
    {
        //direk,iðneli kutu,pervanenin iðne taglý objelere çarptýðýnda yapýlmasý gereken iþlemler
        if (collision.gameObject.CompareTag("Mast")|| collision.gameObject.CompareTag("NeedleBox")||collision.gameObject.CompareTag("PropallerNeedle"))
        {
            //ekranýn sað tarafýnda ise
            if (transform.position.x>0)
            {
                //bulunduðu posizyondan x bileieninde .2 birim kaydýrma yapar 
                transform.position = new Vector3(transform.position.x -.2f,transform.position.y,transform.position.z);
            }
            else //ekranýn sol tarafýnda ise
            {
                //bulunduðu posizyondan x bileieninde .2 birim kaydýrma yapar
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z);
            }
        }
    }

}
