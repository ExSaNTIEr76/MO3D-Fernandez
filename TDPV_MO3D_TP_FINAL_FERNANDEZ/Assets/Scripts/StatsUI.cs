using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text nextText;
    [SerializeField] private TMP_Text waveText;

    private PlayerStats player;
    private PlayerXP xp;
    private EnemySpawner spawner;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        xp = FindObjectOfType<PlayerXP>();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        if (player != null)
            healthText.text = $"Health: {player.currentHealth}/{player.maxHealth}";

        if (xp != null)
        {
            levelText.text = $"Level: {xp.currentLevel}";
            nextText.text = $"Next: {xp.nextLevelXP - xp.currentXP}";
        }

        if (spawner != null)
            waveText.text = $"Wave: {spawner.GetCurrentWave()}";
    }
}
