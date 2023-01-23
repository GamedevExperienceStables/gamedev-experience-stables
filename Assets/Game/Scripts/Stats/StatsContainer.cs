using UnityEngine;

namespace Game.Stats
{
    public class StatsContainer
    {
        public bool HasStat(CharacterStats stat)
        {
            return true;
        }

        public void AddValue(CharacterStats targetStat, float value)
        {
            Debug.Log($"+{value} {targetStat}");
        }
    }
}