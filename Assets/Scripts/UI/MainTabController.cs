using UnityEngine;

public class MainTabController : MonoBehaviour
{
    [SerializeField] GameObject battlePanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject petPanel;
    [SerializeField] GameObject achievementPanel;

    void Start() => ShowTab(upgradePanel);

    public void OnTabBattle()      => ShowTab(battlePanel);
    public void OnTabUpgrade()     => ShowTab(upgradePanel);
    public void OnTabSkill()       => ShowTab(skillPanel);
    public void OnTabPet()         => ShowTab(petPanel);
    public void OnTabAchievement() => ShowTab(achievementPanel);

    void ShowTab(GameObject target)
    {
        battlePanel.SetActive(false);
        upgradePanel.SetActive(false);
        skillPanel.SetActive(false);
        petPanel.SetActive(false);
        achievementPanel.SetActive(false);
        target.SetActive(true);
    }
}
