using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Texto opcional (filho do FadePanel)")]
    [SerializeField] private TypewriterText typewriterText;

    // Transição simples (sem texto) — compatível com quem já chama LoadScene(string)
    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName, null));
    }

    // Transição com texto de máquina de escrever
    public void LoadSceneWithText(string sceneName, string message)
    {
        StartCoroutine(Transition(sceneName, message));
    }

    private IEnumerator Transition(string sceneName, string message)
    {
        // Dispara o fade para preto
        animator.SetBool("Fade", true);

        // Espera a tela ficar totalmente preta
        // Ajuste esse valor para bater com a duração da sua animação de fade in
        yield return new WaitForSeconds(0.5f);

        // Se tiver texto, toca a sequência inteira antes de carregar
        if (!string.IsNullOrEmpty(message) && typewriterText != null)
        {
            yield return StartCoroutine(typewriterText.PlayAndFadeOut(message));
        }

        SceneManager.LoadScene(sceneName);
    }
}