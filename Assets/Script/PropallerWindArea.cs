using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropallerWindArea : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {    
        if (other.CompareTag("LowerCharacters"))//e�er �arp��may� tetikleyen objenin tag� alt karakter ise olmas� gerekenler
        {
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.Impulse);//x eksenin de -5 lik bir kuvvet uygulan�yor
        }
    }
}
