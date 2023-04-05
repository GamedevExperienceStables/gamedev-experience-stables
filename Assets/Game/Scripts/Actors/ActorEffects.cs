using System.Collections.Generic;

namespace Game.Actors
{
    public class ActorEffects
    {
        private readonly List<Effect> _effects = new();
        private readonly List<Effect> _effectsToAdd = new();

        private readonly ActorStatus _status;
        private readonly IActorController _owner;

        public ActorEffects(IActorController owner)
        {
            _owner = owner;

            _status = new ActorStatus(owner);
        }

        public void Apply(Effect effect)
            => effect.Execute(_owner);

        public void Add(Effect effect)
        {
            if (effect.Definition.Status)
                _status.CreateStatus(effect.Definition.Status);

            SuppressEffectIfExisted(effect.Definition);

            _effectsToAdd.Add(effect);
        }

        public void CancelAll(object instigator, IEnumerable<EffectDefinition> toCancel)
        {
            foreach (EffectDefinition definition in toCancel)
            {
                foreach (Effect effect in _effects)
                {
                    if (ReferenceEquals(effect.Instigator, instigator) &&
                        ReferenceEquals(effect.Definition, definition))
                        effect.Cancel();
                }
            }
        }

        public void CancelAll()
        {
            foreach (Effect effect in _effects)
                effect.Cancel();

            _status.Clear();
            _effects.Clear();
            _effectsToAdd.Clear();
        }

        public void Tick()
        {
            AddEffects();
            RemoveStatus();
            RemoveEffects();
        }

        private void AddEffects()
        {
            for (int i = _effectsToAdd.Count - 1; i >= 0; i--)
            {
                Effect effect = _effectsToAdd[i];
                if (effect.IsCanceled)
                    continue;

                _effects.Add(effect);
                Apply(effect);
            }

            _effectsToAdd.Clear();
        }

        private void RemoveEffects()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                if (!_effects[i].IsCanceled)
                    continue;

                Effect effect = _effects[i];
                _effects.RemoveAt(i);

                UnSuppressEffectIfExisted(effect.Definition);
            }
        }

        private void RemoveStatus()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                if (!_effects[i].IsCanceled)
                    continue;

                Effect effect = _effects[i];
                _status.Remove(effect.Definition.Status);
            }
        }

        private void SuppressEffectIfExisted(EffectDefinition definition)
        {
            if (TryGetEffect(definition.Status, out Effect existed) && !existed.IsCanceled)
                existed.Suppress();
        }

        private void UnSuppressEffectIfExisted(EffectDefinition definition)
        {
            if (TryGetEffect(definition.Status, out Effect existed) && !existed.IsCanceled)
                existed.UnSuppress();
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        private bool TryGetEffect(StatusDefinition status, out Effect effect)
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
    }
}