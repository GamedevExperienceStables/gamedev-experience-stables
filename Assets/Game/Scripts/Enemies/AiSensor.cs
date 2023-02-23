using Game.Hero;
using Game.Utils;
using UnityEngine;

namespace Game.Enemies
{
    public class AiSensor : MonoBehaviour
    {
        private float _distance;
        private float _height;
        
        private readonly Color _color = Color.red;

        public void InitSensor(EnemyStats.InitialStats stats)
        {
            _distance = stats.SensorDistance;
            _height = stats.SensorHeight;
        }

        public bool Scan(Transform target)
        {
            float sqrDistance = (transform.position - target.position).sqrMagnitude;
            float sqrScanDistance = _distance * _distance;
            
            
            if (sqrDistance <= sqrScanDistance && FindHero(target))
                return true;
            
            return false;
        }

        private bool FindHero(Transform target)
        {
            Vector3 origin = transform.position;
            Vector3 dest = target.position;
            Vector3 direction = dest - origin;
            
            if (direction.y < 0 || direction.y > _height) 
                return false;

            dest.y = origin.y;
            if (!Physics.Linecast(origin, direction, LayerMasks.Player, QueryTriggerInteraction.Ignore))
                return true;
            
            return false;
        }

        private void OnDrawGizmos()
        {
            DrawCylinder();
        }
        
        private void DrawCylinder()
        {
            Vector3 position = transform.position;
            DebugExtensions.DrawCylinder(position, new Vector3(position.x, _height, position.z), _color,
                _distance);
        }
    }
}