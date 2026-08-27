using UnityEngine;

[CreateAssetMenu(menuName = "IdleCA/PetData")]
public class PetData : ScriptableObject
{
    public string petName;
    public Sprite sprite;
    public StatBonus passiveBonus;
    [TextArea] public string unlockCondition;
}
