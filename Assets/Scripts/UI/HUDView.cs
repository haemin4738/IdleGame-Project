using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text expText;
    [SerializeField] Image expBarFill;

    void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
        EventBus.Subscribe<PlayerExpChangedEvent>(OnExpChanged);
        Refresh();
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
        EventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
        EventBus.Unsubscribe<PlayerExpChangedEvent>(OnExpChanged);
    }

    void Refresh()
    {
        var data = SaveManager.Instance.Data;
        goldText.text = $"골드: {data.gold:N0}";
        levelText.text = $"Lv.{data.playerLevel}";
        long required = GameManager.ExpRequired(data.playerLevel);
        expText.text = $"EXP: {data.playerExp} / {required}";
        if (expBarFill != null) expBarFill.fillAmount = (float)data.playerExp / required;
    }

    void OnGoldChanged(GoldChangedEvent e)    => goldText.text = $"골드: {e.NewAmount:N0}";
    void OnLevelUp(LevelUpEvent e)            => levelText.text = $"Lv.{e.NewLevel}";
    void OnExpChanged(PlayerExpChangedEvent e)
    {
        expText.text = $"EXP: {e.CurrentExp} / {e.RequiredExp}";
        if (expBarFill != null) expBarFill.fillAmount = (float)e.CurrentExp / e.RequiredExp;
    }
}
