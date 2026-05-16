using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("Cena")]
    [SerializeField] private string sceneToLoad = "MainMenu";
    [SerializeField] private float minimumLoadTime = 3f;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (elapsed < minimumLoadTime || operation.progress < 0.9f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}