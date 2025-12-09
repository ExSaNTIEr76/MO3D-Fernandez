using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "Waves/Wave")]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int amount;
    }

    public List<EnemySpawnData> enemies;
}
