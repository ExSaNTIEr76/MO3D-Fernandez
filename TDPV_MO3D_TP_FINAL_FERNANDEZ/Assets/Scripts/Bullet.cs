using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int damage;
    private bool homing;
    private bool piercing;

    private Transform target;
    private Vector3 direction;

    private float lifeTime = 3f;
    private float homingStrength = 12f;

    public void InitDirectional(Vector3 dir, float speed, int dmg, bool homing, bool piercing, float lifeOverride, float homingStrength = 12f)
    {
        this.direction = dir.normalized;
        this.speed = speed;
        this.damage = dmg;
        this.homing = homing;
        this.piercing = piercing;

        this.lifeTime = lifeOverride;
        this.homingStrength = homingStrength;
    }


    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void Update()
    {
        // Vida
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) Destroy(gameObject);

        // HOMING
        if (homing && target != null)
        {
            Vector3 desired = (target.position - transform.position).normalized;
            direction = Vector3.Lerp(direction, desired, homingStrength * Time.deltaTime).normalized;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyChase e = other.GetComponent<EnemyChase>();
        if (e != null) e.TakeDamage(damage);

        if (!piercing) Destroy(gameObject);
    }
}