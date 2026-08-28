using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] CharacterData characterData;

    float _characterHp;
    float _monsterHp;
    MonsterData _currentMonster;
    bool _isBattling;

    const float REVIVE_DELAY = 3f;
    const float ATTACK_INTERVAL = 1f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        _characterHp = characterData.baseHP;
        LoadMonster(StageManager.Instance.CurrentStage.ActiveMonster);
    }

    public void LoadMonster(MonsterData monster)
    {
        StopAllCoroutines();
        _currentMonster = monster;
        _monsterHp = monster.hp;
        _isBattling = true;
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop()
    {
        var wait = new WaitForSeconds(ATTACK_INTERVAL);
        while (_isBattling)
        {
            yield return wait;
            AttackMonster();
            if (!_isBattling) break;
            MonsterAttack();
        }
    }

    void AttackMonster()
    {
        float dmg = Mathf.Max(1f, characterData.baseATK - _currentMonster.def);
        _monsterHp -= dmg;
        EventBus.Publish(new DamageEvent { Target = DamageTarget.Monster, Amount = dmg });
        if (_monsterHp <= 0) OnMonsterKilled();
    }

    void MonsterAttack()
    {
        float dmg = Mathf.Max(1f, _currentMonster.atk - characterData.baseDEF);
        _characterHp -= dmg;
        EventBus.Publish(new DamageEvent { Target = DamageTarget.Character, Amount = dmg });
        if (_characterHp <= 0) StartCoroutine(Revive());
    }

    void OnMonsterKilled()
    {
        _isBattling = false;
        StopAllCoroutines();

        var gold = _currentMonster.goldReward;
        var exp = _currentMonster.expReward;
        SaveManager.Instance.Data.gold += gold;

        EventBus.Publish(new MonsterKilledEvent
        {
            MonsterName = _currentMonster.monsterName,
            ExpReward = exp,
            GoldReward = gold
        });
        EventBus.Publish(new GoldChangedEvent { NewAmount = SaveManager.Instance.Data.gold });

        // 같은 몬스터 재소환 — 스테이지 전환은 StageManager가 처리
        LoadMonster(_currentMonster);
    }

    IEnumerator Revive()
    {
        _isBattling = false;
        StopAllCoroutines();
        yield return new WaitForSeconds(REVIVE_DELAY);
        _characterHp = characterData.baseHP;
        LoadMonster(_currentMonster);
    }

    public float CharacterHpRatio => _characterHp / characterData.baseHP;
    public float MonsterHpRatio => _currentMonster != null ? _monsterHp / _currentMonster.hp : 0f;
}
