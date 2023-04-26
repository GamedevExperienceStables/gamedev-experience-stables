using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Level
{
    public class MagnetSystem : IMagnetSystem, ITickable
    {
        private readonly List<MagnetTask> _updates = new();

        private readonly Settings _settings;

        [Inject]
        public MagnetSystem(Settings settings)
            => _settings = settings;

        public void Tick()
        {
            float delta = Time.deltaTime;

            Update(delta);
        }

        private void Update(float delta)
        {
            for (int i = _updates.Count - 1; i >= 0; i--)
            {
                _updates[i].Execute(delta);
                
                if (_updates[i].Completed)
                    _updates.RemoveAt(i);
            }
        }

        public UniTask StartPullAsync(Transform source, Transform target)
        {
            var completion = new UniTaskCompletionSource();
            var magnet = new MagnetTask(source, target, _settings.Power, _settings.MinDistance, completion);
            _updates.Add(magnet);

            return completion.Task;
        }

        [Serializable]
        public class Settings
        {
            [SerializeField]
            private float power = 10f;

            [SerializeField]
            private float minDistance = 0.5f;

            public float Power => power;
            public float MinDistance => minDistance;
        }
    }
}