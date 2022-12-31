using Cinemachine;
using UnityEngine;

namespace Game.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FollowSceneCamera : MonoBehaviour
    {
        [SerializeField]
        private float closeDistance = 8f;

        [SerializeField]
        private float farDistance = 15f;

        private float _initialDistance;

        private CinemachineVirtualCamera _cinemachine;
        private CinemachineFramingTransposer _framingTransposer;

        private void Awake()
        {
            _cinemachine = GetComponent<CinemachineVirtualCamera>();
            _framingTransposer = _cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
            _initialDistance = _framingTransposer.m_CameraDistance;
        }

        public void SetTarget(Transform target)
        {
            Debug.Assert(target, "target == null");

            _cinemachine.Follow = target;
        }

        public void ClearTarget()
        {
            _cinemachine.Follow = null;
        }

        public void ZoomOut()
        {
            _framingTransposer.m_CameraDistance = farDistance;
        }

        public void ZoomIn()
        {
            _framingTransposer.m_CameraDistance = closeDistance;
        }

        public void ZoomReset()
        {
            _framingTransposer.m_CameraDistance = _initialDistance;
        }
    }
}