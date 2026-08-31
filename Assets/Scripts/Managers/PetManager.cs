using UnityEngine;

[DefaultExecutionOrder(-50)]
public class PetManager : MonoBehaviour
{
    public static PetManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool IsOwned(PetData pet) =>
        SaveManager.Instance.Data.ownedPets.Contains(pet.petName);

    public bool IsUnlockable(PetData pet) =>
        SaveManager.Instance.Data.maxReachedStageIndex >= pet.unlockStageIndex;

    public void Unlock(PetData pet)
    {
        if (IsOwned(pet) || !IsUnlockable(pet)) return;
        SaveManager.Instance.Data.ownedPets.Add(pet.petName);
    }

    public StatBonus GetTotalBonus(PetData[] pets)
    {
        var total = new StatBonus();
        foreach (var pet in pets)
        {
            if (!IsOwned(pet)) continue;
            total.atkPercent     += pet.passiveBonus.atkPercent;
            total.hpPercent      += pet.passiveBonus.hpPercent;
            total.goldPercent    += pet.passiveBonus.goldPercent;
            total.expPercent     += pet.passiveBonus.expPercent;
            total.defFlat        += pet.passiveBonus.defFlat;
            total.critChanceFlat += pet.passiveBonus.critChanceFlat;
            total.critDamageFlat += pet.passiveBonus.critDamageFlat;
        }
        return total;
    }
}
