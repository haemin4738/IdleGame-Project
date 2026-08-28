using UnityEngine;

public class PetPanel : MonoBehaviour
{
    [SerializeField] PetData[] pets;
    [SerializeField] Transform container;
    [SerializeField] GameObject slotPrefab;

    PetSlotUI[] _slots;

    void OnEnable()
    {
        if (_slots == null) BuildSlots();
        else foreach (var slot in _slots) slot.Refresh();
    }

    void BuildSlots()
    {
        _slots = new PetSlotUI[pets.Length];
        for (int i = 0; i < pets.Length; i++)
        {
            var go = Instantiate(slotPrefab, container);
            _slots[i] = go.GetComponent<PetSlotUI>();
            _slots[i].Init(pets[i]);
        }
    }
}
