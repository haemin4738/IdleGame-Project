using UnityEngine;

[DefaultExecutionOrder(-50)]
public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
    }

    void OnDestroy() => EventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);

    void OnLevelUp(LevelUpEvent _)
    {
        SaveManager.Instance.Data.skillPoints++;
        EventBus.Publish(new SkillPointsChangedEvent { Points = SaveManager.Instance.Data.skillPoints });
    }

    public int GetSkillPoints() => SaveManager.Instance.Data.skillPoints;

    public int GetLevel(SkillData skill)
    {
        var entry = SaveManager.Instance.Data.skillLevels.Find(e => e.key == skill.skillName);
        return entry?.value ?? 0;
    }

    public bool TryUpgrade(SkillData skill)
    {
        var data = SaveManager.Instance.Data;
        var level = GetLevel(skill);
        if (data.skillPoints <= 0 || level >= skill.maxLevel) return false;

        data.skillPoints--;
        SetLevel(skill, level + 1);
        EventBus.Publish(new SkillPointsChangedEvent { Points = data.skillPoints });
        return true;
    }

    public StatBonus GetTotalPassiveBonus(SkillData[] skills)
    {
        var total = new StatBonus();
        foreach (var skill in skills)
        {
            if (skill.skillType != SkillType.Passive) continue;
            var level = GetLevel(skill);
            total.atkPercent  += skill.statPerLevel.atkPercent  * level;
            total.hpPercent   += skill.statPerLevel.hpPercent   * level;
            total.goldPercent += skill.statPerLevel.goldPercent * level;
            total.expPercent  += skill.statPerLevel.expPercent  * level;
            total.defFlat     += skill.statPerLevel.defFlat     * level;
        }
        return total;
    }

    void SetLevel(SkillData skill, int level)
    {
        var list = SaveManager.Instance.Data.skillLevels;
        var entry = list.Find(e => e.key == skill.skillName);
        if (entry != null) entry.value = level;
        else list.Add(new UpgradeEntry { key = skill.skillName, value = level });
    }
}
