using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Video;

public class CameraManagement : MonoBehaviour
{
    public Transform target; //hedef
    public Vector3 target_offset; //mesafe
    public bool ComeToEnd; // sona geldik mi
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
