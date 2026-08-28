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
    }

    void OnDestroy() => EventBus.Unsubscribe<MonsterKilledEvent>(OnMonsterKilled);

    void OnMonsterKilled(MonsterKilledEvent e)
    {
        var data = SaveManager.Instance.Data;
        data.totalMonstersKilled++;
        data.totalGoldEarned += e.GoldReward;
    }

    // 수령 완료한 마일스톤 수
    public int GetClaimedCount(AchievementData a)
    {
        var entry = SaveManager.Instance.Data.achievementMilestones.Find(e => e.key == a.name);
        return entry?.value ?? 0;
    }

    // 현재 진행 값
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

    public bool IsAllDone(AchievementData a) =>
        GetClaimedCount(a) >= a.milestoneValues.Length;

    public bool CanClaim(AchievementData a)
    {
        int claimed = GetClaimedCount(a);
        if (claimed >= a.milestoneValues.Length) return false;
        return GetProgress(a) >= a.milestoneValues[claimed];
    }

    public void Claim(AchievementData a)
    {
        if (!CanClaim(a)) return;

        int claimed = GetClaimedCount(a);
        var data = SaveManager.Instance.Data;

        data.gold += a.goldRewards[claimed];
        data.skillPoints += a.skillPointRewards[claimed];

        var list = data.achievementMilestones;
        var entry = list.Find(e => e.key == a.name);
        if (entry != null) entry.value++;
        else list.Add(new UpgradeEntry { key = a.name, value = 1 });

        EventBus.Publish(new AchievementUnlockedEvent { AchievementId = a.name, Title = a.title });
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        if (a.skillPointRewards[claimed] > 0)
            EventBus.Publish(new SkillPointsChangedEvent { Points = data.skillPoints });
    }

    public AchievementData[] GetAll() => achievements;
}
