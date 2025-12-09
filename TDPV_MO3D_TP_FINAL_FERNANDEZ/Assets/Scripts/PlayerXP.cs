using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel = 1; 
    public int maxLevel = 20;

    [Header("Experience")]
    public int currentXP = 0;
    public int nextLevelXP = 1;
    public float xpMultiplier = 1.5f;

    public void AddXP(int amount)
    {
        if (currentLevel >= maxLevel) return;

        currentXP += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentXP >= nextLevelXP && currentLevel < maxLevel)
        {
            currentXP -= nextLevelXP;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;

        nextLevelXP = Mathf.RoundToInt(nextLevelXP * 1.5f);

        Debug.Log($"LEVEL UP! Nuevo nivel: {currentLevel} - Próximo nivel requiere {nextLevelXP} XP");

        LevelUpManager.Instance.TriggerLevelUp();
    }
}
