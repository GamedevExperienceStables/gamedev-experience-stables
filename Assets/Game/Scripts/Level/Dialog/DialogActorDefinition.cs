using UnityEngine;
using UnityEngine.Localization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Dialog/Actor")]
    public class DialogActorDefinition : ScriptableObject
    {
        [SerializeField]
        private LocalizedString displayName;

        [SerializeField]
        private Sprite image;

        public LocalizedString DisplayName => displayName;

        public Sprite Image => image;
    }
}