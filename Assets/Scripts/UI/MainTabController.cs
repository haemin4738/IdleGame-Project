using UnityEngine;

public class MainTabController : MonoBehaviour
{
    [SerializeField] GameObject playerInfoPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject petPanel;
    [SerializeField] GameObject equipmentPanel;
    [SerializeField] GameObject characterPanel;

    GameObject _activePanel;

    void Start() => HideAll();

    public void OnTabPlayerInfo()  => Toggle(playerInfoPanel);
    public void OnTabUpgrade()     => Toggle(upgradePanel);
    public void OnTabSkill()       => Toggle(skillPanel);
    public void OnTabPet()         => Toggle(petPanel);
    public void OnTabEquipment()   => Toggle(equipmentPanel);
    public void OnTabCharacter()   => Toggle(characterPanel);

    void Toggle(GameObject target)
    {
        if (_activePanel == target)
        {
            target.SetActive(false);
            _activePanel = null;
            return;
        }
        HideAll();
        target.SetActive(true);
        target.transform.SetAsLastSibling();
        _activePanel = target;
    }

    void HideAll()
    {
        playerInfoPanel.SetActive(false);
        upgradePanel.SetActive(false);
        skillPanel.SetActive(false);
        petPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        characterPanel.SetActive(false);
    }
}
