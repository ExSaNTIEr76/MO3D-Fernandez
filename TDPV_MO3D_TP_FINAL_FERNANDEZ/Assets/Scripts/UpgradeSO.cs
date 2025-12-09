using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public string upgradeName;

    public enum UpgradeClass
    {
        Defensive,
        Ofensive,
        Buff
    }

    public enum UpgradeType
    {
        MoveSpeed,
        ShotSpeed,
        FireRate,
        ExtraBullet,
        FanShot,
        Damage,

        HealthUp,
        FullHealth,
        
        Shotgun,
        HomingShot,
        PiercingShot
    }

    public UpgradeClass upgradeClass;
    public UpgradeType upgradeType;
    public float value;
}
