using UnityEngine;

[DefaultExecutionOrder(-45)]
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    static int BuyCost(Rarity r) => r switch
    {
        Rarity.Normal => 500,
        Rarity.Rare   => 2000,
        Rarity.Hero   => 5000,
        _             => 10000
    };

    static int UpgradeBaseCost(Rarity r) => r switch
    {
        Rarity.Normal => 100,
        Rarity.Rare   => 200,
        Rarity.Hero   => 500,
        _             => 1000
    };

    public bool IsOwned(EquipmentData eq) =>
        SaveManager.Instance.Data.ownedEquipment.Contains(eq.equipmentName);

    public bool TryBuy(EquipmentData eq)
    {
        var data = SaveManager.Instance.Data;
        int cost = BuyCost(eq.rarity);
        if (IsOwned(eq) || data.gold < cost) return false;
        data.gold -= cost;
        data.ownedEquipment.Add(eq.equipmentName);
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        return true;
    }

    public bool IsEquipped(EquipmentData eq) => GetEquippedName(eq.slot) == eq.equipmentName;

    string GetEquippedName(EquipmentSlot slot)
    {
        var data = SaveManager.Instance.Data;
        return slot switch
        {
            EquipmentSlot.Weapon    => data.equippedWeapon,
            EquipmentSlot.Armor     => data.equippedArmor,
            _                       => data.equippedAccessory
        };
    }

    public void Equip(EquipmentData eq)
    {
        if (!IsOwned(eq)) return;
        var data = SaveManager.Instance.Data;
        switch (eq.slot)
        {
            case EquipmentSlot.Weapon:    data.equippedWeapon    = eq.equipmentName; break;
            case EquipmentSlot.Armor:     data.equippedArmor     = eq.equipmentName; break;
            case EquipmentSlot.Accessory: data.equippedAccessory = eq.equipmentName; break;
        }
    }

    public void Unequip(EquipmentSlot slot)
    {
        var data = SaveManager.Instance.Data;
        switch (slot)
        {
            case EquipmentSlot.Weapon:    data.equippedWeapon    = ""; break;
            case EquipmentSlot.Armor:     data.equippedArmor     = ""; break;
            case EquipmentSlot.Accessory: data.equippedAccessory = ""; break;
        }
    }

    public int GetLevel(EquipmentData eq)
    {
        var entry = SaveManager.Instance.Data.equipmentLevels.Find(e => e.key == eq.equipmentName);
        return entry?.value ?? 0;
    }

    public int GetUpgradeCost(EquipmentData eq) =>
        Mathf.RoundToInt(UpgradeBaseCost(eq.rarity) * Mathf.Pow(1.2f, GetLevel(eq)));

    public int GetBuyCost(EquipmentData eq) => BuyCost(eq.rarity);

    public bool TryUpgrade(EquipmentData eq)
    {
        if (!IsOwned(eq)) return false;
        int level = GetLevel(eq);
        if (level >= eq.upgradeMaxLevel) return false;
        var data = SaveManager.Instance.Data;
        int cost = GetUpgradeCost(eq);
        if (data.gold < cost) return false;
        data.gold -= cost;
        var entry = data.equipmentLevels.Find(e => e.key == eq.equipmentName);
        if (entry != null) entry.value++;
        else data.equipmentLevels.Add(new UpgradeEntry { key = eq.equipmentName, value = 1 });
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        return true;
    }

    public StatBonus GetTotalBonus(EquipmentData[] allEquipment)
    {
        var total = new StatBonus();
        foreach (var eq in allEquipment)
        {
            if (!IsEquipped(eq)) continue;
            float mult = 1f + GetLevel(eq) * 0.1f;
            total.atkPercent     += eq.baseStats.atkPercent     * mult;
            total.hpPercent      += eq.baseStats.hpPercent      * mult;
            total.goldPercent    += eq.baseStats.goldPercent     * mult;
            total.expPercent     += eq.baseStats.expPercent      * mult;
            total.defFlat        += eq.baseStats.defFlat         * mult;
            total.critChanceFlat += eq.baseStats.critChanceFlat  * mult;
            total.critDamageFlat += eq.baseStats.critDamageFlat  * mult;
        }
        return total;
    }
}
