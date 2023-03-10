using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = "Status")]
    public class StatusDefinition : ScriptableObject
    {
        [SerializeField]
        private GameObject statusPrefab;

        public GameObject StatusPrefab => statusPrefab;
    }
}