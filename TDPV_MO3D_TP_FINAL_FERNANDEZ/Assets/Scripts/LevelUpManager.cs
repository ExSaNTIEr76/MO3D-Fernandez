using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [Header("Referencias")]
    [SerializeField] private GameObject levelUpCanvas;
    [SerializeField] private GameObject LevelUpBackground;
    [SerializeField] private TMP_Text defensiveText;
    [SerializeField] private TMP_Text ofensiveText;
    [SerializeField] private TMP_Text buffText;

    [Header("Base de datos de mejoras")]
    [SerializeField] private List<UpgradeSO> upgrades;

    private UpgradeSO[] currentOptions = new UpgradeSO[3];
    private PlayerStats player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        levelUpCanvas.SetActive(false);
    }

    public void TriggerLevelUp()
    {
        Time.timeScale = 0f;
        levelUpCanvas.SetActive(true);

        PickClassifiedUpgrades();

        defensiveText.text = currentOptions[0].upgradeName; // Defensive
        ofensiveText.text = currentOptions[1].upgradeName; // Ofensive
        buffText.text = currentOptions[2].upgradeName; // Buff

        listeningForInput = true;
    }

    // ------------------------------
    // 🔥 NUEVA FUNCIÓN: UNA POR CLASE
    // ------------------------------
    private void PickClassifiedUpgrades()
    {
        currentOptions[0] = GetRandomUpgradeOfClass(UpgradeSO.UpgradeClass.Defensive);
        currentOptions[1] = GetRandomUpgradeOfClass(UpgradeSO.UpgradeClass.Ofensive);
        currentOptions[2] = GetRandomUpgradeOfClass(UpgradeSO.UpgradeClass.Buff);
    }

    private UpgradeSO GetRandomUpgradeOfClass(UpgradeSO.UpgradeClass c)
    {
        var list = upgrades.Where(u => u.upgradeClass == c).ToList();

        if (list.Count == 0)
        {
            Debug.LogWarning("No upgrades found for class: " + c);
            return null;
        }

        return list[Random.Range(0, list.Count)];
    }

    // ------------------------------

    private bool listeningForInput = false;

    private void Update()
    {
        if (!listeningForInput) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseUpgrade(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseUpgrade(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChooseUpgrade(2);
    }

    private void ChooseUpgrade(int index)
    {
        ApplyUpgrade(currentOptions[index]);

        levelUpCanvas.SetActive(false);
        Time.timeScale = 1f;
        listeningForInput = false;
    }

    private void ApplyUpgrade(UpgradeSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeSO.UpgradeType.MoveSpeed:
                player.moveSpeed += upgrade.value;
                break;

            case UpgradeSO.UpgradeType.ShotSpeed:
                player.shotSpeed += upgrade.value;
                break;

            case UpgradeSO.UpgradeType.FireRate:
                player.fireRate += upgrade.value;
                break;

            case UpgradeSO.UpgradeType.ExtraBullet:
                player.extraBullets += (int)upgrade.value;
                break;

            case UpgradeSO.UpgradeType.FanShot:
                player.fanShot += (int)upgrade.value;
                break;

            case UpgradeSO.UpgradeType.Damage:
                player.shotDamage += upgrade.value;
                break;




            case UpgradeSO.UpgradeType.HealthUp:
                player.maxHealth += (int)upgrade.value;
                break;

            case UpgradeSO.UpgradeType.FullHealth:
                player.currentHealth = player.maxHealth;
                break;



            case UpgradeSO.UpgradeType.Shotgun:
                player.shotgun = true;
                break;

            case UpgradeSO.UpgradeType.HomingShot:
                player.homingShot = true;
                break;

            case UpgradeSO.UpgradeType.PiercingShot:
                player.piercingShot = true;
                break;
        }

        Debug.Log("Upgrade applied: " + upgrade.upgradeName);
    }
}
