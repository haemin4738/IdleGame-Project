using UnityEngine;

public enum EquipmentSlot { Weapon, Armor, Accessory }
public enum Rarity { Normal, Rare, Hero, Legend }

[CreateAssetMenu(menuName = "IdleCA/EquipmentData")]
public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    public Sprite icon;
    public EquipmentSlot slot;
    public Rarity rarity;
    public StatBonus baseStats;
    public int upgradeMaxLevel;
}
