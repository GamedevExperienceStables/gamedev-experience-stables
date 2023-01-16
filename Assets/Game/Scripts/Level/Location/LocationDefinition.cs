using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Location")]
    public class LocationDefinition : ScriptableObject
    {
        [SerializeField]
        private string sceneName;

        public string SceneName => sceneName;
    }
}