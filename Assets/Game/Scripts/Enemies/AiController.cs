using Game.Actors;
using UnityEngine;

namespace Game.Enemies
{
    public class AiController : MonoBehaviour, IActorInputController
    {
        private Transform _target;

        private float _sensorDistance;
        private float _attackRange;
        private float _attackInterval;
        private bool _inputBlocked;

        public Transform Target => _target;
        public float SensorDistance => _sensorDistance;
        public float AttackRange => _attackRange;
        public float AttackInterval => _attackInterval;
        
        public void InitSensor(EnemyStats.InitialStats stats)
            => _sensorDistance = stats.SensorDistance;

        public void SetTarget(Transform target)
            => _target = target;

        public void SetAttackParameters(float range, float interval)
        {
            _attackRange = range;
            _attackInterval = interval;
        }

        public void BlockInput(bool isBlocked) 
            => _inputBlocked = isBlocked;
    }
}