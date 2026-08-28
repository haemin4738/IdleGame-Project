using UnityEngine;

[DefaultExecutionOrder(-45)]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] StageData[] stages;

    int _killCount;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        EventBus.Subscribe<MonsterKilledEvent>(OnMonsterKilled);
    }

    void OnDestroy() => EventBus.Unsubscribe<MonsterKilledEvent>(OnMonsterKilled);

    void Start() => PublishCurrentStage();

    void OnMonsterKilled(MonsterKilledEvent _)
    {
        _killCount++;
        if (_killCount >= CurrentStage.RequiredKills)
            AdvanceStage();
    }

    void AdvanceStage()
    {
        var data = SaveManager.Instance.Data;
        EventBus.Publish(new StageClearedEvent { StageIndex = data.currentStageIndex });

        if (data.currentStageIndex < stages.Length - 1)
            data.currentStageIndex++;

        _killCount = 0;
        PublishCurrentStage();
        BattleManager.Instance.LoadMonster(CurrentStage.ActiveMonster);
    }

    void PublishCurrentStage()
    {
        var s = CurrentStage;
        EventBus.Publish(new StageChangedEvent
        {
            StageIndex = SaveManager.Instance.Data.currentStageIndex,
            StageName = s.StageName
        });
    }

    public StageData CurrentStage => stages[SaveManager.Instance.Data.currentStageIndex];
}
