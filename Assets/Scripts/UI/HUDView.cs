using TMPro;
using UnityEngine;

public class HUDView : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text levelText;

    void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
        Refresh();
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
        EventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
    }

    void Refresh()
    {
        var data = SaveManager.Instance.Data;
        goldText.text  = $"골드: {data.gold:N0}";
        levelText.text = $"Lv.{data.playerLevel}";
    }

    void OnGoldChanged(GoldChangedEvent e) => goldText.text = $"골드: {e.NewAmount:N0}";
    void OnLevelUp(LevelUpEvent e)         => levelText.text = $"Lv.{e.NewLevel}";
}
