using System.Collections;
using UnityEngine;

public class CoinFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color color = spriteRenderer.color;
        float elapsed = 0f;

        // começa invisível
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }
}