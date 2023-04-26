using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Location/Location")]
    public class LocationDefinition : ScriptableObject, ILocationDefinition
    {
        [SerializeField, Scene]
        private string sceneName;

        [SerializeField]
        private Sprite mapImage;

        public string SceneName => sceneName;

        public Sprite MapImage => mapImage;
    }
}