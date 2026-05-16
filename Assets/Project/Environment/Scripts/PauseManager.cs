using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseText;

    private bool isPaused = false;

    private void Start()
    {
        if (pauseText != null)
            pauseText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseText != null)
            pauseText.SetActive(isPaused);
    }
}