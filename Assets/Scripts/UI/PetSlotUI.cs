using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text bonusText;
    [SerializeField] TMP_Text statusText;
    [SerializeField] Button unlockButton;

    PetData _pet;

    public void Init(PetData pet)
    {
        _pet = pet;
        unlockButton.onClick.AddListener(OnUnlock);
        Refresh();
    }

    public void Refresh()
    {
        var owned      = PetManager.Instance.IsOwned(_pet);
        var unlockable = PetManager.Instance.IsUnlockable(_pet);
        nameText.text  = _pet.petName;
        bonusText.text = BuildBonusText(_pet.passiveBonus);
        statusText.text = owned      ? "보유중" :
                          unlockable ? "해금 가능" :
                                       $"1-{_pet.unlockStageIndex + 1} 클리어 필요";
        unlockButton.gameObject.SetActive(!owned && unlockable);
    }

    void OnUnlock()
    {
        PetManager.Instance.Unlock(_pet);
        Refresh();
    }

    string BuildBonusText(StatBonus b)
    {
        var parts = new System.Collections.Generic.List<string>();
        if (b.atkPercent     != 0) parts.Add($"ATK +{b.atkPercent}%");
        if (b.hpPercent      != 0) parts.Add($"HP +{b.hpPercent}%");
        if (b.goldPercent    != 0) parts.Add($"골드 +{b.goldPercent}%");
        if (b.expPercent     != 0) parts.Add($"EXP +{b.expPercent}%");
        if (b.defFlat        != 0) parts.Add($"DEF +{b.defFlat}");
        if (b.critChanceFlat != 0) parts.Add($"크리확률 +{b.critChanceFlat}%");
        if (b.critDamageFlat != 0) parts.Add($"크리배율 +{b.critDamageFlat:F2}x");
        return string.Join(", ", parts);
    }
}
