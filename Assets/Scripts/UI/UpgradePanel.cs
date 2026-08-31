using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [Header("ATK")]
    [SerializeField] TMP_Text atkStatText;
    [SerializeField] TMP_Text atkCostText;
    [SerializeField] Button   atkButton;

    [Header("HP")]
    [SerializeField] TMP_Text hpStatText;
    [SerializeField] TMP_Text hpCostText;
    [SerializeField] Button   hpButton;

    [Header("DEF")]
    [SerializeField] TMP_Text defStatText;
    [SerializeField] TMP_Text defCostText;
    [SerializeField] Button   defButton;

    [Header("Speed")]
    [SerializeField] TMP_Text speedStatText;
    [SerializeField] TMP_Text speedCostText;
    [SerializeField] Button   speedButton;

    void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        atkButton.onClick.AddListener(()   => OnUpgrade(StatType.ATK));
        hpButton.onClick.AddListener(()    => OnUpgrade(StatType.HP));
        defButton.onClick.AddListener(()   => OnUpgrade(StatType.DEF));
        speedButton.onClick.AddListener(() => OnUpgrade(StatType.Speed));
        Refresh();
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
        atkButton.onClick.RemoveAllListeners();
        hpButton.onClick.RemoveAllListeners();
        defButton.onClick.RemoveAllListeners();
        speedButton.onClick.RemoveAllListeners();
    }

    void OnUpgrade(StatType stat)
    {
        if (!UpgradeManager.Instance.TryUpgrade(stat))
            ToastPopup.Instance.Show("골드가 부족합니다");
        Refresh();
    }

    void OnGoldChanged(GoldChangedEvent _) => Refresh();

    void Refresh()
    {
        var um = UpgradeManager.Instance;
        atkStatText.text   = $"ATK +{um.GetTotalBonus(StatType.ATK)}";
        atkCostText.text   = $"{um.GetCost(StatType.ATK):N0}G";
        hpStatText.text    = $"HP +{um.GetTotalBonus(StatType.HP)}";
        hpCostText.text    = $"{um.GetCost(StatType.HP):N0}G";
        defStatText.text   = $"DEF +{um.GetTotalBonus(StatType.DEF)}";
        defCostText.text   = $"{um.GetCost(StatType.DEF):N0}G";
        speedStatText.text = $"Speed +{um.GetTotalBonus(StatType.Speed):F1}";
        speedCostText.text = $"{um.GetCost(StatType.Speed):N0}G";
    }
}
