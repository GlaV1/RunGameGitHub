using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropallerWindArea : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {    
        if (other.CompareTag("LowerCharacters"))//eðer çarpýþmayý tetikleyen objenin tagý alt karakter ise olmasý gerekenler
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.Impulse);//x eksenin de -5 lik bir kuvvet uygulanýyor
        }
    }
}
