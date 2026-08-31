using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [SerializeField] PetData[] allPets;
    [SerializeField] SkillData[] allSkills;
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
        var data = SaveManager.Instance.Data;
        var up    = UpgradeManager.Instance;
        var pet   = PetManager.Instance.GetTotalBonus(allPets);
        var skill = SkillManager.Instance.GetTotalPassiveBonus(allSkills);

        float baseAtk = characterData.baseATK + up.GetTotalBonus(StatType.ATK);
        float baseDef = characterData.baseDEF  + up.GetTotalBonus(StatType.DEF);
        float baseHp  = characterData.baseHP   + up.GetTotalBonus(StatType.HP);

        float totalAtkMult = 1f + (pet.atkPercent + skill.atkPercent) / 100f;
        float totalHpMult  = 1f + (pet.hpPercent  + skill.hpPercent)  / 100f;

        nameText.text  = characterData.characterName;
        levelText.text = $"Lv.{data.playerLevel}";
        atkText.text   = $"공격력: {baseAtk * totalAtkMult:F0}";
        defText.text   = $"방어력: {baseDef + pet.defFlat + skill.defFlat:F0}";
        hpText.text    = $"HP: {baseHp * totalHpMult:F0}";
        speedText.text = $"속도: {characterData.baseSpeed + up.GetTotalBonus(StatType.Speed):F1}";
        critText.text  = $"크리티컬: {characterData.baseCritChance:F0}% / {characterData.baseCritMultiplier:F1}x";
    }
}
