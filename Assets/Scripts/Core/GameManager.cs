using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        EventBus.Subscribe<MonsterKilledEvent>(OnMonsterKilled);
    }

    void OnDestroy() => EventBus.Unsubscribe<MonsterKilledEvent>(OnMonsterKilled);

    void OnMonsterKilled(MonsterKilledEvent e)
    {
        var data = SaveManager.Instance.Data;
        data.playerExp += e.ExpReward;

        long required = ExpRequired(data.playerLevel);
        while (data.playerExp >= required)
        {
            data.playerExp -= required;
            data.playerLevel++;
            required = ExpRequired(data.playerLevel);
            EventBus.Publish(new LevelUpEvent { NewLevel = data.playerLevel });
        }

        EventBus.Publish(new PlayerExpChangedEvent
        {
            CurrentExp = data.playerExp,
            RequiredExp = ExpRequired(data.playerLevel),
            Level = data.playerLevel
        });
    }

    public static long ExpRequired(int level) => level * 100L;

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveManager.Instance.Save();
    }

    void OnApplicationQuit() => SaveManager.Instance.Save();
}
