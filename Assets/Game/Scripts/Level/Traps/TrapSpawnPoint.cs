using Game.Utils;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TrapSpawnPoint : MonoBehaviour
    {
        private const float RAYCAST_DISTANCE = 1f;

        [SerializeField, Required]
        private TrapDynamicDefinition trap;

        private TrapFactory _factory;

        [Inject]
        public void Construct(TrapFactory factory)
            => _factory = factory;

        public void Activate()
        {
            TrapView instance = _factory.Create(trap.Prefab, trap.Definition);

            Transform self = transform;
            
            Vector3 spawnPoint = self.position;
            Quaternion spawnRotation = self.rotation;
            Vector3 normal = Vector3.up;

            if (trap.SnapToGround)
                SnapToGround(self, ref spawnPoint, ref normal);

            if (trap.RandomizeRotationY)
                spawnRotation = RandomizeY(spawnRotation, normal);

            instance.transform.SetPositionAndRotation(spawnPoint, spawnRotation);
        }

        private static Quaternion RandomizeY(Quaternion spawnRotation, Vector3 normal)
        {
            spawnRotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), normal);
            return spawnRotation;
        }

        private void SnapToGround(Transform self, ref Vector3 spawnPoint, ref Vector3 normal)
        {
            if (Physics.Raycast(self.position, Vector3.down, out RaycastHit hit, RAYCAST_DISTANCE,
                    LayerMasks.GroundLayers, QueryTriggerInteraction.Ignore))
            {
                spawnPoint = hit.point;

                if (trap.AlignToGround)
                    normal = hit.normal;
            }
        }
    }
}