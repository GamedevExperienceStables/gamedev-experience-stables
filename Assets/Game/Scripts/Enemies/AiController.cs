using Game.Actors;
using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace Game.Enemies
{
    public class AiController : MonoBehaviour, IActorInputController
    {
        private Transform _target;

        private float _sensorDistance;
        private float _attackRange;
        private float _attackInterval;

        private BehaviourTreeOwner _brain;
        private bool _hasBrain;

        private readonly ActorBlock _block = new();

        public Transform Target => _target;
        public float SensorDistance => _sensorDistance;
        public float AttackRange => _attackRange;
        public float AttackInterval => _attackInterval;

        private void Awake()
            => _hasBrain = TryGetComponent(out _brain);

        public void InitSensor(EnemyStats.InitialStats stats)
            => _sensorDistance = stats.SensorDistance;

        public void SetTarget(Transform target)
            => _target = target;

        public void SetAttackParameters(float range, float interval)
        {
            _attackRange = range;
            _attackInterval = interval;
        }

        public void SetBlock(bool isBlocked)
        {
            if (isBlocked)
                SetBlock(InputBlockExtensions.FULL_BLOCK);
            else 
                RemoveBlock(InputBlockExtensions.FULL_BLOCK);
        }

        public void SetBlock(InputBlock input)
        {
            if (!_hasBrain) 
                return;
            
            _block.SetBlock(input);
            if (_block.IsBlocked(InputBlockExtensions.FULL_BLOCK))
                _brain.PauseBehaviour();
        }

        public void RemoveBlock(InputBlock input)
        {
            if (!_hasBrain) 
                return;
            
            _block.RemoveBlock(input);
            if (!_block.IsBlocked(InputBlockExtensions.FULL_BLOCK) && _brain.isPaused)
                _brain.StartBehaviour();
        }

        public Vector3 GetTargetPosition(bool grounded = false)
            => _target.position;

        public Vector3 DesiredDirection => transform.forward;
    }
}