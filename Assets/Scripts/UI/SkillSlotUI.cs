using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text bonusText;
    [SerializeField] Button upgradeButton;

    SkillData _skill;

    public void Init(SkillData skill)
    {
        _skill = skill;
        upgradeButton.onClick.AddListener(OnUpgrade);
        Refresh();
    }

    public void Refresh()
    {
        var level = SkillManager.Instance.GetLevel(_skill);
        nameText.text  = _skill.skillName;
        levelText.text = $"Lv.{level}/{_skill.maxLevel}";
        bonusText.text = BuildBonusText(_skill.statPerLevel, level);
        upgradeButton.interactable = level < _skill.maxLevel && SkillManager.Instance.GetSkillPoints() > 0;
    }

    void OnUpgrade()
    {
        if (!SkillManager.Instance.TryUpgrade(_skill))
            ToastPopup.Instance.Show("스킬 포인트가 부족합니다");
        Refresh();
    }

    string BuildBonusText(StatBonus b, int level)
    {
        if (b.atkPercent  != 0) return $"ATK +{b.atkPercent  * level:F0}%";
        if (b.hpPercent   != 0) return $"HP +{b.hpPercent    * level:F0}%";
        if (b.goldPercent != 0) return $"골드 +{b.goldPercent * level:F0}%";
        if (b.expPercent  != 0) return $"EXP +{b.expPercent  * level:F0}%";
        if (b.defFlat     != 0) return $"DEF +{b.defFlat     * level:F0}";
        return "";
    }
}
