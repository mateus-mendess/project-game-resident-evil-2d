using UnityEngine;

public class EndZoneTrigger : MonoBehaviour
{
    [Header("Transição")]
    [SerializeField] private SceneTransition transitionManager;
    [SerializeField] private string targetScene = "MainMenu";
    [SerializeField] private string endMessage = "Obrigado por jogar a demo!";

    [Header("Tag do Player")]
    [SerializeField] private string playerTag = "Player";

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"EndZone tocado por: {other.gameObject.name} | Tag: {other.tag}");
        
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        transitionManager.LoadSceneWithText(targetScene, endMessage);
    }
}