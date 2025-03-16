using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManStainsControl : MonoBehaviour
{
    /// <summary>
    /// Efekt aktif olduktan 5 sn sonra efektin silinme iþlemini yapar
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
