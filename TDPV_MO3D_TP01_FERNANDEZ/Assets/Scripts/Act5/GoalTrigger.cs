using UnityEngine;
using TMPro;

public class GoalTrigger : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI winText;

    private void Start()
    {
        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡GANASTE!");

            if (winText != null)
                winText.gameObject.SetActive(true);
        }
    }
}
