using UnityEngine;

public class MainTabController : MonoBehaviour
{
    [SerializeField] GameObject playerInfoPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject petPanel;
    [SerializeField] GameObject achievementPanel;

    GameObject _activePanel;

    void Start() => HideAll();

    public void OnTabPlayerInfo()  => Toggle(playerInfoPanel);
    public void OnTabUpgrade()     => Toggle(upgradePanel);
    public void OnTabSkill()       => Toggle(skillPanel);
    public void OnTabPet()         => Toggle(petPanel);
    public void OnTabAchievement() => Toggle(achievementPanel);

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
        achievementPanel.SetActive(false);
    }
}
