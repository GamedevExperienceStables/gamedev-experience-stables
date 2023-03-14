using UnityEngine;

namespace Game.Utils
{
    [DisallowMultipleComponent]
    public class TriggerMarker : MonoBehaviour
    {
        private const float FILL_OPACITY_MULTIPLIER = 0.8f;

        [SerializeField]
        private Color gizmoColor = Color.green;

        [SerializeField]
        private bool show = true;

        private void OnDrawGizmos()
        {
            if (!show)
                return;
            
            if (!TryGetComponent(out Collider col) || !col.isTrigger)
                return;

            Color outlineGizmoColor = gizmoColor * FILL_OPACITY_MULTIPLIER;

            Draw(col, gizmoColor, outlineGizmoColor);
        }

        private static void Draw(Collider col, Color fillColor, Color outlineColor)
        {
            switch (col)
            {
                case SphereCollider sphere:
                    DrawSphere(sphere, fillColor, outlineColor);
                    break;

                case BoxCollider box:
                    DrawBox(box, fillColor, outlineColor);
                    break;

                case CapsuleCollider capsule:
                    DrawCapsule(capsule, fillColor, outlineColor);
                    break;
            }
        }

        private static void DrawSphere(SphereCollider sphere, Color fillColor, Color outlineColor)
        {
            Gizmos.matrix = sphere.transform.localToWorldMatrix;

            Gizmos.color = fillColor;
            Gizmos.DrawSphere(sphere.center, sphere.radius);

            Gizmos.color = outlineColor;
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }

        private static void DrawBox(BoxCollider box, Color fillColor, Color outlineColor)
        {
            Gizmos.matrix = box.transform.localToWorldMatrix;

            Gizmos.color = fillColor;
            Gizmos.DrawCube(box.center, box.size);

            Gizmos.color = outlineColor;
            Gizmos.DrawWireCube(box.center, box.size);
        }

        private static void DrawCapsule(CapsuleCollider capsule, Color fillColor, Color outlineColor)
        {
            Transform capsuleTransform = capsule.transform;
            
            Gizmos.matrix = capsuleTransform.localToWorldMatrix;
            Vector3 half = capsuleTransform.up * capsule.height * 0.5f;
            
            Vector3 startPosition = Vector3.zero - half;
            Vector3 endPosition = Vector3.zero + half;

            DebugExtensions.DrawCapsule(startPosition, endPosition, fillColor, capsule.radius);
        }
    }
}