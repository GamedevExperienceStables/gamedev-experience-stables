using Game.Utils.DataTable;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Location/Location")]
    public class LocationDefinition : DataTableItemDefinition, ILocationDefinition
    {
        [SerializeField, Scene]
        private string sceneName;

        [SerializeField]
        private Sprite mapImage;

        public string SceneName => sceneName;

        public Sprite MapImage => mapImage;
    }
}