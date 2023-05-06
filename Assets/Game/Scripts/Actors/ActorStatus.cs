using System.Collections.Generic;
using UnityEngine;


namespace Game.Actors
{
    public class ActorStatus
    {
        private readonly Dictionary<StatusDefinition, Status> _statuses = new();

        private readonly IActorController _owner;

        public ActorStatus(IActorController owner)
            => _owner = owner;

        public void AddStatus(StatusDefinition status)
        {
            if (_statuses.TryGetValue(status, out Status instance))
            {
                instance.Increase();
                return;
            }

            AttachStatus(status);
        }

        private void AttachStatus(StatusDefinition definition)
        {
            Status instance;
            if (definition.StatusPrefab)
            {
                GameObject view = CreateView(definition);
                instance = new Status(definition, view);
            }
            else
            {
                instance = new Status(definition);
            }

            _statuses.Add(definition, instance);
        }

        public void RemoveStatus(StatusDefinition status)
        {
            if (!_statuses.TryGetValue(status, out Status instance))
                return;

            instance.Decrease();

            if (instance.Count <= 0)
                DestroyStatus(status, instance);
        }

        private void DestroyStatus(StatusDefinition status, Status instance)
        {
            _statuses.Remove(status);
            DestroyView(instance);
        }


        private GameObject CreateView(StatusDefinition status)
        {
            Vector3 position = _owner.Transform.position + status.Offset;
            return Object.Instantiate(status.StatusPrefab, position, Quaternion.identity, _owner.Transform);
        }

        private static void DestroyView(Status status)
        {
            if (status.HasView)
                Object.Destroy(status.View);
        }

        public void Clear()
        {
            foreach (Status status in _statuses.Values)
                DestroyView(status);

            _statuses.Clear();
        }
    }
}