using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Video;

public class CameraManagement : MonoBehaviour
{
    [Header("Kamera hedefi(Karakter)")]
    [Tooltip("Ana karakter verilecek")]
    public Transform target; //hedef

    [Header("Karakter ile arasindaki mesafe")]
    [Tooltip("mesafe verilecek")]
    public Vector3 target_offset; //mesafe

    public bool ComeToEnd; // sona geldik mi
    [Header("Kameran�n gidecegi yer")]
    [Tooltip("bolum sonunda kameran�n alacagi konum")]
    public GameObject WillGoCamera; // kameran�n gidece�i yer
    void Start()
    {
        target_offset=transform.position-target.position;//kamera ile runner aras�ndaki mesafeyi bulur
    }

    void Update()
    {
        //karakter sona geld�inide kameran�n gitmesi gereken yer Lerp() komutuyla yumu�ak ge�i� sa�land�
        if (ComeToEnd==false)
        {
            transform.position = Vector3.Lerp(transform.position,target.position+target_offset,.125f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position,WillGoCamera.transform.position,.015f);
        }

    }
}
