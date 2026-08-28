using UnityEngine;

public class AchievementPanel : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject slotPrefab;

    AchievementSlotUI[] _slots;

    void OnEnable()
    {
        EventBus.Subscribe<AchievementUnlockedEvent>(OnUnlocked);
        if (_slots == null) BuildSlots();
        else foreach (var s in _slots) s.Refresh();
    }

    void OnDisable() => EventBus.Unsubscribe<AchievementUnlockedEvent>(OnUnlocked);

    void BuildSlots()
    {
        var all = AchievementManager.Instance.GetAll();
        _slots = new AchievementSlotUI[all.Length];
        for (int i = 0; i < all.Length; i++)
        {
            var go = Instantiate(slotPrefab, container);
            _slots[i] = go.GetComponent<AchievementSlotUI>();
            _slots[i].Init(all[i]);
        }
    }

    void OnUnlocked(AchievementUnlockedEvent _)
    {
        foreach (var s in _slots) s.Refresh();
    }
}
