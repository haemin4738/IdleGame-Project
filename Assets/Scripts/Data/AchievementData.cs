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
    public long conditionValue;
    public long goldReward;
    public int skillPointReward;
}
