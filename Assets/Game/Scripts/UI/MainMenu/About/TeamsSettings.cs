using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Settings/Teams")]
    public class TeamsSettings : ScriptableObject
    {
        [SerializeField]
        private List<TeamData> teams;

        public List<TeamData> Teams => teams;
    }
}