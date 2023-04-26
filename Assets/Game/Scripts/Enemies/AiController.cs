using Game.Actors;
using JetBrains.Annotations;
using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace Game.Enemies
{
    public sealed class AiController : ActorInputController
    {
        private Transform _target;

        private float _sensorDistance;
        private float _attackRange;
        private float _attackInterval;

        private BehaviourTreeOwner _brain;
        private bool _hasBrain;

        [UsedImplicitly]
        public Transform Target => _target;

        [UsedImplicitly]
        public float SensorDistance => _sensorDistance;

        [UsedImplicitly]
        public float AttackRange => _attackRange;

        [UsedImplicitly]
        public float AttackInterval => _attackInterval;

        public override Vector3 DesiredDirection => transform.forward;

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

        public override void SetBlock(InputBlock input)
        {
            base.SetBlock(input);

            if (!_hasBrain)
                return;

            if (block.HasAny(InputBlockExtensions.ALL))
                _brain.PauseBehaviour();
        }

        public override void RemoveBlock(InputBlock input)
        {
            base.RemoveBlock(input);

            if (!_hasBrain)
                return;

            if (_brain.isPaused && !block.HasAny(InputBlockExtensions.ALL)) 
                _brain.StartBehaviour();
        }

        public override Vector3 GetTargetPosition(bool grounded = false)
            => _target.position;
    }
}