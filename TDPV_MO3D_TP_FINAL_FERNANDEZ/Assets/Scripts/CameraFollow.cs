using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Settings")]
    public float smoothTime = 0.25f;
    public Vector3 offset = new Vector3(0, 15, -10);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (!target) return;

        // Posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Movimiento suave
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}
