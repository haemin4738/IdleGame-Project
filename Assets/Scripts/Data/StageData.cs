using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IdleCA/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public Sprite backgroundSprite;
    public List<MonsterData> monsterWaves;
    public MonsterData bossMonster;
    public float recommendedATK;
    public float goldMultiplier;
}
