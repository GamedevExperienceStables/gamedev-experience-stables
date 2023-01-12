using UnityEngine;

namespace Game.Utils
{
    [DisallowMultipleComponent]
    public class SphereMarker : MonoBehaviour
    {
        private const float FILL_OPACITY_MULTIPLIER = 0.8f;

        [SerializeField]
        private Color gizmoColor = Color.green;

        private void OnDrawGizmos()
        {
            if (!TryGetComponent(out SphereCollider sphere))
                return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = gizmoColor * FILL_OPACITY_MULTIPLIER;
            Gizmos.DrawSphere(sphere.center, sphere.radius);
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }
    }
}