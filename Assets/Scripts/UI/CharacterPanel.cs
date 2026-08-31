using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] CharacterSlotUI[] slots;

    void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        var chars = CharacterManager.Instance.GetAll();
        for (int i = 0; i < slots.Length && i < chars.Length; i++)
            slots[i].Init(chars[i]);
    }

    void OnDisable() => EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);

    void OnGoldChanged(GoldChangedEvent _)
    {
        foreach (var s in slots) s.Refresh();
    }
}
