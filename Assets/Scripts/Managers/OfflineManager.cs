using System;
using UnityEngine;

public class OfflineManager : MonoBehaviour
{
    public static OfflineManager Instance { get; private set; }

    const long MAX_OFFLINE_SECONDS = 12 * 60 * 60; // 12시간 캡

    // 오프라인 보상 계산 결과
    public struct OfflineReward
    {
        public long Gold;
        public long Seconds;
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // GameManager.Initialize()에서 Load() 직후 호출
    public OfflineReward Calculate()
    {
        var data = SaveManager.Instance.Data;
        if (data.lastSaveTimestamp == 0) return default;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var elapsed = Math.Min(now - data.lastSaveTimestamp, MAX_OFFLINE_SECONDS);
        if (elapsed <= 0) return default;

        var goldMult = BattleManager.Instance != null ? BattleManager.Instance.GoldMultiplier : 1f;
        var goldPerSecond = (data.currentStageIndex + 1) * 10L;
        var earned = (long)(elapsed * goldPerSecond * goldMult);

        data.gold += earned;
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });

        return new OfflineReward { Gold = earned, Seconds = elapsed };
    }
}
