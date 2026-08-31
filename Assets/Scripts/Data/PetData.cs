using UnityEngine;

[CreateAssetMenu(menuName = "IdleCA/PetData")]
public class PetData : ScriptableObject
{
    public string petName;
    public Sprite sprite;
    public StatBonus passiveBonus;
    public int unlockStageIndex; // 이 스테이지 index 이상 도달 시 해금 가능
    [TextArea] public string unlockCondition;
}
