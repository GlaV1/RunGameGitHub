using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManStainsControl : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
