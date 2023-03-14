using Game.Cameras;
using UnityEngine;

namespace Game.Hero
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTarget;
        [SerializeField]
        private Transform petPosition;
        
        private SceneCamera _sceneCamera;

        public Transform CameraTarget => cameraTarget;
        public Transform PetPosition => petPosition;
    }
}