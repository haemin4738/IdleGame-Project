using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text rarityText;
    [SerializeField] TMP_Text bonusText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] Button   actionButton;
    [SerializeField] TMP_Text actionButtonText;

    EquipmentData _eq;

    public void Init(EquipmentData eq)
    {
        _eq = eq;
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnAction);
        Refresh();
    }

    public void Refresh()
    {
        var em      = EquipmentManager.Instance;
        bool owned    = em.IsOwned(_eq);
        bool equipped = em.IsEquipped(_eq);
        int  level    = em.GetLevel(_eq);

        nameText.text   = _eq.equipmentName;
        rarityText.text = _eq.rarity switch
        {
            Rarity.Normal => "일반",
            Rarity.Rare   => "희귀",
            Rarity.Hero   => "영웅",
            _             => "전설"
        };
        bonusText.text = BuildBonusText(_eq.baseStats, level);
        levelText.text = owned ? $"Lv.{level}/{_eq.upgradeMaxLevel}" : "";

        if (!owned)
        {
            actionButtonText.text     = $"구매 {em.GetBuyCost(_eq):N0}G";
            actionButton.interactable = SaveManager.Instance.Data.gold >= em.GetBuyCost(_eq);
        }
        else if (!equipped)
        {
            actionButtonText.text     = "장착";
            actionButton.interactable = true;
        }
        else
        {
            bool canUpgrade           = level < _eq.upgradeMaxLevel;
            actionButtonText.text     = canUpgrade ? $"강화 {em.GetUpgradeCost(_eq):N0}G" : "최대";
            actionButton.interactable = canUpgrade && SaveManager.Instance.Data.gold >= em.GetUpgradeCost(_eq);
        }
    }

    void OnAction()
    {
        var em = EquipmentManager.Instance;
        if (!em.IsOwned(_eq))
        {
            if (!em.TryBuy(_eq)) ToastPopup.Instance.Show("골드가 부족합니다");
        }
        else if (!em.IsEquipped(_eq))
        {
            em.Equip(_eq);
        }
        else
        {
            if (!em.TryUpgrade(_eq)) ToastPopup.Instance.Show("골드가 부족합니다");
        }
        Refresh();
    }

    string BuildBonusText(StatBonus b, int level)
    {
        float m = 1f + level * 0.1f;
        if (b.atkPercent     != 0) return $"ATK +{b.atkPercent * m:F0}%";
        if (b.hpPercent      != 0) return $"HP +{b.hpPercent * m:F0}%";
        if (b.defFlat        != 0) return $"DEF +{b.defFlat * m:F0}";
        if (b.goldPercent    != 0) return $"골드 +{b.goldPercent * m:F0}%";
        if (b.expPercent     != 0) return $"EXP +{b.expPercent * m:F0}%";
        if (b.critChanceFlat != 0) return $"크리확률 +{b.critChanceFlat * m:F0}%";
        if (b.critDamageFlat != 0) return $"크리배율 +{b.critDamageFlat * m:F2}x";
        return "";
    }
}
