using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Melee, Ranged }

[CreateAssetMenu(menuName = "IdleCA/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSpeed;
    public AttackType attackType;
    public int unlockCost;
    public List<SkillData> ownedSkills;
    [Range(0f, 100f)] public float baseCritChance = 10f;
    public float baseCritMultiplier = 1.5f;
    public GameObject characterPrefab;
}
