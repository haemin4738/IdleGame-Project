// EventBus에서 사용하는 이벤트 타입 정의

public struct MonsterKilledEvent
{
    public string MonsterName;
    public int ExpReward;
    public long GoldReward;
}

public struct GoldChangedEvent
{
    public long NewAmount;
}

public struct LevelUpEvent
{
    public int NewLevel;
}

public struct StageClearedEvent
{
    public int StageIndex;
}

public struct StageChangedEvent
{
    public int StageIndex;
    public string StageName;
}

public struct SkillPointsChangedEvent
{
    public int Points;
}

public struct AchievementUnlockedEvent
{
    public string AchievementId;
    public string Title;
}

public enum DamageTarget { Character, Monster }

public struct DamageEvent
{
    public DamageTarget Target;
    public float Amount;
}
