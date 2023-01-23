using Game.Cameras;
using UnityEngine;

namespace Game.Hero
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTarget;

        private SceneCamera _sceneCamera;

        public Transform CameraTarget => cameraTarget;
    }
}