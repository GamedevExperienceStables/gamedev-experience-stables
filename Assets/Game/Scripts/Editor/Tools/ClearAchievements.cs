using Game.Steam;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public static class ClearAchievements
    {
        [MenuItem("Tools/Achievements/Clear", priority = 10)]
        private static void ClearPrefs()
        {
            var steam = new SteamService();
            
            if (steam.ClearStatsAndAchievements())
                Debug.Log("Achievements cleared");

            steam.Dispose();
        }
    }
}