using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private RectTransform newGameOption;
    [SerializeField] private RectTransform exitOption;

    [Header("Arrow")]
    [SerializeField] private RectTransform arrow;

    [Header("Arrow Offsets")]
    [SerializeField] private Vector2 newGameOffset;
    [SerializeField] private Vector2 exitOffset;

    [Header("Transition")]
    [SerializeField] private SceneTransition transitionManager;

    [Header("Blink")]
    [SerializeField] private float blinkSpeed = 4f;

    private CanvasGroup canvasGroup;
    private int selectedIndex = 0;
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
        HandleInput();
        BlinkArrow();
    }

    private void HandleInput()
    {
        if (isConfirming) return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedIndex < 1)
            {
                selectedIndex++;
                UpdateSelectionVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                UpdateSelectionVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ConfirmSelection();
        }
    }

    private void UpdateSelectionVisual()
    {
        if (selectedIndex == 0)
        {
            arrow.anchoredPosition = newGameOption.anchoredPosition + newGameOffset;
        }
        else
        {
            arrow.anchoredPosition = exitOption.anchoredPosition + exitOffset;
        }
    }

    private void ConfirmSelection()
    {
        isConfirming = true;

        if (selectedIndex == 0)
        {
            transitionManager.LoadScene("ChoiceScene");
        }
        else
        {
            Application.Quit();
        }
    }

    private void BlinkArrow()
    {
        if (canvasGroup == null) return;

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        canvasGroup.alpha = alpha;
    }
}