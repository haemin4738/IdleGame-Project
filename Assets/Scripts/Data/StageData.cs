using UnityEngine;

[CreateAssetMenu(menuName = "IdleCA/StageData")]
public class StageData : ScriptableObject
{
    public int world;
    public int stage;               // 1~5
    public bool isBossStage;        // true면 보스 처치 1회로 클리어
    public MonsterData monster;     // 일반 스테이지 몬스터
    public int killsRequired;       // 일반 스테이지 처치 수
    public MonsterData bossMonster; // 보스 스테이지 몬스터
    public Sprite backgroundSprite;
    public float goldMultiplier = 1f;

    public string StageName => $"{world}-{stage}";
    public MonsterData ActiveMonster => isBossStage ? bossMonster : monster;
    public int RequiredKills => isBossStage ? 1 : killsRequired;
}
