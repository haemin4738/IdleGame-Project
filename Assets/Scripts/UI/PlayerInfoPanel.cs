using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] PetData[] allPets;
    [SerializeField] SkillData[] allSkills;
    [SerializeField] EquipmentData[] allEquipment;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text atkText;
    [SerializeField] TMP_Text defText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text speedText;
    [SerializeField] TMP_Text critText;

    void OnEnable() => Refresh();

    void Refresh()
    {
        var data  = SaveManager.Instance.Data;
        var up    = UpgradeManager.Instance;
        var pet   = PetManager.Instance.GetTotalBonus(allPets);
        var skill = SkillManager.Instance.GetTotalPassiveBonus(allSkills);
        var equip = EquipmentManager.Instance.GetTotalBonus(allEquipment);

        float baseAtk = CharacterManager.Instance.ActiveCharacter.baseATK + up.GetTotalBonus(StatType.ATK);
        float baseDef = CharacterManager.Instance.ActiveCharacter.baseDEF  + up.GetTotalBonus(StatType.DEF);
        float baseHp  = CharacterManager.Instance.ActiveCharacter.baseHP   + up.GetTotalBonus(StatType.HP);

        float totalAtkMult = 1f + (pet.atkPercent + skill.atkPercent + equip.atkPercent) / 100f;
        float totalHpMult  = 1f + (pet.hpPercent  + skill.hpPercent  + equip.hpPercent)  / 100f;

        float totalAtk = baseAtk * totalAtkMult;
        float totalDef = baseDef + pet.defFlat + skill.defFlat + equip.defFlat;
        float totalHp  = baseHp * totalHpMult;

        nameText.text  = CharacterManager.Instance.ActiveCharacter.characterName;
        levelText.text = $"Lv.{data.playerLevel}";
        atkText.text   = $"공격력: {totalAtk:F0} ({CharacterManager.Instance.ActiveCharacter.baseATK:F0} + {totalAtk - CharacterManager.Instance.ActiveCharacter.baseATK:F0})";
        defText.text   = $"방어력: {totalDef:F0} ({CharacterManager.Instance.ActiveCharacter.baseDEF:F0} + {totalDef - CharacterManager.Instance.ActiveCharacter.baseDEF:F0})";
        hpText.text    = $"HP: {totalHp:F0} ({CharacterManager.Instance.ActiveCharacter.baseHP:F0} + {totalHp - CharacterManager.Instance.ActiveCharacter.baseHP:F0})";
        float baseSpeed = CharacterManager.Instance.ActiveCharacter.baseSpeed;
        float speedBonus = up.GetTotalBonus(StatType.Speed);
        speedText.text = $"속도: {baseSpeed + speedBonus:F2} ({baseSpeed:F2} + {speedBonus:F2})";
        float critChanceBonus = up.GetTotalBonus(StatType.CritChance) + pet.critChanceFlat + skill.critChanceFlat + equip.critChanceFlat;
        float critDamageBonus = up.GetTotalBonus(StatType.CritDamage) + pet.critDamageFlat + skill.critDamageFlat + equip.critDamageFlat;
        critText.text = $"크리티컬: {CharacterManager.Instance.ActiveCharacter.baseCritChance + critChanceBonus:F0}% ({CharacterManager.Instance.ActiveCharacter.baseCritChance:F0}% + {critChanceBonus:F0}%) / {CharacterManager.Instance.ActiveCharacter.baseCritMultiplier + critDamageBonus:F2}x";
    }
}
