using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 5;
    public int currentHealth;
    public int enemyDamage = 1;

    [Header("XP")]
    public int xpAmount = 5;
    [SerializeField] private GameObject xpPrefab;
    public float dropRadius = 1f;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 3f;

    private PlayerStats player;
    private bool isDead = false;
    private Transform target;
    private bool isTouchingPlayer = false;

    private void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target == null) return;

        if (!isTouchingPlayer)
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;

            // Obtener PlayerStats del jugador
            player = collision.gameObject.GetComponent<PlayerStats>();

            if (player != null)
            {
                player.TakeDamage(enemyDamage);
            }
            else
            {
                Debug.LogError("PlayerStats no encontrado en el jugador.");
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Spawn xp solo una vez
        Instantiate(xpPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void DropXP()
    {
        if (xpPrefab == null) return;

        Vector3 randomOffset = Random.insideUnitSphere * dropRadius;
        randomOffset.y = 0;

        Vector3 dropPos = transform.position + randomOffset;

        GameObject xp = Instantiate(xpPrefab, dropPos, Quaternion.identity);
        xp.GetComponent<XPpoint>().xpValue = xpAmount;
    }
}
