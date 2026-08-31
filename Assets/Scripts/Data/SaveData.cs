using System;
using System.Collections.Generic;

[Serializable]
public class UpgradeEntry
{
    public string key;
    public int value;
}

[Serializable]
public class SaveData
{
    public int playerLevel = 1;
    public long playerExp = 0;
    public long gold = 0;
    public int currentStageIndex = 0;
    public int maxReachedStageIndex = 0;
    public List<UpgradeEntry> upgradeLevels = new();
    public int skillPoints = 0;
    public List<UpgradeEntry> skillLevels = new();
    public List<string> ownedPets = new();
    public List<string> ownedEquipment = new();
    public string equippedWeapon = "";
    public string equippedArmor = "";
    public string equippedAccessory = "";
    public List<UpgradeEntry> equipmentLevels = new();
    public List<UpgradeEntry> achievementMilestones = new();  // value = 수령 완료한 마일스톤 수
    public long totalMonstersKilled = 0;
    public long totalGoldEarned = 0;
    public long lastSaveTimestamp = 0;
    public string selectedCharacterName = "";
    public List<string> ownedCharacters = new();
}
