using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
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

        float baseAtk = characterData.baseATK + up.GetTotalBonus(StatType.ATK);
        float baseDef = characterData.baseDEF  + up.GetTotalBonus(StatType.DEF);
        float baseHp  = characterData.baseHP   + up.GetTotalBonus(StatType.HP);

        float totalAtkMult = 1f + (pet.atkPercent + skill.atkPercent + equip.atkPercent) / 100f;
        float totalHpMult  = 1f + (pet.hpPercent  + skill.hpPercent  + equip.hpPercent)  / 100f;

        float totalAtk = baseAtk * totalAtkMult;
        float totalDef = baseDef + pet.defFlat + skill.defFlat + equip.defFlat;
        float totalHp  = baseHp * totalHpMult;

        nameText.text  = characterData.characterName;
        levelText.text = $"Lv.{data.playerLevel}";
        atkText.text   = $"공격력: {totalAtk:F0} ({characterData.baseATK:F0} + {totalAtk - characterData.baseATK:F0})";
        defText.text   = $"방어력: {totalDef:F0} ({characterData.baseDEF:F0} + {totalDef - characterData.baseDEF:F0})";
        hpText.text    = $"HP: {totalHp:F0} ({characterData.baseHP:F0} + {totalHp - characterData.baseHP:F0})";
        speedText.text = $"속도: {characterData.baseSpeed + up.GetTotalBonus(StatType.Speed):F1}";
        float critChanceBonus = up.GetTotalBonus(StatType.CritChance) + pet.critChanceFlat + skill.critChanceFlat + equip.critChanceFlat;
        float critDamageBonus = up.GetTotalBonus(StatType.CritDamage) + pet.critDamageFlat + skill.critDamageFlat + equip.critDamageFlat;
        critText.text = $"크리티컬: {characterData.baseCritChance + critChanceBonus:F0}% ({characterData.baseCritChance:F0}% + {critChanceBonus:F0}%) / {characterData.baseCritMultiplier + critDamageBonus:F2}x";
    }
}
