using TMPro;
using UnityEngine;

public class HUDView : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text expText;
    [SerializeField] RectTransform expBarFill;

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
        expText.text = $"{(float)data.playerExp / required * 100:F0}%";
        SetExpBar((float)data.playerExp / required);
    }

    void SetExpBar(float ratio)
    {
        if (expBarFill == null) return;
        expBarFill.localScale = new Vector3(Mathf.Clamp01(ratio), 1f, 1f);
    }

    void OnGoldChanged(GoldChangedEvent e)    => goldText.text = $"골드: {e.NewAmount:N0}";
    void OnLevelUp(LevelUpEvent e)            => levelText.text = $"Lv.{e.NewLevel}";
    void OnExpChanged(PlayerExpChangedEvent e)
    {
        expText.text = $"{(float)e.CurrentExp / e.RequiredExp * 100:F0}%";
        SetExpBar((float)e.CurrentExp / e.RequiredExp);
    }
}
