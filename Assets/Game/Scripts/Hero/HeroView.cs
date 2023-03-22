using System.Collections.Generic;
using Game.Cameras;
using UnityEngine;

namespace Game.Hero
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTarget;
        
        [SerializeField]
        private List<Transform> petPoints;
        
        private SceneCamera _sceneCamera;

        public Transform CameraTarget => cameraTarget;
        
        public IList<Transform> PetPoints => petPoints;
    }
}