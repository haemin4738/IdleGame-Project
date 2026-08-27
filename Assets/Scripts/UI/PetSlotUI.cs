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
        var owned = PetManager.Instance.IsOwned(_pet);
        nameText.text   = _pet.petName;
        bonusText.text  = BuildBonusText(_pet.passiveBonus);
        statusText.text = owned ? "보유중" : "미보유";
        unlockButton.gameObject.SetActive(!owned);
    }

    void OnUnlock()
    {
        PetManager.Instance.Unlock(_pet);
        Refresh();
    }

    string BuildBonusText(StatBonus b)
    {
        if (b.atkPercent  != 0) return $"ATK +{b.atkPercent}%";
        if (b.hpPercent   != 0) return $"HP +{b.hpPercent}%";
        if (b.goldPercent != 0) return $"골드 +{b.goldPercent}%";
        if (b.expPercent  != 0) return $"EXP +{b.expPercent}%";
        if (b.defFlat     != 0) return $"DEF +{b.defFlat}";
        return "";
    }
}
