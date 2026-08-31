using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text statsText;
    [SerializeField] TMP_Text actionButtonText;
    [SerializeField] Button actionButton;

    CharacterData _data;

    public void Init(CharacterData data)
    {
        _data = data;
        actionButton.onClick.AddListener(OnAction);
        Refresh();
    }

    public void Refresh()
    {
        var cm = CharacterManager.Instance;
        bool owned    = cm.IsOwned(_data);
        bool selected = cm.ActiveCharacter == _data;

        nameText.text  = _data.characterName;
        statsText.text = $"ATK {_data.baseATK:F0}  HP {_data.baseHP:F0}  DEF {_data.baseDEF:F0}";

        if (!owned)
        {
            actionButtonText.text     = $"구매 {_data.unlockCost:N0}G";
            actionButton.interactable = SaveManager.Instance.Data.gold >= _data.unlockCost;
        }
        else if (selected)
        {
            actionButtonText.text     = "선택됨";
            actionButton.interactable = false;
        }
        else
        {
            actionButtonText.text     = "선택";
            actionButton.interactable = true;
        }
    }

    void OnAction()
    {
        var cm = CharacterManager.Instance;
        if (!cm.IsOwned(_data))
        {
            if (!cm.TryBuy(_data))
                ToastPopup.Instance.Show("골드가 부족합니다");
        }
        else
        {
            cm.Select(_data);
        }
        Refresh();
    }
}
