using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceMenuController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private RectTransform keyboardOptionOne;
    [SerializeField] private RectTransform keyboardOptionTwo;

    [Header("Arrow")]
    [SerializeField] private RectTransform arrow;

    [Header("Arrow Offsets")]
    [SerializeField] private Vector2 optionOneOffset;
    [SerializeField] private Vector2 optionTwoOffset;

    [Header("Icon Groups")]
    [SerializeField] private GameObject keyboardOneIconsGroup;
    [SerializeField] private GameObject keyboardTwoIconsGroup;

    [Header("Blink")]
    [SerializeField] private float blinkSpeed = 4f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private float confirmVolume = 1.5f;

    [Header("Transition")]
    [SerializeField] private SceneTransition transitionManager;

    private CanvasGroup canvasGroup;
    private int selectedIndex = 1;
    private bool isConfirming = false;

    private void Start()
    {
        canvasGroup = arrow.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = arrow.gameObject.AddComponent<CanvasGroup>();

        UpdateSelectionVisual();
    }

    private void Update()
    {
        if (isConfirming) return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                UpdateSelectionVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedIndex < 1)
            {
                selectedIndex++;
                UpdateSelectionVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ConfirmSelection();
        }

        BlinkArrow();
    }

    private void UpdateSelectionVisual()
    {
        if (selectedIndex == 1)
        {
            arrow.anchoredPosition = keyboardOptionOne.anchoredPosition + optionOneOffset;

            if (keyboardOneIconsGroup != null)
                keyboardOneIconsGroup.SetActive(true);

            if (keyboardTwoIconsGroup != null)
                keyboardTwoIconsGroup.SetActive(false);
        }
        else
        {
            arrow.anchoredPosition = keyboardOptionTwo.anchoredPosition + optionTwoOffset;

            if (keyboardOneIconsGroup != null)
                keyboardOneIconsGroup.SetActive(false);

            if (keyboardTwoIconsGroup != null)
                keyboardTwoIconsGroup.SetActive(true);
        }
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

        transitionManager.LoadScene("SampleScene");
    }

    private void BlinkArrow()
    {
        if (canvasGroup == null) return;

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        canvasGroup.alpha = alpha;
    }
}