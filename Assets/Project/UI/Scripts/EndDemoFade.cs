using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndDemoFade : MonoBehaviour
{
    public Image fadePanel;
    public TMP_Text continueText;

    public float fadeDuration = 1f;
    public float typingSpeed = 0.1f; // 🔥 velocidade da digitação

    private bool started = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (started) return;

        if (collision.CompareTag("Player"))
        {
            started = true;
            StartCoroutine(FadeToEnd());
        }
    }

    private IEnumerator FadeToEnd()
    {
        float timer = 0f;

        // 🔥 Fade da tela
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;

            SetAlpha(fadePanel, alpha);

            yield return null;
        }

        SetAlpha(fadePanel, 1f);

        yield return new WaitForSeconds(0.5f);

        // 🔥 Inicia o efeito de digitação
        StartCoroutine(TypeText("Continua..."));
    }

    private IEnumerator TypeText(string text)
    {
        continueText.text = "";
        SetAlpha(continueText, 1f);

        foreach (char letter in text)
        {
            continueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}