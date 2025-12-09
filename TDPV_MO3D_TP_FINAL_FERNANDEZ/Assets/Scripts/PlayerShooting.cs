using UnityEngine;
using System.Linq;
using System.Collections;


public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private float fireCooldown = 0f;
    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        Transform nearest = GetNearestEnemy();
        if (nearest == null) return;

        float dynamicFireRate = 1f / stats.fireRate;

        if (fireCooldown <= 0f)
        {
            ShootPattern(nearest);
            fireCooldown = dynamicFireRate;
        }
    }


    private void ShootPattern(Transform target)
    {
        Vector3 baseDir = (target.position - transform.position).normalized;

        // --- SHOT BASE ---
        ShootDirection(baseDir);

        // --- EXTRA BULLETS ---
        StartCoroutine(ExtraBulletBurst(baseDir));

        // --- FAN SHOT ---
        for (int i = 1; i <= stats.fanShot; i++)
        {
            float angle = i * 10f;

            ShootDirectionalFan(Quaternion.Euler(0, angle, 0) * baseDir);
            ShootDirectionalFan(Quaternion.Euler(0, -angle, 0) * baseDir);
        }

        // --- SHOTGUN ---
        if (stats.shotgun)
            ShotgunShot(baseDir);
    }


    private IEnumerator ExtraBulletBurst(Vector3 baseDir)
    {
        for (int i = 0; i < stats.extraBullets; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ShootDirection(baseDir);
        }
    }


    private void ShootDirection(Vector3 dir)
    {
        Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();

        b.InitDirectional(
            dir,
            stats.shotSpeed,
            (int)stats.shotDamage,
            stats.homingShot,
            stats.piercingShot,
            3f
        );

        if (stats.homingShot)
            b.SetTarget(GetNearestEnemy());
    }

    private void ShootDirectionalFan(Vector3 dir)
    {
        Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();

        b.InitDirectional(
            dir,
            stats.shotSpeed,
            (int)stats.shotDamage,
            stats.homingShot,
            stats.piercingShot,
            3f,
            6f  // homing suave para el fan
        );

        if (stats.homingShot)
            b.SetTarget(GetNearestEnemy());
    }


    private void ShotgunShot(Vector3 baseDir)
    {
        for (int i = 0; i < 10; i++)   // 10 pellets
        {
            float spread = Random.Range(-15f, 15f);
            Vector3 dir = Quaternion.Euler(0, spread, 0) * baseDir;

            ShootDirectionalShortLife(dir);
        }
    }


    private void ShootDirectionalShortLife(Vector3 dir)
    {
        Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();

        b.InitDirectional(
            dir,
            stats.shotSpeed * 0.8f, // escopeta viaja más lento
            (int)stats.shotDamage,
            false,                  // shotgun NO usa homing
            stats.piercingShot,
            0.4f                    // vida corta
        );
    }

    private Transform GetNearestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        return enemies
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .First()
            .transform;
    }
}
