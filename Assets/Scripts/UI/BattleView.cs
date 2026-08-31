using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    [SerializeField] Image monsterImage;
    [SerializeField] RectTransform characterHpBarFill;
    [SerializeField] RectTransform monsterHpBarFill;
    [SerializeField] TMP_Text stageText;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] DamagePopup popupPrefab;
    [SerializeField] RectTransform monsterPopupAnchor;
    [SerializeField] RectTransform characterPopupAnchor;

    void OnEnable()
    {
        EventBus.Subscribe<DamageEvent>(OnDamage);
        EventBus.Subscribe<StageChangedEvent>(OnStageChanged);
        EventBus.Subscribe<MonsterKilledEvent>(OnMonsterKilled);
        Refresh();
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<DamageEvent>(OnDamage);
        EventBus.Unsubscribe<StageChangedEvent>(OnStageChanged);
        EventBus.Unsubscribe<MonsterKilledEvent>(OnMonsterKilled);
    }

    void Refresh()
    {
        SetBar(characterHpBarFill, 1f);
        SetBar(monsterHpBarFill, 1f);
        var stage = StageManager.Instance.CurrentStage;
        stageText.text = stage.StageName;
        UpdateMonsterDisplay(stage.ActiveMonster);
        UpdateKillCount();
    }

    void UpdateKillCount()
    {
        if (killCountText == null) return;
        var stage = StageManager.Instance.CurrentStage;
        killCountText.text = $"{StageManager.Instance.KillCount}/{stage.RequiredKills}";
    }

    void SetBar(RectTransform bar, float ratio)
    {
        if (bar == null) return;
        bar.localScale = new Vector3(Mathf.Clamp01(ratio), 1f, 1f);
    }

    void OnDamage(DamageEvent e)
    {
        if (e.Target == DamageTarget.Character)
        {
            SetBar(characterHpBarFill, BattleManager.Instance.CharacterHpRatio);
            SpawnPopup(characterPopupAnchor, e.Amount, false);
        }
        else
        {
            SetBar(monsterHpBarFill, BattleManager.Instance.MonsterHpRatio);
            SpawnPopup(monsterPopupAnchor, e.Amount, e.IsCrit);
        }
    }

    void SpawnPopup(RectTransform anchor, float damage, bool isCrit)
    {
        if (popupPrefab == null || anchor == null) return;
        Instantiate(popupPrefab, anchor).Show(damage, isCrit);
    }

    void OnStageChanged(StageChangedEvent e)
    {
        stageText.text = e.StageName;
        UpdateMonsterDisplay(StageManager.Instance.CurrentStage.ActiveMonster);
        SetBar(monsterHpBarFill, 1f);
        UpdateKillCount();
    }

    void OnMonsterKilled(MonsterKilledEvent _)
    {
        SetBar(monsterHpBarFill, 1f);
        UpdateKillCount();
    }

    void UpdateMonsterDisplay(MonsterData monster)
    {
        if (monster.sprite != null)
            monsterImage.sprite = monster.sprite;
    }
}
