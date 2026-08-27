using UnityEngine;

public class MainTabController : MonoBehaviour
{
    [SerializeField] GameObject battlePanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject petPanel;
    [SerializeField] GameObject achievementPanel;

    GameObject _activePanel;

    void Start() => HideAll();

    public void OnTabBattle()      => Toggle(battlePanel);
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
        _activePanel = target;
    }

    void HideAll()
    {
        battlePanel.SetActive(false);
        upgradePanel.SetActive(false);
        skillPanel.SetActive(false);
        petPanel.SetActive(false);
        achievementPanel.SetActive(false);
    }
}
