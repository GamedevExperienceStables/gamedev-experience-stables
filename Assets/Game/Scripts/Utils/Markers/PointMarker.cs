using UnityEngine;

namespace Game.Utils
{
    [DisallowMultipleComponent]
    public class PointMarker : MonoBehaviour
    {
        [SerializeField]
        private Color gizmoColor = Color.yellow;

        [SerializeField]
        private float radius = 0.2f;

        [SerializeField]
        private float height = 2f;

        private void OnDrawGizmos()
        {
            Transform t = transform;
            Vector3 position = t.position + Vector3.up * 0.01f;

            DebugExtensions.DrawArrow(position, t.forward * radius, gizmoColor);
            DebugExtensions.DrawPoint(position, gizmoColor, 0.05f);
            DebugExtensions.DrawCircle(position, gizmoColor, 0.05f);
        }

        private void OnDrawGizmosSelected()
        {
            Transform t = transform;
            Vector3 position = t.position;
            DebugExtensions.DrawCylinder(position, position + t.up * height, gizmoColor, radius);
        }
    }
}