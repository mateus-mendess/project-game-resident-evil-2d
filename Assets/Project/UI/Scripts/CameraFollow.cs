using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Camera Position")]
    [SerializeField] private float fixedY = 1f;
    [SerializeField] private float fixedZ = -10f;

    [Header("Camera Limits")]
    [SerializeField] private float minX = 6f;
    [SerializeField] private float maxX = 50f;

    private void LateUpdate()
    {
        if (player == null) return;

        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        transform.position = new Vector3(
            targetX,
            fixedY,
            fixedZ
        );
    }
}