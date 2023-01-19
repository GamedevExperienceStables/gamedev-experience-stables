using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public static class PlayerPrefs
    {
        [MenuItem("Tools/Player Prefs/Clear", priority = 10)]
        public static void ClearPrefs()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            UnityEngine.PlayerPrefs.Save();

            Debug.Log("Player Prefs cleared");
        }
    }
}