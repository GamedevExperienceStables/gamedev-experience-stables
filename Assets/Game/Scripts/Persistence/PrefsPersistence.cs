using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.Persistence
{
    public class PrefsPersistence : IPlayerPrefs
    {
        public void SetFloat(string key, float value)
            => PlayerPrefs.SetFloat(key, value);

        public float GetFloat(string key, float defaultValue)
            => PlayerPrefs.GetFloat(key, defaultValue);

        public void SetString(string key, string value) 
            => PlayerPrefs.SetString(key, value);

        public string GetString(string key, string defaultValue) 
            => PlayerPrefs.GetString(key, defaultValue);
    }
}