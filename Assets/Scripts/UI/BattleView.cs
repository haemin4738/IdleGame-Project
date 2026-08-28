using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    [SerializeField] Image monsterImage;
    [SerializeField] Slider characterHpBar;
    [SerializeField] Slider monsterHpBar;
    [SerializeField] TMP_Text stageText;
    [SerializeField] TMP_Text monsterNameText;

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
        characterHpBar.value = 1f;
        monsterHpBar.value = 1f;
        var stage = StageManager.Instance.CurrentStage;
        stageText.text = stage.StageName;
        UpdateMonsterDisplay(stage.ActiveMonster);
    }

    void OnDamage(DamageEvent e)
    {
        if (e.Target == DamageTarget.Character)
            characterHpBar.value = BattleManager.Instance.CharacterHpRatio;
        else
            monsterHpBar.value = BattleManager.Instance.MonsterHpRatio;
    }

    void OnStageChanged(StageChangedEvent e)
    {
        stageText.text = e.StageName;
        UpdateMonsterDisplay(StageManager.Instance.CurrentStage.ActiveMonster);
        monsterHpBar.value = 1f;
    }

    void OnMonsterKilled(MonsterKilledEvent _) => monsterHpBar.value = 1f;

    void UpdateMonsterDisplay(MonsterData monster)
    {
        monsterNameText.text = monster.monsterName;
        if (monster.sprite != null)
            monsterImage.sprite = monster.sprite;
    }
}
