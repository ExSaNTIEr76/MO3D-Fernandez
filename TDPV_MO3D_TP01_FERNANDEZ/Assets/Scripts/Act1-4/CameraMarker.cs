using UnityEngine;

public class CameraMarker : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Cubo Verde
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 10f);

        // Línea Roja en dirección Forward
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15f);
    }
}
