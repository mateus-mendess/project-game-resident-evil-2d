using UnityEngine;
using UnityEngine.UI;

public class WarehouseEntrance : MonoBehaviour
{
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private string sceneToLoad = "WarehouseScene";

    [Header("Transition")]
    [SerializeField] private SceneTransition transitionManager;
    [SerializeField] private string transitionMessage = "";

    [Header("Feedback inimigos vivos")]
    [SerializeField] private GameObject enemiesAliveIcon;

    [Header("Seta direcional")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform arrowTarget;
    [SerializeField] private Image arrowImage;
    [SerializeField] private float blinkSpeed = 3f;
    [SerializeField] private Vector2 rightPosition = new Vector2(300f, 0f);  // posição quando target está à direita
    [SerializeField] private Vector2 leftPosition = new Vector2(-300f, 0f);  // posição quando target está à esquerda

    private RectTransform arrowRectTransform;
    private bool playerInRange;
    private bool arrowShowing = false;

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);

        if (enemiesAliveIcon != null)
            enemiesAliveIcon.SetActive(false);

        if (arrowImage != null)
        {
            arrowRectTransform = arrowImage.GetComponent<RectTransform>();
            arrowImage.enabled = false;
        }
    }

    private void Update()
    {
        if (!playerInRange)
        {
            if (arrowImage != null && AllEnemiesDead())
                ShowArrow();
            return;
        }

        HideArrow();
        UpdateInteractionIcon();

        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (!AllEnemiesDead())
        {
            if (enemiesAliveIcon != null)
                StartCoroutine(ShowEnemiesAliveIcon());
            return;
        }

        if (!string.IsNullOrEmpty(transitionMessage))
            transitionManager.LoadSceneWithText(sceneToLoad, transitionMessage);
        else
            transitionManager.LoadScene(sceneToLoad);
    }

    private void ShowArrow()
    {
        if (arrowImage == null) return;

        arrowShowing = true;
        arrowImage.enabled = true;

        // Piscar
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, alpha);

        // Direção + posição
        if (player != null && arrowTarget != null && arrowRectTransform != null)
        {
            bool targetIsToTheRight = arrowTarget.position.x > player.position.x;

            arrowRectTransform.localScale = new Vector3(
                targetIsToTheRight ? 1f : -1f,
                1f,
                1f
            );

            arrowRectTransform.anchoredPosition = targetIsToTheRight ? rightPosition : leftPosition;
        }
    }
    private void HideArrow()
    {
        if (arrowImage == null) return;
        arrowShowing = false;
        arrowImage.enabled = false;
    }

    private void UpdateInteractionIcon()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(playerInRange && AllEnemiesDead());
    }

    private bool AllEnemiesDead()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();

        foreach (EnemyHealth enemy in enemies)
        {
            if (!enemy.IsDead())
                return false;
        }

        return true;
    }

    private System.Collections.IEnumerator ShowEnemiesAliveIcon()
    {
        if (enemiesAliveIcon != null)
        {
            enemiesAliveIcon.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            enemiesAliveIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateInteractionIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionIcon != null)
                interactionIcon.SetActive(false);

            if (enemiesAliveIcon != null)
                enemiesAliveIcon.SetActive(false);
        }
    }
}