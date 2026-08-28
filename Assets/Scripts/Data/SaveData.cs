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
    public List<UpgradeEntry> upgradeLevels = new();
    public int skillPoints = 0;
    public List<UpgradeEntry> skillLevels = new();
    public List<string> ownedPets = new();
    public List<string> completedAchievements = new();
    public long totalMonstersKilled = 0;
    public long totalGoldEarned = 0;
    public long lastSaveTimestamp = 0;
}
