using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Location/Location")]
    public class LocationDefinition : ScriptableObject
    {
        [SerializeField, Scene]
        private string sceneName;

        public string SceneName => sceneName;
    }
}