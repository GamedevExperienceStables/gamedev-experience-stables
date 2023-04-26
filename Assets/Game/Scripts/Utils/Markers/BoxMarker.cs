using UnityEngine;

namespace Game.Utils
{
    [DisallowMultipleComponent]
    public class BoxMarker : MonoBehaviour
    {
        private const float FILL_OPACITY_MULTIPLIER = 0.8f;

        [SerializeField]
        private Color gizmoColor = Color.green;

        private void OnDrawGizmos()
        {
            if (!TryGetComponent(out BoxCollider box))
                return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = gizmoColor * FILL_OPACITY_MULTIPLIER;
            Gizmos.DrawCube(box.center, box.size);
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(box.center, box.size);
        }
    }
}