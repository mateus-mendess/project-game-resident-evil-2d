using UnityEngine;
using UnityEngine.UI;

public class ArrowIndicator : MonoBehaviour
{
    [Header("Piscar")]
    [SerializeField] private float blinkSpeed = 3f;

    private Image image;
    private bool isShowing = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        Hide();
    }

    private void Update()
    {
        if (!isShowing) return;

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void Show()
    {
        isShowing = true;
        image.enabled = true;
    }

    public void Hide()
    {
        isShowing = false;
        image.enabled = false;
    }
}