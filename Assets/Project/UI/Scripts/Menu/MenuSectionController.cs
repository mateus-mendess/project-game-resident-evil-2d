using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelectionController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform exitButton;

    [Header("Arrow")]
    [SerializeField] private RectTransform arrow;

    [Header("Arrow Offsets")]
    [SerializeField] private Vector2 playOffset;
    [SerializeField] private Vector2 exitOffset;

    [Header("Blink")]
    [SerializeField] private float blinkSpeed = 4f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float confirmVolume = 1.5f;
    [SerializeField] private AudioClip confirmSound;

    private CanvasGroup canvasGroup;
    private int selectedIndex = 1;
    private bool isConfirming = false;

    private void Start()
    {
        canvasGroup = arrow.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = arrow.gameObject.AddComponent<CanvasGroup>();

        UpdateArrowPosition();
    }

    private void Update()
    {
        if (isConfirming) return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                UpdateArrowPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedIndex < 1)
            {
                selectedIndex++;
                UpdateArrowPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ConfirmSelection();
        }

        BlinkArrow();
    }

    private void UpdateArrowPosition()
    {
        if (selectedIndex == 1)
            arrow.anchoredPosition = playButton.anchoredPosition + playOffset;
        else
            arrow.anchoredPosition = exitButton.anchoredPosition + exitOffset;
    }

    private void ConfirmSelection()
    {
        isConfirming = true;

        if (audioSource != null && confirmSound != null)
            audioSource.PlayOneShot(confirmSound, confirmVolume);

        StartCoroutine(ExecuteSelection());
    }

    private IEnumerator ExecuteSelection()
    {
        yield return new WaitForSeconds(0.2f);

        switch (selectedIndex)
        {
            case 1:
                SceneManager.LoadScene("ChoiceScene");
                break;

            case 0:
                Application.Quit();
                break;
        }
    }

    private void BlinkArrow()
    {
        if (canvasGroup == null) return;

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        canvasGroup.alpha = alpha;
    }
}