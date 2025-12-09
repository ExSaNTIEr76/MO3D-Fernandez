using UnityEngine;

public class XPpoint : MonoBehaviour
{
    public int xpValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerXP playerXP = other.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.AddXP(xpValue);
            }

            Destroy(gameObject);
        }
    }
}
