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
        private Collider[] _colliders = new Collider[50];

        public void InitSensor(EnemyStats.InitialStats stats)
        {
            _distance = stats.SensorDistance;
            _height = stats.SensorHeight;
        }

        public void Update()
        {
            Scan();
        }

        public bool Scan()
        {
           int count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders);

           for (int i = 0; i < count; i++)
           {
               GameObject gameObject = _colliders[i].gameObject;

               if (IsInSight(gameObject))
               {
                   Debug.Log("Enemy find player");
                   return true;
               }
           }
           return false;
        }

        private bool IsInSight(GameObject gameObject)
        {
            Vector3 origin = transform.position;
            Vector3 dest = gameObject.transform.position;
            Vector3 direction = dest - origin;
            
            if (direction.y < 0 || direction.y > _height) 
                return false;

            origin.y += _height / 2;
            dest.y = origin.y;
            return !Physics.Linecast(origin, direction) && gameObject.GetComponent<HeroController>();
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