using UnityEngine;

namespace Game.Cameras
{
    public class SceneCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        
        public Vector3 TransformDirection(Vector3 direction)
        {
            return mainCamera.transform.TransformDirection(direction);
        }

        public Ray ScreenPointToRay(Vector2 position)
        {
           return mainCamera.ScreenPointToRay(position);
        }
    }
}