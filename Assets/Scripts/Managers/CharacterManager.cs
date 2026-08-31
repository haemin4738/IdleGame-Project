using UnityEngine;

[DefaultExecutionOrder(-48)]
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] CharacterData[] allCharacters;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public CharacterData ActiveCharacter
    {
        get
        {
            var name = SaveManager.Instance.Data.selectedCharacterName;
            foreach (var c in allCharacters)
                if (c.characterName == name) return c;
            return allCharacters[0];
        }
    }

    public bool IsOwned(CharacterData c)
    {
        if (c == allCharacters[0]) return true; // 기본 캐릭터는 항상 보유
        return SaveManager.Instance.Data.ownedCharacters.Contains(c.characterName);
    }

    public bool TryBuy(CharacterData c)
    {
        if (IsOwned(c)) return false;
        var data = SaveManager.Instance.Data;
        if (data.gold < c.unlockCost) return false;
        data.gold -= c.unlockCost;
        data.ownedCharacters.Add(c.characterName);
        EventBus.Publish(new GoldChangedEvent { NewAmount = data.gold });
        return true;
    }

    public void Select(CharacterData c)
    {
        if (!IsOwned(c)) return;
        SaveManager.Instance.Data.selectedCharacterName = c.characterName;
    }

    public CharacterData[] GetAll() => allCharacters;
}
