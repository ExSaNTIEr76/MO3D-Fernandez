using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform platform;
    [SerializeField] private float spawnDelay = 0.1f;

    [Header("Expansion Config")]
    [SerializeField] private int expandEveryXWaves = 5;
    [SerializeField] private PlatformExpander platformExpander;

    [Header("Waves Config")]
    [SerializeField] private List<WaveSO> waves;
    [SerializeField] private int maxWaves = 40;

    private int currentWave = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private BoxCollider platformCollider;

    private bool isSpawning = false;

    void Start()
    {
        platformCollider = platform.GetComponent<BoxCollider>();

        if (platformCollider == null)
        {
            Debug.LogError("La plataforma necesita un BoxCollider.");
            return;
        }

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        aliveEnemies.RemoveAll(e => e == null);

        if (aliveEnemies.Count == 0 && !isSpawning)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        if (currentWave >= waves.Count)
        {
            Debug.Log("Enhorabuena. Lograste sobrevivir, soldado.");
            yield break;
        }

        isSpawning = true;
        currentWave++;

        // EXPANSIÓN AQUÍ:
        if (currentWave % expandEveryXWaves == 0)
        {
            platformExpander.ExpandPlatform();
            Debug.Log($"Wave {currentWave} — Plataforma expandida.");
        }

        WaveSO wave = waves[currentWave - 1];
        Debug.Log($"Wave {currentWave} – Spawning {wave.enemies.Count} tipos de enemigos");

        foreach (var entry in wave.enemies)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                SpawnEnemy(entry.enemyPrefab);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        isSpawning = false;
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPos = GetRandomPointOnPlatformBorder();

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    private Vector3 GetRandomPointOnPlatformBorder()
    {
        Vector3 size = Vector3.Scale(platformCollider.size, platform.localScale);
        Vector3 center = platformCollider.bounds.center;

        float halfX = size.x / 2f;
        float halfZ = size.z / 2f;

        int side = Random.Range(0, 4);
        float x = 0;
        float z = 0;

        switch (side)
        {
            case 0: x = Random.Range(-halfX, halfX); z = halfZ; break;
            case 1: x = Random.Range(-halfX, halfX); z = -halfZ; break;
            case 2: x = halfX; z = Random.Range(-halfZ, halfZ); break;
            case 3: x = -halfX; z = Random.Range(-halfZ, halfZ); break;
        }

        float y = platformCollider.bounds.max.y;
        return new Vector3(center.x + x, y, center.z + z);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
