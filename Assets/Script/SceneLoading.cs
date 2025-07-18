using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public GameObject LoadingPanel;
    public Animator SceneLoadingAnimator;
    private const string Load = "Load";//animat�rdeki load de�i�keni
    /// <summary>
    /// �stenen indexli sahne y�klenirken Sahne y�kleme Ekran�n� ortaya ��kart�r
    /// </summary>
    /// <param name="SceneIndex">Y�klenilmesi istenen sahnenin indexi</param>
    public void LoadScene(int SceneIndex)
    {
        LoadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync(SceneIndex));
    }
    IEnumerator LoadSceneAsync(int SceneIndex)
    {
        SceneLoadingAnimator.SetBool(Load, true);
        yield return new WaitForSeconds(1f);
        AsyncOperation operation =SceneManager.LoadSceneAsync(SceneIndex);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true; // Yeni sahneye ge�i� yap
        SceneLoadingAnimator.SetBool(Load, false); // Animasyonu durdur
        yield return new WaitForSeconds(0.5f);
        LoadingPanel.SetActive(false); // Y�kleme panelini kapat
    }
}
