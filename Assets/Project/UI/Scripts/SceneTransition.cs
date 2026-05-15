using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Animator animator;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        animator.SetBool("Fade", true);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName);
    }
}