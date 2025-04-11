using rgame;
using rgamekeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropallerWindArea : MonoBehaviour
{
    /// <summary>
    /// E�er alt karakterler R�zgar alan�na girdiyse. alt karakterlere g�� uygular ve r�zgar yemi� efekti verir
    /// </summary>
    /// <param name="other">Karaktelerin �arpt�� alan/b�lge</param>
    private void OnTriggerStay(Collider other)
    {    
        if (other.CompareTag(GameCharacters.LowerCharacters))//e�er �arp��may� tetikleyen objenin tag� alt karakter ise olmas� gerekenler
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.Impulse);//x eksenin de -5 lik bir kuvvet uygulan�yor
        }
    }
}
