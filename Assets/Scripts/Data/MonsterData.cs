using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IdleCA/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public Sprite sprite;
    public float hp;
    public float atk;
    public float def;
    public int expReward;
    public long goldReward;
    public List<EquipmentData> dropTable;
    public bool isBoss;
}
