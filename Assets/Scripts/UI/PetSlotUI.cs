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
        if (b.atkPercent     != 0) return $"ATK +{b.atkPercent}%";
        if (b.hpPercent      != 0) return $"HP +{b.hpPercent}%";
        if (b.goldPercent    != 0) return $"골드 +{b.goldPercent}%";
        if (b.expPercent     != 0) return $"EXP +{b.expPercent}%";
        if (b.defFlat        != 0) return $"DEF +{b.defFlat}";
        if (b.critChanceFlat != 0) return $"크리확률 +{b.critChanceFlat}%";
        if (b.critDamageFlat != 0) return $"크리배율 +{b.critDamageFlat:F2}x";
        return "";
    }
}
