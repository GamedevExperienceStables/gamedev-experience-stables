using System.Collections.Generic;
using UnityEngine;

namespace Game.Actors
{
    public class ActorEffects
    {
        private readonly List<Effect> _effects = new();
        private readonly List<Effect> _effectsToAdd = new();

        private IActorController _owner;

        public void Init(IActorController owner)
            => _owner = owner;

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public bool TryGetEffect(StatusDefinition status, out Effect effect)
        {
            foreach (Effect existed in _effects)
            {
                if (!ReferenceEquals(existed.Definition.Status, status)) 
                    continue;
                
                effect = existed;
                return true;
            }

            effect = default;
            return false;
        }

        public void Execute(Effect effect)
            => effect.Execute(_owner);

        public void Add(Effect effect) 
            => _effectsToAdd.Add(effect);

        public void Cancel(StatusDefinition status)
        {
            CancelByStatus(status, _effects);
            CancelByStatus(status, _effectsToAdd);
        }

        public void CancelAll(object instigator)
        {
            CancelByInstigator(instigator, _effects);
            CancelByInstigator(instigator, _effectsToAdd);
        }

        public void CancelAll()
        {
            foreach (Effect effect in _effects)
                effect.Cancel();

            _effects.Clear();
            _effectsToAdd.Clear();
        }

        public void Tick()
        {
            AddEffects();
            RemoveEffects();
        }

        private void RemoveEffects()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                if (_effects[i].IsCanceled)
                    _effects.RemoveAt(i);
            }
        }

        private void AddEffects()
        {
            for (int i = _effectsToAdd.Count - 1; i >= 0; i--)
            {
                Effect effect = _effectsToAdd[i];
                if (effect.IsCanceled)
                    continue;

                _effects.Add(effect);
                Execute(effect);
            }

            _effectsToAdd.Clear();
        }

        private static void CancelByInstigator(object instigator, List<Effect> list)
        {
            foreach (Effect effect in list)
                if (ReferenceEquals(effect.Instigator, instigator))
                    effect.Cancel();
        }

        private static void CancelByStatus(Object status, List<Effect> list)
        {
            foreach (Effect effect in list)
                if (ReferenceEquals(effect.Definition.Status, status))
                    effect.Cancel();
        }
    }
}