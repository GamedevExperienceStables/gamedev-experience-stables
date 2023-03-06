using UnityEngine;

namespace Game.Audio
{
    public class LocationAudioListener : MonoBehaviour
    {
        [SerializeField]
        private Transform attenuationObject;

        private bool _hasTarget;
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        public void ClearTarget()
        {
            _target = null;
            _hasTarget = false;
        }

        public void LateUpdate()
        {
            if (!_hasTarget)
                return;

            CopyPositionToTarget();
        }

        private void CopyPositionToTarget()
            => attenuationObject.position = _target.position;
    }
}