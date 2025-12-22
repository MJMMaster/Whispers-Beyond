using System.Collections.Generic;
using UnityEngine;

public static class BuildingUnlockManager
{
    private static HashSet<string> unlockedBuildings = new HashSet<string>();

    public static bool IsUnlocked(string buildingID)
    {
        return unlockedBuildings.Contains(buildingID);
    }

    public static void Unlock(string buildingID)
    {
        if (!unlockedBuildings.Contains(buildingID))
            unlockedBuildings.Add(buildingID);
    }
}