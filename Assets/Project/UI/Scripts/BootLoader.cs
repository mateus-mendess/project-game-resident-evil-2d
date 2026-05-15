using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float loadingTime = 2f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(loadingTime);

        SceneManager.LoadScene("MainMenu");
    }
}