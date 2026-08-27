using TMPro;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] SkillData[] skills;
    [SerializeField] Transform container;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] TMP_Text pointsText;

    SkillSlotUI[] _slots;

    void OnEnable()
    {
        EventBus.Subscribe<SkillPointsChangedEvent>(OnPointsChanged);
        if (_slots == null) BuildSlots();
        Refresh();
    }

    void OnDisable() => EventBus.Unsubscribe<SkillPointsChangedEvent>(OnPointsChanged);

    void BuildSlots()
    {
        _slots = new SkillSlotUI[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            var go = Instantiate(slotPrefab, container);
            _slots[i] = go.GetComponent<SkillSlotUI>();
            _slots[i].Init(skills[i]);
        }
    }

    void Refresh()
    {
        pointsText.text = $"스킬 포인트: {SkillManager.Instance.GetSkillPoints()}";
        if (_slots == null) return;
        foreach (var slot in _slots) slot.Refresh();
    }

    void OnPointsChanged(SkillPointsChangedEvent e)
    {
        pointsText.text = $"스킬 포인트: {e.Points}";
        foreach (var slot in _slots) slot.Refresh();
    }
}
