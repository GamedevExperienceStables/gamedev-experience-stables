using Cinemachine;
using Game.GameFlow;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FollowSceneCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachine;
        private CinemachineFramingTransposer _framingTransposer;
        
        private CameraSettings _settings;

        [Inject]
        public void Construct(CameraSettings settings) 
            => _settings = settings;

        private void Awake()
        {
            _cinemachine = GetComponent<CinemachineVirtualCamera>();
            _framingTransposer = _cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
            _framingTransposer.m_CameraDistance = _settings.DefaultDistance;
        }

        public void SetTarget(Transform target)
        {
            Debug.Assert(target, "target == null");

            _cinemachine.PreviousStateIsValid = false;
            _cinemachine.Follow = target;
        }

        public void ClearTarget()
            => _cinemachine.Follow = null;

        public void ZoomIn()
            => _framingTransposer.m_CameraDistance = _settings.CloseDistance;

        public void ZoomOut() 
            => _framingTransposer.m_CameraDistance = _settings.FarDistance;

        public void ZoomReset() 
            => _framingTransposer.m_CameraDistance = _settings.DefaultDistance;
    }
}