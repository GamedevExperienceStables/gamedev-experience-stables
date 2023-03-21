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
    }
}