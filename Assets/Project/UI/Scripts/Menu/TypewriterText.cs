using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float charDelay = 0.05f;   // velocidade de digitação
    [SerializeField] private float fadeOutDuration = 0.4f;

    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.alpha = 0f;
    }

    // Chame isso para rodar toda a sequência; retorna quando o fade out terminar
    public IEnumerator PlayAndFadeOut(string message)
    {
        tmp.text = "";
        tmp.alpha = 1f;
        gameObject.SetActive(true);

        // Máquina de escrever
        foreach (char c in message)
        {
            tmp.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        // Pequena pausa antes de sumir
        yield return new WaitForSeconds(0.3f);

        // Fade out do texto
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        tmp.alpha = 0f;
        gameObject.SetActive(false);
    }
}