using UnityEngine;

namespace Game.Cameras
{
    public class SceneCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        public Vector3 TransformDirection(Vector2 direction)
        {
            var planeDirection = new Vector3(direction.x, 0, direction.y);
            return Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * planeDirection;
        }

        public Ray ScreenPointToRay(Vector2 position)
        {
            return mainCamera.ScreenPointToRay(position);
        }
    }
}