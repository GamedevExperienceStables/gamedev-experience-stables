using UnityEngine;
using VContainer;

namespace Game.UI
{
    public class LocationMarker : MonoBehaviour, ILocationMarker
    {
        [SerializeField]
        private MapMarkerDefinition definition;

        private Transform _transform;
        private LocationMarkers _markers;

        public Sprite Icon => definition.Icon;
        public Vector3 Position => _transform.position;

        [Inject]
        public void Construct(LocationMarkers markers)
            => _markers = markers;

        private void Awake()
            => _transform = GetComponent<Transform>();

        private void Start()
            => _markers?.Add(this);

        private void OnDestroy()
            => _markers?.Remove(this);
    }
}