using UnityEngine;

public enum SkillType { Active, Passive }

[CreateAssetMenu(menuName = "IdleCA/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType skillType;
    public float damage;
    public float cooldown;
    [TextArea] public string effect;
    public int maxLevel;
    public StatBonus statPerLevel;
}
