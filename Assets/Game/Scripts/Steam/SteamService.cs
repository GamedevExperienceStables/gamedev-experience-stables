using System;
using Steamworks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.Steam
{
    public sealed class SteamService : ITickable, IDisposable
    {
        private readonly AppId_t _appId = (AppId_t)2440730;

        [Inject]
        public SteamService()
        {
            if (!Packsize.Test())
                Debug.LogError(
                    "[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");

            if (!DllCheck.Test())
                Debug.LogError(
                    "[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");

            try
            {
                if (SteamAPI.RestartAppIfNecessary(_appId))
                {
                    Application.Quit();
                    return;
                }
            }
            catch (DllNotFoundException e)
            {
                Debug.LogError(
                    $"[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n{e}");

                Application.Quit();
                return;
            }

            bool init = SteamAPI.Init();
            if (init)
            {
#if UNITY_EDITOR
                Debug.Log("[Steamworks.NET] SteamAPI_Init() succeeded.");
#endif
                return;
            }

            Debug.LogError(
                "[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
        }

        public void Tick()
            => SteamAPI.RunCallbacks();

        public void Dispose()
        {
#if UNITY_EDITOR
            Debug.Log("[Steamworks.NET] Disposing SteamAPI");
#endif
            SteamAPI.Shutdown();
        }


        public Callback<T> CreateCallback<T>(Callback<T>.DispatchDelegate func)
            => Callback<T>.Create(func);

        public void SetAchievement(string achievement)
        {
#if UNITY_EDITOR
            Debug.Log($"[Steamworks.NET] SetAchievement: {achievement}");
#endif
            SteamUserStats.SetAchievement(achievement);
            SteamUserStats.StoreStats();
        }

        public void RemoveAchievement(string achievement)
        {
            if (HasAchievement(achievement))
                SteamUserStats.ClearAchievement(achievement);
        }

        public bool HasAchievement(string achievement)
        {
            SteamUserStats.GetAchievement(achievement, out bool achievementCompleted);
            return achievementCompleted;
        }

        public bool ClearStatsAndAchievements() 
            => SteamUserStats.ResetAllStats(true);

        public string GetSteamID() 
            => SteamUser.GetSteamID().ToString();

        public int GetStat(string key)
        {
            if (!SteamUserStats.GetStat(key, out int value))
                value = 0;
            
            return value;
        }

        public void SetStat(string key, int value)
        {
#if UNITY_EDITOR
            Debug.Log($"[Steamworks.NET] SetStat: {key} = {value.ToString()}");
#endif
            SteamUserStats.SetStat(key, value);
            SteamUserStats.StoreStats();
        }
    }
}