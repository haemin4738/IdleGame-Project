using System;
using UnityEngine;

public enum StatType { ATK, HP, DEF, Speed }

[DefaultExecutionOrder(-50)]
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    // 업그레이드당 증가량
    const float BONUS_ATK   = 5f;
    const float BONUS_HP    = 50f;
    const float BONUS_DEF   = 3f;
    const float BONUS_SPEED = 0.1f;

    // 스탯별 기본 비용
    const long BASE_COST_ATK   = 100;
    const long BASE_COST_HP    = 80;
    const long BASE_COST_DEF   = 120;
    const long BASE_COST_SPEED = 150;

    const float COST_MULTIPLIER = 1.15f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public int GetLevel(StatType stat)
    {
        var key = stat.ToString();
        var list = SaveManager.Instance.Data.upgradeLevels;
        var entry = list.Find(e => e.key == key);
        return entry?.value ?? 0;
    }

    public long GetCost(StatType stat)
    {
        var level = GetLevel(stat);
        var baseCost = stat switch
        {
            StatType.ATK   => BASE_COST_ATK,
            StatType.HP    => BASE_COST_HP,
            StatType.DEF   => BASE_COST_DEF,
            StatType.Speed => BASE_COST_SPEED,
            _ => BASE_COST_ATK
        };
        return (long)(baseCost * Math.Pow(COST_MULTIPLIER, level));
    }

    // 현재 스탯에 업그레이드 보너스를 더한 총합 반환
    public float GetTotalBonus(StatType stat)
    {
        var level = GetLevel(stat);
        return stat switch
        {
            StatType.ATK   => level * BONUS_ATK,
            StatType.HP    => level * BONUS_HP,
            StatType.DEF   => level * BONUS_DEF,
            StatType.Speed => level * BONUS_SPEED,
            _ => 0f
        };
    }

    public bool TryUpgrade(StatType stat)
    {
        var cost = GetCost(stat);
        var data = SaveManager.Instance.Data;
        if (data.gold < cost) return false;

        data.gold -= cost;
        SetLevel(stat, GetLevel(stat) + 1);

        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        return true;
    }

    void SetLevel(StatType stat, int level)
    {
        var key = stat.ToString();
        var list = SaveManager.Instance.Data.upgradeLevels;
        var entry = list.Find(e => e.key == key);
        if (entry != null)
            entry.value = level;
        else
            list.Add(new UpgradeEntry { key = key, value = level });
    }
}
