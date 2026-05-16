using UnityEngine;

public class EndZoneTrigger : MonoBehaviour
{
    [Header("Transição")]
    [SerializeField] private SceneTransition transitionManager;
    [SerializeField] private string targetScene = "MainMenu";
    [SerializeField] private string endMessage = "Obrigado por jogar a demo!";

    [Header("Tag do Player")]
    [SerializeField] private string playerTag = "Player";

    [Header("HUD")]
    [SerializeField] private GameObject hudCanvas;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;
        if (other.gameObject.name != "Player") return;

        triggered = true;

        if (hudCanvas != null)
            hudCanvas.SetActive(false);

        transitionManager.LoadSceneWithText(targetScene, endMessage);
    }
}