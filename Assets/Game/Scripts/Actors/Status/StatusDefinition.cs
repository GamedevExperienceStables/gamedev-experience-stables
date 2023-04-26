using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = "Status")]
    public class StatusDefinition : ScriptableObject
    {
        [SerializeField]
        private GameObject statusPrefab;

        [SerializeField]
        private Vector3 offset;

        public GameObject StatusPrefab => statusPrefab;

        public Vector3 Offset => offset;
    }
}