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
            => _effectsToAdd.Add(effect);

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
            RemoveEffects();
        }

        private void AddEffects()
        {
            for (int i = _effectsToAdd.Count - 1; i >= 0; i--)
            {
                if (_effectsToAdd[i].IsCanceled)
                    continue;
                
                Effect effect = _effectsToAdd[i];
                
                StatusDefinition status = effect.Definition.Status;
                if (status)
                {
                    _status.AddStatus(status);
                    
                    SuppressEffectIfExisted(status);
                }

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

                StatusDefinition status = effect.Definition.Status;
                if (status)
                {
                    _status.RemoveStatus(status);

                    UnSuppressEffectIfExisted(status);
                }
            }
        }

        private void SuppressEffectIfExisted(StatusDefinition status)
        {
            foreach (Effect existed in _effects)
            {
                if (ReferenceEquals(existed.Definition.Status, status) && !existed.IsCanceled) 
                    existed.Suppress();
            }

            // if (TryGetEffect(definition.Status, out Effect existed) && !existed.IsCanceled)
            //     existed.Suppress();
        }

        private void UnSuppressEffectIfExisted(StatusDefinition status)
        {
            if (TryGetEffect(status, out Effect existed) && !existed.IsCanceled)
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