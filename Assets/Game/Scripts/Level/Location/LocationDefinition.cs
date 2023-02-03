using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Location/Location")]
    public class LocationDefinition : ScriptableObject, ILocationDefinition
    {
        [SerializeField, Scene]
        private string sceneName;

        public string SceneName => sceneName;
    }
}