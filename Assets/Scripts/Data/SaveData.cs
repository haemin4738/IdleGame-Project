using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int playerLevel = 1;
    public long playerExp = 0;
    public long gold = 0;
    public int currentStageIndex = 0;
    public Dictionary<string, int> upgradeLevels = new();
    public List<string> unlockedSkills = new();
    public List<string> ownedPets = new();
    public long lastSaveTimestamp = 0; // Unix time — OfflineManager가 경과시간 계산에 사용
}
