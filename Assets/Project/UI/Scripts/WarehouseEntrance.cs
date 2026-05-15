using UnityEngine;

public class WarehouseEntrance : MonoBehaviour
{
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private string sceneToLoad = "WarehouseScene";

    [Header("Transition")]
    [SerializeField] private SceneTransition transitionManager;

    private bool playerInRange;

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            transitionManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionIcon != null)
                interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }
}