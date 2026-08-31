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
        else
            data.currentStageIndex = 0; // 현재 마지막 스테이지(1-5)이므로 1-1로 루프백
            // TODO: 2스테이지(2-1 이후) 추가 시 아래 주석 해제하고 위 루프백 제거
            // stages 배열에 2-1, 2-2... 순서대로 추가하면 위 if 분기가 자동으로 다음 스테이지로 이동함
            // data.currentStageIndex++; // 2스테이지 추가 후 사용

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
    public int KillCount => _killCount;
}
