using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Inventory
{
    public class MagnetSystem : IMagnetSystem, ITickable
    {
        private readonly List<MagnetTask> _updates = new();
        private readonly List<MagnetTask> _toRemove = new();

        private readonly Settings _settings;

        [Inject]
        public MagnetSystem(Settings settings)
            => _settings = settings;

        public void Tick()
        {
            float delta = Time.deltaTime;

            Update(delta);
            Cleanup();
        }

        private void Update(float delta)
        {
            foreach (MagnetTask task in _updates)
            {
                if (task.Completed)
                {
                    _toRemove.Add(task);
                    continue;
                }

                task.Execute(delta);
            }
        }

        private void Cleanup()
        {
            for (int i = _toRemove.Count - 1; i >= 0; i--)
            {
                MagnetTask task = _toRemove[i];

                _updates.Remove(task);
                _toRemove.Remove(task);
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
            [field: SerializeField]
            public float Power { get; private set; } = 10f;

            [field: SerializeField]
            public float MinDistance { get; private set; } = 0.5f;
        }
    }
}