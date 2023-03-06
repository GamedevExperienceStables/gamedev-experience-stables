using UnityEngine;

namespace Game.Audio
{
    public class SurfaceMaterialObject : MonoBehaviour
    {
        [SerializeField]
        private SurfaceType type;

        public SurfaceType Type => type;
    }
}