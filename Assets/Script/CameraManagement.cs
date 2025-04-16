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
    [Header("Kameranýn gidecegi yer")]
    [Tooltip("bolum sonunda kameranýn alacagi konum")]
    public GameObject WillGoCamera; // kameranýn gideceði yer
    void Start()
    {
        target_offset=transform.position-target.position;//kamera ile runner arasýndaki mesafeyi bulur
    }

    void Update()
    {
        //karakter sona geldðinide kameranýn gitmesi gereken yer Lerp() komutuyla yumuþak geçiþ saðlandý
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
