using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    [SerializeField] Transform monsterAnchor;
    [SerializeField] RectTransform characterHpBarFill;
    [SerializeField] RectTransform monsterHpBarFill;
    [SerializeField] TMP_Text stageText;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] DamagePopup popupPrefab;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform characterPopupAnchor;

    GameObject _spawnedMonster;
    Animator _monsterAnimator;

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
            SpawnMonsterPopup(e.Amount, e.IsCrit);
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        if (projectilePrefab == null || characterPopupAnchor == null) return;
        var target = _spawnedMonster != null
            ? Camera.main.WorldToScreenPoint(_spawnedMonster.transform.position)
            : (Vector3)characterPopupAnchor.position;
        var p = Instantiate(projectilePrefab, transform);
        p.Launch(characterPopupAnchor.position, target);
    }

    [SerializeField] Vector2 monsterPopupOffset = new Vector2(0.75f, 2f);

    void SpawnMonsterPopup(float damage, bool isCrit)
    {
        if (popupPrefab == null || canvas == null || _spawnedMonster == null) return;
        var worldPos = _spawnedMonster.transform.position + new Vector3(monsterPopupOffset.x, monsterPopupOffset.y, 0f);
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), screenPos, null, out var localPos);
        var popup = Instantiate(popupPrefab, canvas.transform);
        popup.GetComponent<RectTransform>().localPosition = localPos;
        popup.Show(damage, isCrit);
    }

    void SpawnPopup(RectTransform anchor, float damage, bool isCrit)
    {
        if (popupPrefab == null || anchor == null) return;
        Instantiate(popupPrefab, anchor).Show(damage, isCrit);
    }

    void OnStageChanged(StageChangedEvent e)
    {
        stageText.text  = e.IsBossStage ? $"BOSS {e.StageName}" : e.StageName;
        stageText.color = e.IsBossStage ? new UnityEngine.Color(1f, 0.3f, 0.3f) : UnityEngine.Color.white;
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
        if (_spawnedMonster != null) Destroy(_spawnedMonster);
        if (monsterAnchor == null || monster.monsterPrefab == null) return;
        _spawnedMonster = Instantiate(monster.monsterPrefab, monsterAnchor.position, Quaternion.identity);
        _monsterAnimator = _spawnedMonster.GetComponentInChildren<Animator>();
    }
}
