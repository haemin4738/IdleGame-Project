using UnityEngine;

[DefaultExecutionOrder(-40)]
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] AchievementData[] achievements;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        EventBus.Subscribe<MonsterKilledEvent>(OnMonsterKilled);
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
        EventBus.Subscribe<StageClearedEvent>(OnStageCleared);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<MonsterKilledEvent>(OnMonsterKilled);
        EventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
        EventBus.Unsubscribe<StageClearedEvent>(OnStageCleared);
    }

    void OnMonsterKilled(MonsterKilledEvent e)
    {
        var data = SaveManager.Instance.Data;
        data.totalMonstersKilled++;
        data.totalGoldEarned += e.GoldReward;
        CheckAll();
    }

    void OnLevelUp(LevelUpEvent _) => CheckAll();
    void OnStageCleared(StageClearedEvent _) => CheckAll();

    void CheckAll()
    {
        var data = SaveManager.Instance.Data;
        foreach (var a in achievements)
        {
            if (data.completedAchievements.Contains(a.name)) continue;
            if (GetProgress(a) >= a.conditionValue) Unlock(a);
        }
    }

    void Unlock(AchievementData a)
    {
        var data = SaveManager.Instance.Data;
        data.completedAchievements.Add(a.name);
        data.gold += a.goldReward;
        data.skillPoints += a.skillPointReward;
        EventBus.Publish(new AchievementUnlockedEvent { AchievementId = a.name, Title = a.title });
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        if (a.skillPointReward > 0)
            EventBus.Publish(new SkillPointsChangedEvent { Points = data.skillPoints });
    }

    public long GetProgress(AchievementData a)
    {
        var data = SaveManager.Instance.Data;
        return a.conditionType switch
        {
            AchievementConditionType.MonsterKillCount => data.totalMonstersKilled,
            AchievementConditionType.GoldEarned       => data.totalGoldEarned,
            AchievementConditionType.LevelReach       => data.playerLevel,
            AchievementConditionType.StageReach       => data.currentStageIndex + 1,
            _ => 0
        };
    }

    public bool IsCompleted(AchievementData a) =>
        SaveManager.Instance.Data.completedAchievements.Contains(a.name);

    public AchievementData[] GetAll() => achievements;
}
