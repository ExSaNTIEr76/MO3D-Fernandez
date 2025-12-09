using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats Generales")]
    public float moveSpeed = 10f;

    public float shotSpeed = 20f;
    public float fireRate = 1f;
    public int extraBullets = 0;
    public int fanShot = 0;
    public float shotDamage= 1f;

    public int maxHealth = 100;
    public int currentHealth;

    public bool shotgun = false;

    public bool homingShot = false;
    public bool piercingShot = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Max(0, currentHealth); // evitar negativos

        // acá puedes agregar animaciones, invencibilidad, etc
    }
}
