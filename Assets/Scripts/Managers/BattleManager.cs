using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] CharacterData characterData;
    [SerializeField] PetData[] allPets;
    [SerializeField] SkillData[] allSkills;

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

    StatBonus PetBonus   => PetManager.Instance.GetTotalBonus(allPets);
    StatBonus SkillBonus => SkillManager.Instance.GetTotalPassiveBonus(allSkills);

    float MaxHp => (characterData.baseHP + UpgradeManager.Instance.GetTotalBonus(StatType.HP))
                   * (1f + (PetBonus.hpPercent + SkillBonus.hpPercent) / 100f);

    void Start()
    {
        _characterHp = MaxHp;
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
        var pb = PetBonus; var sb = SkillBonus;
        float atk = (characterData.baseATK + UpgradeManager.Instance.GetTotalBonus(StatType.ATK))
                    * (1f + (pb.atkPercent + sb.atkPercent) / 100f);
        bool isCrit = Random.value * 100f < characterData.baseCritChance;
        float dmg = Mathf.Max(1f, atk - _currentMonster.def);
        if (isCrit) dmg *= characterData.baseCritMultiplier;
        _monsterHp -= dmg;
        EventBus.Publish(new DamageEvent { Target = DamageTarget.Monster, Amount = dmg, IsCrit = isCrit });
        if (_monsterHp <= 0) OnMonsterKilled();
    }

    void MonsterAttack()
    {
        var pb = PetBonus; var sb = SkillBonus;
        float def = characterData.baseDEF + UpgradeManager.Instance.GetTotalBonus(StatType.DEF)
                    + pb.defFlat + sb.defFlat;
        float dmg = Mathf.Max(1f, _currentMonster.atk - def);
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
        _characterHp = MaxHp;
        LoadMonster(_currentMonster);
    }

    public float CharacterHpRatio => _characterHp / MaxHp;
    public float MonsterHpRatio => _currentMonster != null ? _monsterHp / _currentMonster.hp : 0f;
}
