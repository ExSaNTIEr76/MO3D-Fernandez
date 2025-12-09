using UnityEngine;

public class PlatformExpander : MonoBehaviour
{
    [Header("Config")]
    public float expandAmount = 5f;
    public float expandHeightCompensation = 0.5f; // por si se eleva

    private BoxCollider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public void ExpandPlatform()
    {
        transform.localScale += new Vector3(expandAmount, 0, expandAmount);

        // Ajuste opcional si la plataforma cambia punto de apoyo
        transform.position += new Vector3(0, expandHeightCompensation, 0);

        Debug.Log("Plataforma expandida!");
    }
}
