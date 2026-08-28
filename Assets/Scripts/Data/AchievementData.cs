using UnityEngine;

public enum AchievementConditionType
{
    MonsterKillCount,
    StageReach,
    GoldEarned,
    LevelReach
}

[CreateAssetMenu(menuName = "IdleCA/AchievementData")]
public class AchievementData : ScriptableObject
{
    public string title;
    [TextArea] public string description;
    public AchievementConditionType conditionType;
    public long[] milestoneValues;      // [5, 10, 15, 20, ...]
    public long[] goldRewards;
    public int[] skillPointRewards;
}
