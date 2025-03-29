using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public GameObject LoadingPanel;
    public Animator SceneLoadingAnimator;
   
    public void LoadScene(int SceneIndex)
    {
        LoadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync(SceneIndex));
    }
    IEnumerator LoadSceneAsync(int SceneIndex)
    {
        SceneLoadingAnimator.SetBool("Load",true);
        yield return new WaitForSeconds(1f);
        AsyncOperation operation =SceneManager.LoadSceneAsync(SceneIndex);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true; // Yeni sahneye geçiþ yap
        SceneLoadingAnimator.SetBool("Load", false); // Animasyonu durdur
        yield return new WaitForSeconds(0.5f);
        LoadingPanel.SetActive(false); // Yükleme panelini kapat
    }
}
