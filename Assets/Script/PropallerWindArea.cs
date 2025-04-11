using rgame;
using rgamekeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropallerWindArea : MonoBehaviour
{
    /// <summary>
    /// Eðer alt karakterler Rüzgar alanýna girdiyse. alt karakterlere güç uygular ve rüzgar yemiþ efekti verir
    /// </summary>
    /// <param name="other">Karaktelerin Çarptýý alan/bölge</param>
    private void OnTriggerStay(Collider other)
    {    
        if (other.CompareTag(GameCharacters.LowerCharacters))//eðer çarpýþmayý tetikleyen objenin tagý alt karakter ise olmasý gerekenler
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.Impulse);//x eksenin de -5 lik bir kuvvet uygulanýyor
        }
    }
}
