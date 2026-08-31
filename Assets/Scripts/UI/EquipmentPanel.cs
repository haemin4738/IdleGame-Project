using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] EquipmentData[]   allEquipment;
    [SerializeField] EquipmentSlotUI[] slots;

    void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        for (int i = 0; i < slots.Length && i < allEquipment.Length; i++)
            slots[i].Init(allEquipment[i]);
    }

    void OnDisable() => EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);

    void OnGoldChanged(GoldChangedEvent _)
    {
        foreach (var slot in slots) slot.Refresh();
    }
}
